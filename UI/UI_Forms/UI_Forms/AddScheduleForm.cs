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
                        case "년": currentStart = currentStart.AddYears(interval); currentEnd = currentEnd.AddYears(interval); break;
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

                foreach (var block in scheduleBlocks)
                {
                    DateTime current = block.start.Date;
                    DateTime endLimit = block.end.Date;

                    while (current <= endLimit)
                    {
                        string slotStart = (current == block.start.Date) ? block.start.ToString("HH:mm") : "00:00";
                        string slotEnd = (current == endLimit) ? block.end.ToString("HH:mm") : "23:59";

                        if (slotStart == slotEnd) { current = current.AddDays(1); continue; }

                        string dateStr = current.ToString("yyyy-MM-dd");

                        // 🌟 변경점: 통째로 request 객체를 넘기지 않고 필요한 변수들을 각각 전달합니다.
                        saveTasks.Add(SaveSingleSlotAsync(ApiService.CurrentUserId, dateStr, txtTitle.Text, slotStart, slotEnd));
                        current = current.AddDays(1);
                    }
                }

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

        // 🌟 변경점: 기존 일정 GET -> 합치기 -> POST(덮어쓰기) 로직으로 전면 교체
        private async Task<bool> SaveSingleSlotAsync(string userId, string dateStr, string title, string start, string end)
        {
            try
            {
                List<object> mergedSlots = new List<object>();

                // 1단계: 기존 일정 불러오기 (GET)
                // (Models에 정의된 AvailabilityData의 이름이 다르다면 프로젝트에 맞게 수정해 주세요)
                var getResponse = await ApiService.GetAsync<ApiResponse<AvailabilityData>>($"/api/availability/{userId}/{dateStr}");

                if (getResponse != null && getResponse.Success && getResponse.Data != null && getResponse.Data.Slots != null)
                {
                    foreach (var existingSlot in getResponse.Data.Slots)
                    {
                        mergedSlots.Add(new
                        {
                            title = existingSlot.Title,
                            start = existingSlot.Start,
                            end = existingSlot.End
                        });
                    }
                }

                // 2단계: 새 일정 끼워 넣기
                mergedSlots.Add(new
                {
                    title = title,
                    start = start,
                    end = end
                });

                // 3단계: 통째로 다시 덮어쓰기 (POST)
                var payload = new
                {
                    user_id = userId,
                    date = dateStr,
                    slots = mergedSlots // 병합된 리스트 전송
                };

                var postResponse = await ApiService.PostAsync<object, ApiResponse<object>>("/api/availability", payload);
                return postResponse != null && postResponse.Success;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}