using System;
using System.Drawing;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI_Forms
{
    public partial class MainForm : Form
    {
        private string currentUserName;
        private DateTime displayedDate; // 현재 캘린더가 가리키는 기준 날짜 (그 달의 1일 등)
        private bool isMonthlyView = true; // 현재 월간 뷰인지 주간 뷰인지 상태
        // DB 연결 전까지 사용할 전역 일정 리스트
        private List<Schedule> globalSchedules = new List<Schedule>();

        public MainForm()
        {
            InitializeComponent();
            lblWelcome.Text = $"님의 팀 캘린더";
            displayedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // 이번 달 1일로 시작
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GenerateCalendarGrid(); // 첫 렌더링
        }

        // 핵심: 현재 날짜(displayedDate)와 상태(isMonthlyView)에 따라 캘린더 재생성
        private void GenerateCalendarGrid()
        {
            calendarPanel.Controls.Clear(); // 기존 DayControl들 싹 지우기
            calendarPanel.SuspendLayout(); // 렌더링 성능 향상

            lblCurrentMonth.Text = displayedDate.ToString("yyyy년 MM월"); // 상단 제목 업데이트

            if (isMonthlyView)
                GenerateMonthlyView();
            else
                GenerateWeeklyView();

            calendarPanel.ResumeLayout();
        }

        // [월간 뷰 전용] 7열 6행 뼈대에 DayControl 배치
        private void GenerateMonthlyView()
        {
            calendarPanel.ColumnCount = 7;
            calendarPanel.RowCount = 6;
            ResetTableStyles(7, 6);

            // 1일의 요일 (일요일:0, 토요일:6)
            int startDayOfWeek = (int)displayedDate.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(displayedDate.Year, displayedDate.Month);

            // 달력 격자에 채워넣을 첫 날짜 계산 (이전 달 날짜 포함)
            DateTime gridStartDate = displayedDate.AddDays(-startDayOfWeek);

            // 7*6 = 42개 칸 생성
            for (int i = 0; i < 42; i++)
            {
                DateTime currentDay = gridStartDate.AddDays(i);
                DayControl dayCtrl = new DayControl();

                // 이번 달에 속하는 날짜인지 체크하여 전달
                bool isCurrentMonth = (currentDay.Month == displayedDate.Month && currentDay.Year == displayedDate.Year);
                dayCtrl.SetDate(currentDay, isCurrentMonth);
                dayCtrl.Dock = DockStyle.Fill;

                // 이 날짜에 해당하는 일정을 찾아서 UI에 그려줌 (V1.2)
                foreach (var sched in globalSchedules)
                {
                    // 현재 그리는 칸의 날짜가 일정의 시작일과 종료일 사이에 있다면 표시
                    if (currentDay.Date >= sched.StartDate.Date && currentDay.Date <= sched.EndDate.Date)
                    {
                        dayCtrl.AddScheduleUI(sched);
                    }
                }

                // TableLayoutPanel에 좌표 지정하여 추가 (col, row)
                calendarPanel.Controls.Add(dayCtrl, i % 7, i / 7);
            }
        }

        // [주간 뷰 전용] 7열 1행 (또는 시간단위로 쪼개도 됨, 여기서는 하루단위 7칸)
        private void GenerateWeeklyView()
        {
            calendarPanel.ColumnCount = 7;
            calendarPanel.RowCount = 1;
            ResetTableStyles(7, 1);

            // displayedDate가 포함된 주의 일요일 구하기
            // (참고: weekly view에서는 displayedDate를 자유롭게 잡아도 되지만, 여기서는 기준날짜로 잡음)
            int diff = (int)displayedDate.DayOfWeek;
            DateTime startOfWeek = displayedDate.AddDays(-diff);

            lblCurrentMonth.Text = $"{displayedDate.ToString("yyyy년 MM월")} {GetWeekOfMonth(displayedDate)}주차";

            for (int i = 0; i < 7; i++)
            {
                DateTime currentDay = startOfWeek.AddDays(i);
                DayControl dayCtrl = new DayControl();
                dayCtrl.SetDate(currentDay, true); // 주간 뷰는 연하게 표시 안함
                dayCtrl.Dock = DockStyle.Fill;

                // 일정을 찾아 UI에 그려주는 로직 (V1.2)
                foreach (var sched in globalSchedules)
                {
                    if (currentDay.Date >= sched.StartDate.Date && currentDay.Date <= sched.EndDate.Date)
                    {
                        dayCtrl.AddScheduleUI(sched);
                    }
                }

                calendarPanel.Controls.Add(dayCtrl, i, 0);
            }
        }

        // TableLayoutPanel의 격자 비율 다시 조정 (필수!)
        private void ResetTableStyles(int colCount, int rowCount)
        {
            calendarPanel.ColumnStyles.Clear();
            calendarPanel.RowStyles.Clear();

            for (int i = 0; i < colCount; i++)
                calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / colCount));
            for (int i = 0; i < rowCount; i++)
                calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rowCount));
        }

        // 몇 주차인지 계산하는 유틸
        private int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) date = date.AddDays(1);
            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        // --- 상단 버튼 이벤트 처리 ---

        private void btnPrev_Click(object sender, EventArgs e)
        {
            // 달 단위 또는 주 단위 이동
            displayedDate = isMonthlyView ? displayedDate.AddMonths(-1) : displayedDate.AddDays(-7);
            GenerateCalendarGrid();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            displayedDate = isMonthlyView ? displayedDate.AddMonths(1) : displayedDate.AddDays(7);
            GenerateCalendarGrid();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            displayedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            GenerateCalendarGrid();
        }

        private void btnMonthView_Click(object sender, EventArgs e)
        {
            isMonthlyView = true;
            GenerateCalendarGrid();
        }

        private void btnWeekView_Click(object sender, EventArgs e)
        {
            // 주간 뷰로 전환 시 기준일 조정 (선택사항, 1일 -> 오늘날짜 등으로)
            displayedDate = DateTime.Today;
            isMonthlyView = false;
            GenerateCalendarGrid();
        }

        // 일정 추가 폼 열기 (이전과 동일)
        private void btnAddSchedule_Click(object sender, EventArgs e)
        {
            AddScheduleForm addForm = new AddScheduleForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                // 입력받은 새 일정을 리스트에 추가 (V1.2)
                globalSchedules.Add(addForm.NewSchedule);

                // 나중에는 여기서 MongoDB Insert 메서드를 호출해야 합니다.
                // dbHelper.InsertSchedule(addForm.NewSchedule);

                // 캘린더 다시 그리기 (추가된 일정 반영)
                GenerateCalendarGrid();
            }
        }
    }
}