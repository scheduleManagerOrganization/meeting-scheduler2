using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Forms.Models;

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

        // 🌟 시간과 분을 따로 세팅하는 메서드 (5분 단위로 수정)
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

            // 분: 00, 05, 10, 15 ... 55 (5분 단위로 변경)
            for (int m = 0; m < 60; m += 5)
            {
                string minuteStr = $"{m:D2}";
                cmbStartMinute.Items.Add(minuteStr);
                cmbEndMinute.Items.Add(minuteStr);
            }

            cmbStartHour.SelectedIndex = 9;   // 09시
            cmbStartMinute.SelectedIndex = 0; // 00분
            cmbEndHour.SelectedIndex = 10;    // 10시
            cmbEndMinute.SelectedIndex = 0;   // 00분
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

            btnSave.Enabled = false;
            try
            {
                bool allSuccess = true;
                DateTime current = startDateTime.Date;
                DateTime endLimit = endDateTime.Date;

                while (current <= endLimit)
                {
                    string slotStart = (current == startDateTime.Date) ? startSlot : "00:00";
                    string slotEnd = (current == endLimit) ? endSlot : "23:59";

                    if (slotStart == slotEnd) { current = current.AddDays(1); continue; }

                    var request = new
                    {
                        userId = ApiService.CurrentUserId,
                        date = current.ToString("yyyy-MM-dd"),
                        slots = new[] { new { title = txtTitle.Text, start = slotStart, end = slotEnd } }
                    };

                    var response = await ApiService.PostAsync<object, ApiResponse<object>>("/api/availability", request);
                    if (response == null || !response.Success) allSuccess = false;

                    current = current.AddDays(1);
                }

                if (allSuccess)
                {
                    MessageBox.Show("일정이 등록되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else MessageBox.Show("일부 날짜 저장에 실패했습니다.");
            }
            catch (Exception ex) { MessageBox.Show("오류: " + ex.Message); }
            finally { btnSave.Enabled = true; }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}