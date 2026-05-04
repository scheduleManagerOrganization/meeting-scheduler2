using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class AddAvailabilityForm : Form
    {
        // 🌟 동적으로 추가되는 시간대 세트를 관리하기 위한 클래스
        private class TimeSlotRow
        {
            public Panel Panel { get; set; }
            public DateTimePicker DtpStart { get; set; }
            public ComboBox CmbStartH { get; set; }
            public ComboBox CmbStartM { get; set; }
            public DateTimePicker DtpEnd { get; set; }
            public ComboBox CmbEndH { get; set; }
            public ComboBox CmbEndM { get; set; }
        }

        private List<TimeSlotRow> _rows = new List<TimeSlotRow>();

        public AddAvailabilityForm()
        {
            InitializeComponent();

            // 폼이 켜질 때 기본으로 1개의 세트를 무조건 추가해 둡니다.
            AddNewTimeSlotRow();
        }

        // 🌟 [+] 버튼 클릭 시 새로운 세트를 아래에 추가
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            AddNewTimeSlotRow();
        }

        // 🌟 동적 UI 생성 핵심 로직
        private void AddNewTimeSlotRow()
        {
            Panel rowPanel = new Panel
            {
                Size = new Size(420, 80),
                BackColor = Color.WhiteSmoke,
                Margin = new Padding(5, 5, 5, 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // --- 시작 줄 ---
            Label lblStart = new Label { Text = "시작", Location = new Point(10, 15), AutoSize = true, Font = new Font("맑은 고딕", 9, FontStyle.Bold) };
            DateTimePicker dtpStart = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(50, 12), Width = 110 };
            ComboBox cmbStartH = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(165, 12), Width = 50 };
            Label lblCol1 = new Label { Text = ":", Location = new Point(215, 14), AutoSize = true, Font = new Font("맑은 고딕", 10, FontStyle.Bold) };
            ComboBox cmbStartM = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(230, 12), Width = 50 };

            // --- 종료 줄 ---
            Label lblEnd = new Label { Text = "종료", Location = new Point(10, 45), AutoSize = true, Font = new Font("맑은 고딕", 9, FontStyle.Bold) };
            DateTimePicker dtpEnd = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(50, 42), Width = 110 };
            ComboBox cmbEndH = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(165, 42), Width = 50 };
            Label lblCol2 = new Label { Text = ":", Location = new Point(215, 44), AutoSize = true, Font = new Font("맑은 고딕", 10, FontStyle.Bold) };
            ComboBox cmbEndM = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(230, 42), Width = 50 };

            // --- 삭제 (X) 버튼 ---
            Button btnRemove = new Button { Text = "X", Location = new Point(370, 25), Width = 30, Height = 30, BackColor = Color.LightCoral, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Click += (s, e) => {
                if (_rows.Count > 1)
                { // 최소 1개는 남겨둠
                    flpTimeSlots.Controls.Remove(rowPanel);
                    _rows.RemoveAll(r => r.Panel == rowPanel);
                }
                else
                {
                    MessageBox.Show("최소 1개의 시간대는 입력해야 합니다.", "알림");
                }
            };

            // 콤보박스 데이터 채우기
            for (int h = 0; h < 24; h++) { cmbStartH.Items.Add($"{h:D2}"); cmbEndH.Items.Add($"{h:D2}"); }
            for (int m = 0; m < 60; m += 10) { cmbStartM.Items.Add($"{m:D2}"); cmbEndM.Items.Add($"{m:D2}"); }

            // 기본값
            cmbStartH.SelectedIndex = 9; cmbStartM.SelectedIndex = 0;
            cmbEndH.SelectedIndex = 18; cmbEndM.SelectedIndex = 0;

            rowPanel.Controls.AddRange(new Control[] { lblStart, dtpStart, cmbStartH, lblCol1, cmbStartM, lblEnd, dtpEnd, cmbEndH, lblCol2, cmbEndM, btnRemove });

            // FlowLayoutPanel에 추가하면 알아서 아래로 쌓이고 스크롤이 생김
            flpTimeSlots.Controls.Add(rowPanel);

            // 관리용 리스트에 저장
            _rows.Add(new TimeSlotRow
            {
                Panel = rowPanel,
                DtpStart = dtpStart,
                CmbStartH = cmbStartH,
                CmbStartM = cmbStartM,
                DtpEnd = dtpEnd,
                CmbEndH = cmbEndH,
                CmbEndM = cmbEndM
            });
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // 모든 세트의 시간들을 날짜별로 묶어줄 딕셔너리
            var groupedSlots = new Dictionary<string, List<object>>();

            foreach (var row in _rows)
            {
                string startSlot = $"{row.CmbStartH.Text}:{row.CmbStartM.Text}";
                string endSlot = $"{row.CmbEndH.Text}:{row.CmbEndM.Text}";

                DateTime startDt = row.DtpStart.Value.Date.Add(TimeSpan.Parse(startSlot));
                DateTime endDt = row.DtpEnd.Value.Date.Add(TimeSpan.Parse(endSlot));

                // 시간 역전 방지
                if (startDt >= endDt)
                {
                    row.Panel.BackColor = Color.MistyRose; // 에러 난 곳 색상 변경
                    MessageBox.Show("종료 시간은 시작 시간 이후여야 합니다.", "시간 설정 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                row.Panel.BackColor = Color.WhiteSmoke; // 정상 복구

                // 며칠에 걸친 경우 하루 단위로 쪼개기
                DateTime current = startDt.Date;
                DateTime endLimit = endDt.Date;

                while (current <= endLimit)
                {
                    // 🌟 에러 수정 부분: 변수명 s, e 대신 currentStart, currentEnd 사용
                    string currentStart = (current == startDt.Date) ? startSlot : "00:00";
                    string currentEnd = (current == endLimit) ? endSlot : "23:59";

                    if (currentStart != currentEnd) // 유효한 시간대면 딕셔너리에 추가
                    {
                        string dateStr = current.ToString("yyyy-MM-dd");
                        if (!groupedSlots.ContainsKey(dateStr)) groupedSlots[dateStr] = new List<object>();

                        groupedSlots[dateStr].Add(new { start = currentStart, end = currentEnd });
                    }
                    current = current.AddDays(1);
                }
            }

            btnSave.Enabled = false;

            try
            {
                bool allSuccess = true;

                // 그룹핑된 날짜별로 API 호출
                foreach (var kvp in groupedSlots)
                {
                    var request = new
                    {
                        user_id = ApiService.CurrentUserId,
                        date = kvp.Key,
                        slots = kvp.Value.ToArray()
                    };

                    var response = await ApiService.PostAsync<object, ApiResponse<object>>("/api/availability", request);
                    if (response == null || !response.Success) allSuccess = false;
                }

                if (allSuccess)
                {
                    // 성공 다이얼로그 띄우고 모달 종료
                    MessageBox.Show("가능 시간대가 성공적으로 저장되었습니다.", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("일부 데이터 저장에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"통신 중 오류 발생: {ex.Message}", "네트워크 오류");
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}
