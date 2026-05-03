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

        public void SetDate(DateTime date, bool isCurrentMonth)
        {
            this.Date = date;
            lblDay.Text = date.Day.ToString();

            // 이번 달 여부 및 오늘 날짜 스타일 (이전과 동일)
            if (!isCurrentMonth) lblDay.ForeColor = Color.LightGray;
            else if (date.Date == DateTime.Today)
            {
                this.BackColor = Color.LightCyan;
                lblDay.Font = new Font(lblDay.Font, FontStyle.Bold);
            }
            else
            {
                lblDay.ForeColor = Color.Black;
                this.BackColor = Color.White;
                lblDay.Font = new Font(lblDay.Font, FontStyle.Regular);
            }

            // 날짜 변경 시 이전 일정 UI 초기화
            flpSchedules.Controls.Clear();
        }

        // 일정을 UI에 그리는 메서드 (갤럭시 캘린더 스타일)
        public void AddScheduleUI(Schedule schedule)
        {
            Panel pnlItem = new Panel
            {
                Height = 20,
                Width = flpSchedules.Width - 6,
                Margin = new Padding(0, 1, 0, 1)
            };

            Label lblTitle = new Label
            {
                Text = schedule.Title,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("맑은 고딕", 8F),
                AutoEllipsis = true // 글씨가 길면 '...' 처리
            };

            if (schedule.IsSingleDay)
            {
                // [당일 일정] 왼쪽에 색깔 선 긋고 옆에 이름
                Panel pnlLeftLine = new Panel
                {
                    Width = 4,
                    Dock = DockStyle.Left,
                    BackColor = schedule.ScheduleColor
                };

                lblTitle.ForeColor = Color.Black; // 글자는 검은색

                pnlItem.Controls.Add(lblTitle);     // 남은 공간 채우기
                pnlItem.Controls.Add(pnlLeftLine);  // 왼쪽에 먼저 붙기 (Docking 순서 중요)
                lblTitle.BringToFront();            // 텍스트 라벨을 앞으로 당겨서 위치 맞춤
            }
            else
            {
                // [여러 날 일정] 색상 칸을 만들고 그 안에 이름
                pnlItem.BackColor = schedule.ScheduleColor;
                lblTitle.ForeColor = Color.White; // 색상 배경 위이므로 흰색 텍스트

                pnlItem.Controls.Add(lblTitle);
            }

            flpSchedules.Controls.Add(pnlItem);
        }
    }
}