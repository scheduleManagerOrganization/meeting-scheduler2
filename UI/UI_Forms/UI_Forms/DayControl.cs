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
            flpSchedules.HorizontalScroll.Enabled = false;
            flpSchedules.HorizontalScroll.Visible = false;
        }

        // API에서 받아온 일정 슬롯을 화면에 추가
        // DayControl.cs 내부의 메서드 수정
        public void AddScheduleSlot(string title, Color color, bool isFullBox)
        {
            Label lblSlot = new Label
            {
                Text = $"  {title}",
                Font = new Font("맑은 고딕", 8F, FontStyle.Bold),
                Width = flpSchedules.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 6,
                Height = 22,
                Margin = new Padding(2, 1, 2, 1),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            if (isFullBox)
            {
                // 박스 형태 (연일 일정 등)
                lblSlot.BackColor = color;
                lblSlot.ForeColor = Color.White;
            }
            else
            {
                // 선 형태 (당일 일정)
                lblSlot.BackColor = Color.White;
                lblSlot.ForeColor = Color.Black;

                // 커스텀 페인팅: 왼쪽에 두꺼운 색상 선 그리기
                lblSlot.Paint += (s, e) => {
                    Control paintLabel = (Control)s;
                    using (Pen p = new Pen(color, 6)) // 3px 두께의 선
                    {
                        e.Graphics.DrawLine(p, 2, 2, 2, paintLabel.Height - 4);
                    }
                };
            }

            flpSchedules.Controls.Add(lblSlot);
        }
    }
}
