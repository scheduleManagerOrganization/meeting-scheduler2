using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class MainForm : Form
    {
        private DateTime _currentDate = DateTime.Today;
        private List<DayControl> _dayControls = new List<DayControl>();
        private bool _isWeeklyView = false;
        private bool _isTeamCalendar = false;
        private int _renderVersion = 0;
        private readonly Dictionary<string, Color> _teamUserColors = new Dictionary<string, Color>();
        private readonly Dictionary<string, string> _teamUserNames = new Dictionary<string, string>();
        private readonly Color[] _teamColorPalette = new[]
        {
            Color.CornflowerBlue, Color.MediumSeaGreen, Color.Orange,
            Color.MediumPurple, Color.Teal, Color.Goldenrod, Color.DeepSkyBlue
        };

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            string userName = ApiService.CurrentUserName ?? "사용자";
            lblTitle.Text = $"{userName}님의 개인 캘린더";
            ApplyResponsiveFormSize();

            // 뷰 타입 초기화
            cmbViewType.SelectedIndexChanged -= cmbViewType_SelectedIndexChanged;
            cmbViewType.Items.Clear();
            cmbViewType.Items.Add("월간 뷰");
            cmbViewType.Items.Add("주간 뷰");
            cmbViewType.SelectedIndex = 0;
            cmbViewType.SelectedIndexChanged += cmbViewType_SelectedIndexChanged;

            InitializeCalendarGrid();
            await LoadUserTeamsAsync();
            UpdateCalendarModeUi();
            await RenderCalendarAsync();
        }

        private string FormatTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "일정";
            return title.Length > 5 ? title.Substring(0, 5) + "..." : title;
        }

        private void ApplyResponsiveFormSize()
        {
            Rectangle workArea = Screen.FromControl(this).WorkingArea;
            int targetWidth = Math.Min(1109, workArea.Width - 40);
            int targetHeight = Math.Min(800, workArea.Height - 60);

            this.ClientSize = new Size(Math.Max(960, targetWidth), Math.Max(640, targetHeight));
            this.CenterToScreen();
        }

        private async Task LoadUserTeamsAsync()
        {
            try
            {
                string userId = ApiService.CurrentUserId;
                var response = await ApiService.GetAsync<ApiResponse<List<TeamDto>>>($"/api/teams/{userId}");

                List<TeamDto> displayTeams = new List<TeamDto>();

                if (response?.Success == true && response.Data != null)
                {
                    var uniqueTeams = response.Data.GroupBy(t => t.TeamName).Select(g => g.First()).ToList();
                    displayTeams.AddRange(uniqueTeams);
                }

                cmbTeams.SelectedIndexChanged -= cmbTeams_SelectedIndexChanged;
                cmbTeams.DataSource = displayTeams;
                cmbTeams.DisplayMember = "TeamName";
                cmbTeams.ValueMember = "TeamId";
                cmbTeams.SelectedIndexChanged += cmbTeams_SelectedIndexChanged;
            }
            catch { /* 오류 처리 생략 */ }
        }

        private void InitializeCalendarGrid()
        {
            pnlCalendar.Controls.Clear();
            _dayControls.Clear();
            for (int i = 0; i < 42; i++)
            {
                DayControl dc = new DayControl();
                _dayControls.Add(dc);
                pnlCalendar.Controls.Add(dc);
            }
        }

        private async Task RenderCalendarAsync()
        {
            int renderVersion = ++_renderVersion;
            int totalDaysToRender = _isWeeklyView ? 7 : 42;
            DateTime startDate;

            if (_isWeeklyView)
            {
                int currentDayOfWeek = (int)_currentDate.DayOfWeek;
                startDate = _currentDate.AddDays(-currentDayOfWeek);
                lblCurrentMonth.Text = $"{startDate:yyyy년 MM월 dd일} ~ {startDate.AddDays(6):dd일}";
            }
            else
            {
                lblCurrentMonth.Text = _currentDate.ToString("yyyy년 MM월");
                DateTime firstDayOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
                int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                startDate = firstDayOfMonth.AddDays(-startDayOfWeek);
            }

            int colWidth = pnlCalendar.Width / 7;
            int rowHeight = _isWeeklyView ? pnlCalendar.Height : (pnlCalendar.Height / 6);

            for (int i = 0; i < 42; i++)
            {
                if (i < totalDaysToRender)
                {
                    _dayControls[i].Visible = true;
                    _dayControls[i].Size = new Size(colWidth, rowHeight);
                    _dayControls[i].Location = new Point((i % 7) * colWidth, (i / 7) * rowHeight);
                    DateTime cellDate = startDate.AddDays(i);
                    _dayControls[i].SetDay(cellDate, _isWeeklyView || (cellDate.Month == _currentDate.Month));
                }
                else _dayControls[i].Visible = false;
            }

            await FetchAndRenderSchedulesAsync(startDate, totalDaysToRender, renderVersion);
        }

        private async Task FetchAndRenderSchedulesAsync(DateTime startDate, int totalDays, int renderVersion)
        {
            try
            {
                string userId = ApiService.CurrentUserId;
                if (_isTeamCalendar) _teamUserNames.Clear();

                if (_isTeamCalendar)
                {
                    await FetchAndRenderTeamSchedulesRangeAsync(startDate, totalDays, renderVersion);
                }
                else
                {
                    await FetchAndRenderPersonalSchedulesRangeAsync(startDate, totalDays, renderVersion);
                }

                if (renderVersion == _renderVersion)
                {
                    UpdateTeamLegend();
                }
            }
            catch (Exception ex)
            {
                if (renderVersion == _renderVersion)
                {
                    MessageBox.Show($"캘린더 조회 중 오류가 발생했습니다.\n{ex.Message}", "조회 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task FetchAndRenderPersonalSchedulesRangeAsync(DateTime startDate, int totalDays, int renderVersion)
        {
            try
            {
                string userId = ApiService.CurrentUserId;
                string startDateStr = startDate.ToString("yyyy-MM-dd");

                var response = await ApiService.GetAsync<ApiResponse<RangeScheduleData>>(
                    $"/api/availability/{userId}/range?startDate={startDateStr}&days={totalDays}"
                );

                if (renderVersion != _renderVersion) return;
                if (response?.Success != true || response.Data?.Schedules == null) return;

                foreach (var schedule in response.Data.Schedules)
                {
                    if (!DateTime.TryParse(schedule.Date, out var scheduleDate)) continue;

                    int dayIndex = (int)(scheduleDate - startDate).TotalDays;
                    if (dayIndex < 0 || dayIndex >= totalDays) continue;

                    if (schedule.Slots != null)
                    {
                        foreach (var slot in schedule.Slots)
                        {
                            string displayTitle = $"{slot.Start} {FormatTitle(slot.Title)}";
                            bool isFullBox = slot.Start == "00:00" && slot.End == "23:59";

                            _dayControls[dayIndex].AddScheduleSlot(displayTitle, Color.CornflowerBlue, isFullBox, () =>
                            {
                                using (var detailForm = new ScheduleDetailForm(scheduleDate, slot, null))
                                {
                                    if (detailForm.ShowDialog() == DialogResult.OK)
                                    {
                                        _ = RenderCalendarAsync();
                                    }
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ FetchAndRenderPersonalSchedulesRangeAsync 오류: {ex.Message}");
            }
        }

        private async Task FetchAndRenderTeamSchedulesRangeAsync(DateTime startDate, int totalDays, int renderVersion)
        {
            try
            {
                string selectedTeamId = cmbTeams.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(selectedTeamId)) return;

                string startDateStr = startDate.ToString("yyyy-MM-dd");

                var response = await ApiService.GetAsync<ApiResponse<TeamRangeScheduleData>>(
                    $"/api/availability/team/{selectedTeamId}/range?startDate={startDateStr}&days={totalDays}"
                );

                if (renderVersion != _renderVersion) return;
                if (response?.Success != true || response.Data?.Schedules == null) return;

                var groupedByDate = response.Data.Schedules.GroupBy(s => s.Date);

                foreach (var group in groupedByDate)
                {
                    if (!DateTime.TryParse(group.Key, out var scheduleDate)) continue;

                    int dayIndex = (int)(scheduleDate - startDate).TotalDays;
                    if (dayIndex < 0 || dayIndex >= totalDays) continue;

                    var teamSlots = group
                        .Where(user => user.Slots != null)
                        .SelectMany(user => user.Slots.Select(slot => new
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            Slot = slot
                        }))
                        .OrderBy(item => ToMinutes(item.Slot.Start))
                        .ThenByDescending(item => ToMinutes(item.Slot.End))
                        .ToList();

                    foreach (var item in teamSlots)
                    {
                        Color userColor = GetTeamUserColor(item.UserId);
                        RememberTeamLegendUser(item.UserId, item.UserName);

                        // 🌟 수정된 부분: 이름은 빼고 시작시간 - 종료시간만 깔끔하게 표시
                        string displayTitle = $"{item.Slot.Start} - {item.Slot.End}";

                        _dayControls[dayIndex].AddScheduleSlot(displayTitle, userColor, false, () =>
                        {
                            using (var detailForm = new ScheduleDetailForm(scheduleDate, item.Slot, selectedTeamId))
                            {
                                if (detailForm.ShowDialog() == DialogResult.OK)
                                {
                                    _ = RenderCalendarAsync();
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ FetchAndRenderTeamSchedulesRangeAsync 오류: {ex.Message}");
            }
        }

        private async void cmbViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _isWeeklyView = cmbViewType.Text == "주간 뷰";
            await RenderCalendarAsync();
        }

        private async void cmbTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCalendarModeUi();
            await RenderCalendarAsync();
        }

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

        private async void btnCalendarMode_Click(object sender, EventArgs e)
        {
            _isTeamCalendar = !_isTeamCalendar;
            UpdateCalendarModeUi();
            await RenderCalendarAsync();
        }

        private async void btnAddAvailability_Click(object sender, EventArgs e)
        {
            using (AddScheduleForm addForm = new AddScheduleForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    await RenderCalendarAsync();
                }
            }
        }

        // 🌟 팀 생성 폼 호출 (모달)
        private async void btnCreateTeam_Click(object sender, EventArgs e)
        {
            using (var createTeamForm = new CreateTeamForm())
            {
                if (createTeamForm.ShowDialog() == DialogResult.OK)
                {
                    // 팀 생성 후 내 팀 목록 새로고침
                    await LoadUserTeamsAsync();
                    UpdateCalendarModeUi();
                    await RenderCalendarAsync();
                }
            }
        }

        // 🌟 팀 참가 버튼 클릭 (다음 단계 준비용)
        private void btnJoinTeam_Click(object sender, EventArgs e)
        {
            MessageBox.Show("팀 참가 기능 폼을 곧 이어서 만들 예정입니다!", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateCalendarModeUi()
        {
            string userName = ApiService.CurrentUserName ?? "사용자";

            // 팀 선택 콤보박스와 팀 생성/참가 버튼을 팀 캘린더 모드에서만 보이게 처리
            cmbTeams.Visible = _isTeamCalendar;
            tlpTeamLegend.Visible = _isTeamCalendar;
            btnCreateTeam.Visible = _isTeamCalendar; // 팀 생성 버튼 가시성 제어
            btnJoinTeam.Visible = _isTeamCalendar;   // 팀 참가 버튼 가시성 제어

            UpdateCalendarAreaSize();
            btnCalendarMode.Text = _isTeamCalendar ? "개인 캘린더" : "팀 캘린더";
            btnCalendarMode.ForeColor = _isTeamCalendar ? Color.CornflowerBlue : Color.MediumSeaGreen;
            btnCalendarMode.FlatAppearance.BorderColor = btnCalendarMode.ForeColor;

            if (_isTeamCalendar)
            {
                string teamName = cmbTeams.Items.Count == 0 ? "팀 없음" : cmbTeams.Text;
                lblTitle.Text = $"{userName}님의 [{teamName}] 팀 캘린더";
            }
            else
            {
                lblTitle.Text = $"{userName}님의 개인 캘린더";
                tlpTeamLegend.Controls.Clear();
            }
        }

        private void UpdateCalendarAreaSize()
        {
            int bottomPadding = 20;
            int legendGap = 8;
            int legendHeight = 82;

            pnlCalendar.Width = this.ClientSize.Width - (pnlCalendar.Left * 2);
            tlpTeamLegend.Width = pnlCalendar.Width;

            if (_isTeamCalendar)
            {
                int calendarHeight = this.ClientSize.Height - pnlCalendar.Top - legendGap - legendHeight - bottomPadding;
                pnlCalendar.Height = Math.Max(420, calendarHeight);
                tlpTeamLegend.Location = new Point(pnlCalendar.Left, pnlCalendar.Bottom + legendGap);
                tlpTeamLegend.Size = new Size(pnlCalendar.Width, legendHeight);
            }
            else
            {
                int calendarHeight = this.ClientSize.Height - pnlCalendar.Top - bottomPadding;
                pnlCalendar.Height = Math.Max(500, calendarHeight);
                tlpTeamLegend.Location = new Point(pnlCalendar.Left, pnlCalendar.Bottom + legendGap);
                tlpTeamLegend.Size = new Size(pnlCalendar.Width, legendHeight);
            }
        }

        private Color GetTeamUserColor(string userId)
        {
            string key = string.IsNullOrEmpty(userId) ? "UNKNOWN" : userId;
            if (!_teamUserColors.ContainsKey(key))
            {
                _teamUserColors[key] = _teamColorPalette[_teamUserColors.Count % _teamColorPalette.Length];
            }

            return _teamUserColors[key];
        }

        private void RememberTeamLegendUser(string userId, string userName)
        {
            string key = string.IsNullOrEmpty(userId) ? "UNKNOWN" : userId;
            string displayName = string.IsNullOrWhiteSpace(userName) ? "Unknown" : userName;

            if (!_teamUserNames.ContainsKey(key))
            {
                _teamUserNames[key] = displayName;
            }
        }

        private void UpdateTeamLegend()
        {
            tlpTeamLegend.Controls.Clear();
            tlpTeamLegend.RowStyles.Clear();
            tlpTeamLegend.RowCount = 0;

            if (!_isTeamCalendar || _teamUserNames.Count == 0) return;

            int rowCount = (int)Math.Ceiling(_teamUserNames.Count / 3.0);
            tlpTeamLegend.RowCount = rowCount;
            for (int i = 0; i < rowCount; i++)
            {
                tlpTeamLegend.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            }

            int index = 0;
            foreach (var user in _teamUserNames.OrderBy(x => x.Value))
            {
                Panel legendItem = CreateLegendItem(GetTeamUserColor(user.Key), user.Value);
                tlpTeamLegend.Controls.Add(legendItem, index % 3, index / 3);
                index++;
            }
        }

        private Panel CreateLegendItem(Color color, string userName)
        {
            Panel itemPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4, 2, 4, 2),
                BackColor = Color.White
            };

            Panel colorLine = new Panel
            {
                BackColor = color,
                Location = new Point(4, 11),
                Size = new Size(36, 4)
            };

            Label nameLabel = new Label
            {
                Text = userName,
                Location = new Point(48, 4),
                Size = new Size(280, 20),
                AutoEllipsis = true,
                Font = new Font("맑은 고딕", 9F, FontStyle.Regular),
                ForeColor = Color.DimGray
            };

            itemPanel.Controls.Add(colorLine);
            itemPanel.Controls.Add(nameLabel);
            return itemPanel;
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