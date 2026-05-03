using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class MainForm : Form
    {
        private DateTime _currentDate = DateTime.Today;
        private List<DayControl> _dayControls = new List<DayControl>();

        // 🌟 현재 뷰 상태를 저장하는 변수 (월간: false, 주간: true)
        private bool _isWeeklyView = false;

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            string userName = ApiService.CurrentUserName ?? "사용자";
            lblTitle.Text = $"{userName}님의 캘린더";

            // 콤보박스 이벤트 잠시 해제 (초기 세팅 중 중복 호출 방지)
            cmbViewType.SelectedIndexChanged -= cmbViewType_SelectedIndexChanged;
            cmbViewType.Items.Clear();
            cmbViewType.Items.Add("월간 뷰");
            cmbViewType.Items.Add("주간 뷰");
            cmbViewType.SelectedIndex = 0; // 기본값: 월간
            cmbViewType.SelectedIndexChanged += cmbViewType_SelectedIndexChanged;

            InitializeCalendarGrid();
            await LoadUserTeamsAsync();
            await RenderCalendarAsync();
        }

        private async Task LoadUserTeamsAsync()
        {
            try
            {
                string userId = ApiService.CurrentUserId;
                if (string.IsNullOrEmpty(userId)) return;

                var response = await ApiService.GetAsync<ApiResponse<List<TeamDto>>>($"/api/teams/{userId}");

                List<TeamDto> displayTeams = new List<TeamDto>();
                displayTeams.Add(new TeamDto { TeamId = "PERSONAL", TeamName = "개인 캘린더" });

                if (response != null && response.Success && response.Data != null)
                {
                    displayTeams.AddRange(response.Data);
                }

                cmbTeams.SelectedIndexChanged -= cmbTeams_SelectedIndexChanged;
                cmbTeams.DataSource = displayTeams;
                cmbTeams.DisplayMember = "TeamName";
                cmbTeams.ValueMember = "TeamId";
                cmbTeams.SelectedIndexChanged += cmbTeams_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"팀 목록을 불러오는 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 42개의 캘린더 칸을 미리 한 번만 만들어 둡니다.
        private void InitializeCalendarGrid()
        {
            for (int i = 0; i < 42; i++)
            {
                DayControl dc = new DayControl();
                _dayControls.Add(dc);
                pnlCalendar.Controls.Add(dc);
            }
        }

        // 🌟 월간/주간 모드에 따라 달력을 다시 그리는 핵심 메서드
        private async Task RenderCalendarAsync()
        {
            int totalDaysToRender = _isWeeklyView ? 7 : 42;
            DateTime startDate;

            if (_isWeeklyView)
            {
                // 주간 뷰: 현재 날짜가 속한 주의 일요일 찾기
                int currentDayOfWeek = (int)_currentDate.DayOfWeek;
                startDate = _currentDate.AddDays(-currentDayOfWeek);

                // 주간 뷰 상단 텍스트 (예: 2026년 05월 03일 ~ 09일)
                lblCurrentMonth.Text = $"{startDate:yyyy년 MM월} {startDate:dd}일 ~ {startDate.AddDays(6):dd}일";
            }
            else
            {
                // 월간 뷰: 현재 날짜가 속한 달의 1일이 속한 주의 일요일 찾기
                lblCurrentMonth.Text = _currentDate.ToString("yyyy년 MM월");
                DateTime firstDayOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
                int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                startDate = firstDayOfMonth.AddDays(-startDayOfWeek);
            }

            int colWidth = pnlCalendar.Width / 7;
            // 주간 뷰일 때는 1줄이 패널 전체 높이를 차지하도록 하고, 월간일 때는 6줄로 나눔
            int rowHeight = _isWeeklyView ? pnlCalendar.Height : (pnlCalendar.Height / 6);

            // UI 컨트롤 가시성 및 크기/위치 동적 조정
            for (int i = 0; i < 42; i++)
            {
                if (i < totalDaysToRender)
                {
                    _dayControls[i].Visible = true;
                    _dayControls[i].Size = new Size(colWidth, rowHeight);
                    _dayControls[i].Location = new Point((i % 7) * colWidth, (i / 7) * rowHeight);

                    DateTime cellDate = startDate.AddDays(i);
                    bool isCurrentMonth = _isWeeklyView || (cellDate.Month == _currentDate.Month);
                    _dayControls[i].SetDay(cellDate, isCurrentMonth);
                }
                else
                {
                    // 주간 뷰일 때 나머지 35개의 칸은 숨김
                    _dayControls[i].Visible = false;
                }
            }

            await FetchAndRenderSchedulesAsync(startDate, totalDaysToRender);
        }

        // 🌟 데이터를 가져올 때도 7일치만 가져올지 42일치만 가져올지 파라미터 적용
        private async Task FetchAndRenderSchedulesAsync(DateTime startDate, int totalDays)
        {
            try
            {
                string userId = ApiService.CurrentUserId;
                if (string.IsNullOrEmpty(userId)) return;

                string selectedTeamId = cmbTeams.SelectedValue?.ToString() ?? "PERSONAL";
                bool isTeamView = selectedTeamId != "PERSONAL";

                var fetchTasks = new List<Task>();

                for (int i = 0; i < totalDays; i++) // totalDays 만큼만 반복
                {
                    int index = i;
                    DateTime targetDate = startDate.AddDays(index);
                    string dateStr = targetDate.ToString("yyyy-MM-dd");

                    fetchTasks.Add(Task.Run(async () =>
                    {
                        ApiResponse<AvailabilityData> res;

                        if (isTeamView)
                            res = await ApiService.GetAsync<ApiResponse<AvailabilityData>>($"/api/availability/team/{selectedTeamId}/{dateStr}");
                        else
                            res = await ApiService.GetAsync<ApiResponse<AvailabilityData>>($"/api/availability/{userId}/{dateStr}");

                        if (res != null && res.Success && res.Data != null && res.Data.Slots != null)
                        {
                            this.Invoke(new Action(() =>
                            {
                                foreach (var slot in res.Data.Slots)
                                {
                                    Color bgColor = isTeamView ? Color.LightCoral : Color.CornflowerBlue;
                                    _dayControls[index].AddScheduleSlot($"{slot.Start} ~ {slot.End}", bgColor);
                                }
                            }));
                        }
                    }));
                }

                await Task.WhenAll(fetchTasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"일정을 불러오는 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- 컨트롤 이벤트들 ---

        // 🌟 뷰 타입(월간/주간) 변경 이벤트 추가
        private async void cmbViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbViewType.SelectedItem != null)
            {
                _isWeeklyView = cmbViewType.SelectedItem.ToString() == "주간 뷰";
                await RenderCalendarAsync(); // 모드 변경 후 달력 다시 그리기
            }
        }

        private async void cmbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTeamName = cmbTeams.Text;
            string userName = ApiService.CurrentUserName ?? "사용자";

            if (selectedTeamName == "개인 캘린더")
                lblTitle.Text = $"{userName}님의 개인 캘린더";
            else
                lblTitle.Text = $"{userName}님의 [{selectedTeamName}] 팀 캘린더";

            await RenderCalendarAsync();
        }

        // 🌟 이전/다음 버튼 누를 때 월간이면 1달, 주간이면 7일 이동
        private async void btnPrev_Click(object sender, EventArgs e)
        {
            _currentDate = _isWeeklyView ? _currentDate.AddDays(-7) : _currentDate.AddMonths(-1);
            await RenderCalendarAsync();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            _currentDate = _isWeeklyView ? _currentDate.AddDays(7) : _currentDate.AddMonths(1);
            await RenderCalendarAsync();
        }

        private async void btnToday_Click(object sender, EventArgs e)
        {
            _currentDate = DateTime.Today;
            await RenderCalendarAsync();
        }

        private async void btnAddSchedule_Click(object sender, EventArgs e)
        {
            // AddScheduleForm은 아직 구현 전이라면 주석 처리하거나 빈 폼을 띄우세요
            // using (AddScheduleForm addForm = new AddScheduleForm())
            // {
            //     if (addForm.ShowDialog() == DialogResult.OK)
            //         await RenderCalendarAsync();
            // }
            MessageBox.Show("일정 추가 폼 준비 중!", "알림");
        }
    }
}