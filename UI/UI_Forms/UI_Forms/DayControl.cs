using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI_Forms
{
    public partial class DayControl : UserControl
    {
        public DateTime Date { get; private set; }

        public DayControl()
        {
            InitializeComponent();
        }

        // 날짜 세팅 및 초기화
        public void SetDay(DateTime date, bool isCurrentMonth)
        {
            this.Date = date;
            lblDay.Text = date.Day.ToString();

            // 오늘 날짜 하이라이트
            if (date.Date == DateTime.Today)
            {
                lblDay.BackColor = Color.CornflowerBlue;
                lblDay.ForeColor = Color.White;
            }
            else
            {
                lblDay.BackColor = Color.Transparent;
                lblDay.ForeColor = isCurrentMonth ? Color.Black : Color.LightGray;
            }

            // 기존 일정 초기화
            flpSchedules.Controls.Clear();
        }

        // API에서 받아온 일정 슬롯을 화면에 추가
        public void AddScheduleSlot(string timeRange, Color bgColor)
        {
            Label lblSlot = new Label
            {
                Text = timeRange,
                BackColor = bgColor,
                ForeColor = Color.White,
                Font = new Font("맑은 고딕", 8F, FontStyle.Bold),
                Width = flpSchedules.Width - 6,
                Height = 22,
                Margin = new Padding(1, 1, 1, 1),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };

            flpSchedules.Controls.Add(lblSlot);
        }
    }
}