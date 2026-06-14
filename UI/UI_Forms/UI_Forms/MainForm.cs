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

        // 🌟 향후 미팅 데이터 렌더링 시 사용할 접두사 포맷터
        private string FormatMeetingTitle(string title, string responseStatus)
        {
            string prefix = "!"; // 기본 미응답
            if (responseStatus?.ToLower() == "yes") prefix = "✓";
            else if (responseStatus?.ToLower() == "no") prefix = "X";

            string formattedTitle = FormatTitle(title);
            return $"[{prefix}] {formattedTitle}";
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
                Console.WriteLine($"FetchAndRenderPersonalSchedulesRangeAsync 오류: {ex.Message}");
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

                // TODO: 향후 GET /api/meetings/team/{teamId} 를 호출하여 
                // Color.Crimson 색상과 FormatMeetingTitle()을 사용하여 미팅 일정을 _dayControls에 추가하는 로직 구현
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FetchAndRenderTeamSchedulesRangeAsync 오류: {ex.Message}");
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

        private async void btnCreateTeam_Click(object sender, EventArgs e)
        {
            using (var createTeamForm = new CreateTeamForm())
            {
                if (createTeamForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadUserTeamsAsync();
                    UpdateCalendarModeUi();
                    await RenderCalendarAsync();
                }
            }
        }

        private async void btnJoinTeam_Click(object sender, EventArgs e)
        {
            using (var form = new JoinTeamForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await LoadUserTeamsAsync();
                    UpdateCalendarModeUi();
                    await RenderCalendarAsync();
                }
            }
        }

        // 🌟 팀 미팅 추가 버튼 클릭 이벤트
        private async void btnAddMeeting_Click(object sender, EventArgs e)
        {
            // 1. 현재 콤보박스에서 선택된 팀 ID 가져오기
            string currentTeamId = cmbTeams.SelectedValue?.ToString();

            if (string.IsNullOrEmpty(currentTeamId))
            {
                MessageBox.Show("미팅을 추가할 팀을 먼저 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. AddMeetingForm을 모달로 띄우기 (teamId 전달)
            using (var addMeetingForm = new AddMeetingForm(currentTeamId))
            {
                var result = addMeetingForm.ShowDialog();

                // 3. 폼에서 '저장'이 성공하여 DialogResult.OK가 반환되었다면 캘린더 새로고침
                if (result == DialogResult.OK)
                {
                    await RenderCalendarAsync();
                }
            }
        }

        private void UpdateCalendarModeUi()
        {
            string userName = ApiService.CurrentUserName ?? "사용자";

            cmbTeams.Visible = _isTeamCalendar;
            tlpTeamLegend.Visible = _isTeamCalendar;
            btnCreateTeam.Visible = _isTeamCalendar;
            btnJoinTeam.Visible = _isTeamCalendar;

            // 미팅 관련 컨트롤 가시성
            btnAddMeeting.Visible = _isTeamCalendar;
            pnlMeetingLegend.Visible = _isTeamCalendar;

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
            // 🌟 [추가된 부분] 팀 캘린더일 때 우측으로 폼 확장
            if (_isTeamCalendar)
            {
                this.Width = 1290; // 원래 Width(약 986)에서 우측 사이드바(280)만큼 확장
                flpMeetingSidebar.Visible = true;
                _ = LoadAndRenderTeamMeetingsAsync(); // 미팅 목록 불러오기
            }
            else
            {
                this.Width = 986; // 개인 캘린더일 때 축소
                flpMeetingSidebar.Visible = false;
            }
        }

        private async Task LoadAndRenderTeamMeetingsAsync()
        {
            flpMeetingSidebar.Controls.Clear();
            string currentTeamId = cmbTeams.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(currentTeamId)) return;

            // 미팅 목록 가져오기
            var meetingsResponse = await ApiService.GetAsync<ApiResponse<List<MeetingDto>>>($"/api/meetings/team/{currentTeamId}");
            if (meetingsResponse?.Success != true || meetingsResponse.Data == null) return;

            foreach (var meeting in meetingsResponse.Data)
            {
                // 미팅별 카드(패널) 생성
                Panel pnlCard = new Panel
                {
                    Width = 250,
                    AutoSize = true,
                    BackColor = Color.White,
                    Margin = new Padding(10),
                    Padding = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // 제목 및 소요 시간
                Label lblTitle = new Label
                {
                    Text = $"{meeting.Title} ({meeting.DurationMinutes}분)",
                    Font = new Font("맑은 고딕", 11F, FontStyle.Bold),
                    ForeColor = Color.Crimson,
                    AutoSize = true,
                    Location = new Point(10, 10)
                };
                pnlCard.Controls.Add(lblTitle);

                // 추천 슬롯 데이터 가져오기
                var slotsResponse = await ApiService.GetAsync<ApiResponse<List<MeetingSlotDto>>>($"/api/slots/{meeting.Id}");
                int yOffset = 40;

                if (slotsResponse?.Success == true && slotsResponse.Data != null && slotsResponse.Data.Count > 0)
                {
                    Label lblSub = new Label { Text = "추천 시간대:", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(10, yOffset), AutoSize = true };
                    pnlCard.Controls.Add(lblSub);
                    yOffset += 20;

                    foreach (var slot in slotsResponse.Data)
                    {
                        Label lblSlot = new Label
                        {
                            Text = $"- {slot.StartTime:MM/dd HH:mm} ~ {slot.EndTime:HH:mm}",
                            Location = new Point(15, yOffset),
                            AutoSize = true,
                            Font = new Font("맑은 고딕", 9F)
                        };
                        pnlCard.Controls.Add(lblSlot);
                        yOffset += 20;
                    }
                }
                else
                {
                    Label lblNoSlot = new Label { Text = "조율 중이거나 슬롯이 없습니다.", Location = new Point(10, yOffset), AutoSize = true };
                    pnlCard.Controls.Add(lblNoSlot);
                    yOffset += 25;
                }

                // 응답하기 버튼 생성
                Button btnRespond = new Button
                {
                    Text = "시간대 응답하기",
                    BackColor = Color.CornflowerBlue,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(10, yOffset + 10),
                    Size = new Size(225, 30)
                };
                btnRespond.Click += (s, e) =>
                {
                    using (var respondForm = new RespondSlotForm(meeting.Id, meeting.Title))
                    {
                        if (respondForm.ShowDialog() == DialogResult.OK)
                        {
                            _ = LoadAndRenderTeamMeetingsAsync(); // 응답 후 패널 새로고침
                        }
                    }
                };
                pnlCard.Controls.Add(btnRespond);
                pnlCard.Height = btnRespond.Bottom + 15;

                flpMeetingSidebar.Controls.Add(pnlCard);
            }
        }

        private void UpdateCalendarAreaSize()
        {
            int bottomPadding = 20;
            int legendGap = 8;
            int legendHeight = 82;

            pnlCalendar.Width = this.ClientSize.Width - (pnlCalendar.Left * 2);

            if (_isTeamCalendar)
            {
                int calendarHeight = this.ClientSize.Height - pnlCalendar.Top - legendGap - legendHeight - bottomPadding;
                pnlCalendar.Height = Math.Max(420, calendarHeight);

                // 범례 너비 분할 (팀 범례 75%, 미팅 범례 25%)
                int totalLegendWidth = pnlCalendar.Width;
                int meetingLegendWidth = 180;

                tlpTeamLegend.Location = new Point(pnlCalendar.Left, pnlCalendar.Bottom + legendGap);
                tlpTeamLegend.Size = new Size(totalLegendWidth - meetingLegendWidth - 10, legendHeight);

                pnlMeetingLegend.Location = new Point(tlpTeamLegend.Right + 10, pnlCalendar.Bottom + legendGap);
                pnlMeetingLegend.Size = new Size(meetingLegendWidth, legendHeight);
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
                Size = new Size(200, 20),
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
            if (TimeSpan.TryParse(time, out TimeSpan parsed))
                return (int)parsed.TotalMinutes;
            return 0;
        }
    }
}