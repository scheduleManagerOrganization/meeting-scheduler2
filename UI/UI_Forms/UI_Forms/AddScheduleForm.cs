using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Forms.Models; // 기존 네임스페이스 유지

namespace UI_Forms
{
    public partial class AddScheduleForm : Form
    {
        private const string PlaceholderTitle = "일정 제목을 입력하세요";

        public AddScheduleForm()
        {
            InitializeComponent();
            SetupTimeComboBoxes();
            SetupPlaceholder();
            SetupRecurrenceControls(); // 반복 컨트롤 초기화 추가

            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today;
        }

        private void SetupPlaceholder()
        {
            txtTitle.Text = PlaceholderTitle;
            txtTitle.ForeColor = Color.Gray;

            txtTitle.Enter += (s, e) => {
                if (txtTitle.Text == PlaceholderTitle)
                {
                    txtTitle.Text = "";
                    txtTitle.ForeColor = Color.Black;
                }
            };

            txtTitle.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    txtTitle.Text = PlaceholderTitle;
                    txtTitle.ForeColor = Color.Gray;
                }
            };
        }

        private void SetupTimeComboBoxes()
        {
            cmbStartHour.Items.Clear();
            cmbEndHour.Items.Clear();
            cmbStartMinute.Items.Clear();
            cmbEndMinute.Items.Clear();

            for (int h = 0; h < 24; h++)
            {
                string hourStr = $"{h:D2}";
                cmbStartHour.Items.Add(hourStr);
                cmbEndHour.Items.Add(hourStr);
            }

            for (int m = 0; m < 60; m += 5)
            {
                string minuteStr = $"{m:D2}";
                cmbStartMinute.Items.Add(minuteStr);
                cmbEndMinute.Items.Add(minuteStr);
            }

            cmbStartHour.SelectedIndex = 9;
            cmbStartMinute.SelectedIndex = 0;
            cmbEndHour.SelectedIndex = 10;
            cmbEndMinute.SelectedIndex = 0;
        }

        // 🌟 반복 컨트롤 초기화 로직 추가
        private void SetupRecurrenceControls()
        {
            cmbRecurType.Items.AddRange(new string[] { "일", "주", "월", "년" });
            cmbRecurType.SelectedIndex = 1; // 기본값 "주"

            for (int i = 1; i <= 10; i++)
            {
                cmbRecurCount.Items.Add(i.ToString());
            }
            cmbRecurCount.SelectedIndex = 0; // 기본값 "1"

            dtpRecurEnd.Value = DateTime.Today.AddMonths(1);

            chkRecurrence.CheckedChanged += (s, e) =>
            {
                if (chkRecurrence.Checked)
                {
                    this.ClientSize = new Size(340, 335); // 폼 확장
                    pnlRecurrence.Visible = true;
                    btnSave.Top = 285;
                    btnCancel.Top = 285;
                }
                else
                {
                    pnlRecurrence.Visible = false;
                    this.ClientSize = new Size(340, 240); // 폼 축소
                    btnSave.Top = 190;
                    btnCancel.Top = 190;
                }
            };

            // 숫자 외 입력 방지
            cmbRecurCount.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == PlaceholderTitle)
            {
                MessageBox.Show("일정 제목을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string startSlot = $"{cmbStartHour.Text}:{cmbStartMinute.Text}";
            string endSlot = $"{cmbEndHour.Text}:{cmbEndMinute.Text}";

            DateTime startDateTime = dtpStartDate.Value.Date.Add(TimeSpan.Parse(startSlot));
            DateTime endDateTime = dtpEndDate.Value.Date.Add(TimeSpan.Parse(endSlot));

            if (startDateTime >= endDateTime)
            {
                MessageBox.Show("종료 시간은 시작 시간 이후여야 합니다.", "시간 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🌟 반복 날짜(일정 블록) 계산 로직
            List<(DateTime start, DateTime end)> scheduleBlocks = new List<(DateTime, DateTime)>();

            if (chkRecurrence.Checked)
            {
                if (!int.TryParse(cmbRecurCount.Text, out int interval) || interval <= 0)
                {
                    MessageBox.Show("반복 주기를 올바른 숫자로 입력해주세요.");
                    return;
                }

                string recurType = cmbRecurType.SelectedItem.ToString();
                DateTime recurEndLimit = dtpRecurEnd.Value.Date.AddDays(1).AddTicks(-1);

                DateTime currentStart = startDateTime;
                DateTime currentEnd = endDateTime;

                while (currentStart <= recurEndLimit)
                {
                    scheduleBlocks.Add((currentStart, currentEnd));

                    switch (recurType)
                    {
                        case "일": currentStart = currentStart.AddDays(interval); currentEnd = currentEnd.AddDays(interval); break;
                        case "주": currentStart = currentStart.AddDays(interval * 7); currentEnd = currentEnd.AddDays(interval * 7); break;
                        case "월": currentStart = currentStart.AddMonths(interval); currentEnd = currentEnd.AddMonths(interval); break;
                        case "연": currentStart = currentStart.AddYears(interval); currentEnd = currentEnd.AddYears(interval); break;
                    }
                }
            }
            else
            {
                scheduleBlocks.Add((startDateTime, endDateTime));
            }

            btnSave.Enabled = false;
            try
            {
                List<Task<bool>> saveTasks = new List<Task<bool>>();

                // 기존의 자정 쪼개기 로직을 유지하면서 Task 리스트에 담기
                foreach (var block in scheduleBlocks)
                {
                    DateTime current = block.start.Date;
                    DateTime endLimit = block.end.Date;

                    while (current <= endLimit)
                    {
                        string slotStart = (current == block.start.Date) ? block.start.ToString("HH:mm") : "00:00";
                        string slotEnd = (current == endLimit) ? block.end.ToString("HH:mm") : "23:59";

                        if (slotStart == slotEnd) { current = current.AddDays(1); continue; }

                        var request = new
                        {
                            user_id = ApiService.CurrentUserId, // 명세서에 맞춤 (user_id)
                            date = current.ToString("yyyy-MM-dd"),
                            slots = new[] { new { title = txtTitle.Text, start = slotStart, end = slotEnd } }
                        };

                        saveTasks.Add(SaveSingleSlotAsync(request));
                        current = current.AddDays(1);
                    }
                }

                // 🌟 모든 API 요청을 일괄 대기
                bool[] results = await Task.WhenAll(saveTasks);

                if (results.Length > 0 && results.All(r => r == true))
                {
                    MessageBox.Show("일정이 성공적으로 등록되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("일부 일정을 저장하는 데 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        // 개별 통신 헬퍼 메서드
        private async Task<bool> SaveSingleSlotAsync(object request)
        {
            try
            {
                var response = await ApiService.PostAsync<object, ApiResponse<object>>("/api/availability", request);
                return response != null && response.Success;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}