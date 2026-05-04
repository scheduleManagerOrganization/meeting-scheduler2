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

        public void AddTeamAvailabilitySlot(string start, string end, Color color)
        {
            int height = GetTeamSlotHeight(start, end);
            int verticalScrollWidth = SystemInformation.VerticalScrollBarWidth;

            Panel slotPanel = new Panel
            {
                Width = flpSchedules.ClientSize.Width - verticalScrollWidth - 6,
                Height = height,
                Margin = new Padding(2, 1, 2, 3),
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };

            Label lblStart = new Label
            {
                Text = start,
                Location = new Point(10, 1),
                Size = new Size(45, 16),
                Font = new Font("맑은 고딕", 7F, FontStyle.Bold),
                ForeColor = Color.Black
            };

            Label lblEnd = new Label
            {
                Text = end,
                Location = new Point(10, height - 17),
                Size = new Size(45, 16),
                Font = new Font("맑은 고딕", 7F, FontStyle.Bold),
                ForeColor = Color.Black
            };

            // 팀원 이름은 하단 범례에서 보여주고, 셀 안에는 시간 흐름만 남겨 밀도를 낮춥니다.
            slotPanel.Paint += (s, e) =>
            {
                using (Pen p = new Pen(color, 5))
                {
                    e.Graphics.DrawLine(p, 4, 5, 4, slotPanel.Height - 5);
                }
            };

            slotPanel.Controls.Add(lblStart);
            slotPanel.Controls.Add(lblEnd);
            flpSchedules.Controls.Add(slotPanel);
        }

        private int GetTeamSlotHeight(string start, string end)
        {
            int duration = Math.Max(10, ToMinutes(end) - ToMinutes(start));

            // 월간 셀 안에서도 여러 사용자가 쌓일 수 있도록 높이를 제한하되, 긴 가용 시간은 더 길게 표시합니다.
            return Math.Max(38, Math.Min(72, 30 + duration / 30));
        }

        private int ToMinutes(string time)
        {
            TimeSpan parsed;
            if (TimeSpan.TryParse(time, out parsed))
            {
                return (int)parsed.TotalMinutes;
            }

            return 0;
        }
    }
}
