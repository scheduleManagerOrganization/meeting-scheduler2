using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_Forms
{
    public partial class AddScheduleForm : Form
    {
        public Schedule NewSchedule { get; private set; }

        public AddScheduleForm()
        {
            InitializeComponent();
            InitializeTimeComboBoxes(); // 시간/분 콤보박스 세팅
            btnColor.BackColor = Color.DodgerBlue;
        }

        private void InitializeTimeComboBoxes()
        {
            // 시간 (00 ~ 23) 세팅
            for (int i = 0; i < 24; i++)
            {
                cmbStartHour.Items.Add(i.ToString("D2"));
                cmbEndHour.Items.Add(i.ToString("D2"));
            }

            // 분 (00 ~ 55, 5분 단위) 세팅
            for (int i = 0; i < 60; i += 5)
            {
                cmbStartMinute.Items.Add(i.ToString("D2"));
                cmbEndMinute.Items.Add(i.ToString("D2"));
            }

            // 기본값 선택 (시작: 현재 시간 다음 정각, 종료: 그 1시간 뒤)
            int currentHour = DateTime.Now.Hour;

            // 콤보박스에 아무것도 선택안되는 오류 방지
            cmbStartHour.SelectedIndex = currentHour;
            cmbStartMinute.SelectedIndex = 0; // 00분

            cmbEndHour.SelectedIndex = (currentHour + 1) % 24;
            cmbEndMinute.SelectedIndex = 0; // 00분
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                btnColor.BackColor = colorDialog.Color;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("일정 제목을 입력하세요.", "알림");
                return;
            }

            // [핵심] 달력의 날짜(Date)와 콤보박스의 시간(Hour, Minute)을 병합
            DateTime startDate = dtpStart.Value.Date;
            int startHour = int.Parse(cmbStartHour.SelectedItem.ToString());
            int startMinute = int.Parse(cmbStartMinute.SelectedItem.ToString());
            DateTime finalStartTime = startDate.AddHours(startHour).AddMinutes(startMinute);

            DateTime endDate = dtpEnd.Value.Date;
            int endHour = int.Parse(cmbEndHour.SelectedItem.ToString());
            int endMinute = int.Parse(cmbEndMinute.SelectedItem.ToString());
            DateTime finalEndTime = endDate.AddHours(endHour).AddMinutes(endMinute);

            // 시간 유효성 검사
            if (finalEndTime < finalStartTime)
            {
                MessageBox.Show("종료 시간이 시작 시간보다 빠를 수 없습니다.", "오류");
                return;
            }

            NewSchedule = new Schedule
            {
                Title = txtTitle.Text,
                StartDate = finalStartTime,
                EndDate = finalEndTime,
                ScheduleColor = btnColor.BackColor
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}