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
        private const int PersonalClientWidth = 970;
        private const int TeamClientWidth = 1270;
        private const int FormPadding = 20;
        private const int SidebarGap = 20;
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
            string prefix = "!"; // 기본 미정
            if (responseStatus?.ToLower() == "yes") prefix = "✓";
            else if (responseStatus?.ToLower() == "no") prefix = "X";

            string formattedTitle = FormatTitle(title);
            return $"[{prefix}] {formattedTitle}";
        }

        private string GetCurrentUserSlotResponse(MeetingSlotDto slot)
        {
            string cachedResponse = SlotResponseCache.GetResponse(ApiService.CurrentUserId, slot?.Id);
            if (!string.IsNullOrWhiteSpace(cachedResponse)) return cachedResponse.ToLower();

            if (slot?.Responses == null) return "maybe";

            foreach (var response in slot.Responses)
            {
                if (IsCurrentUserResponse(response))
                {
                    return string.IsNullOrWhiteSpace(response.Response) ? "maybe" : response.Response.ToLower();
                }
            }

            return "maybe";
        }

        private bool IsCurrentUserResponse(SlotResponseDto response)
        {
            return string.Equals(
                response?.UserId?.Trim(),
                ApiService.CurrentUserId?.Trim(),
                StringComparison.OrdinalIgnoreCase
            );
        }

        private string FormatSlotResponseLabel(string responseStatus)
        {
            switch (responseStatus)
            {
                case "yes":
                    return "✓ 참석";
                case "no":
                    return "X 불참";
                default:
                    return "! 미정";
            }
        }

        private Color GetSlotResponseColor(string responseStatus)
        {
            switch (responseStatus)
            {
                case "yes":
                    return Color.MediumSeaGreen;
                case "no":
                    return Color.Crimson;
                default:
                    return Color.DimGray;
            }
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

                // -------------------------------------------------------------
                // 🌟 확정된 미팅 일정 캘린더에 추가 6.15
                // -------------------------------------------------------------
                var meetingsResponse = await ApiService.GetAsync<ApiResponse<List<MeetingDto>>>($"/api/meetings/team/{selectedTeamId}");

                if (meetingsResponse?.Success == true && meetingsResponse.Data != null)
                {
                    // 상태가 "finalized"이고 확정된 슬롯 ID가 있는 미팅만 필터링
                    var finalizedMeetings = meetingsResponse.Data
                        .Where(m => m.Status == "finalized" && !string.IsNullOrEmpty(m.FinalizedSlotId))
                        .ToList();

                    foreach (var meeting in finalizedMeetings)
                    {
                        // 확정된 미팅의 상세 슬롯 정보 가져오기
                        var slotsResponse = await ApiService.GetAsync<ApiResponse<List<MeetingSlotDto>>>($"/api/slots/{meeting.Id}");

                        if (slotsResponse?.Success == true && slotsResponse.Data != null)
                        {
                            // 확정된 슬롯 찾기
                            var finalSlot = slotsResponse.Data.FirstOrDefault(s => s.Id == meeting.FinalizedSlotId);
                            if (finalSlot != null)
                            {
                                // 달력의 몇 번째 칸(Day)에 그려야 하는지 계산
                                DateTime scheduleDate = finalSlot.StartTime.Date;
                                int dayIndex = (int)(scheduleDate - startDate).TotalDays;

                                // 현재 화면에 보이는 날짜 범위 안인지 확인
                                if (dayIndex >= 0 && dayIndex < totalDays)
                                {
                                    // 기존에 만들어두셨던 FormatMeetingTitle 활용 (확정되었으니 무조건 "yes" 아이콘 사용)
                                    string formattedTitle = FormatMeetingTitle(meeting.Title, "yes");
                                    string displayTitle = $"{finalSlot.StartTime:HH:mm} {formattedTitle}";

                                    // 달력에 빨간색(Crimson)으로 슬롯 추가
                                    _dayControls[dayIndex].AddScheduleSlot(displayTitle, Color.Crimson, false, () =>
                                    {
                                        // 달력에 뜬 미팅을 클릭했을 때 보여줄 정보 창
                                        MessageBox.Show(
                                            $"[확정된 미팅]\n\n제목: {meeting.Title}\n내용: {meeting.Description}\n시간: {finalSlot.StartTime:MM/dd HH:mm} ~ {finalSlot.EndTime:HH:mm}",
                                            "팀 미팅 정보",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information
                                        );
                                    });
                                }
                            }
                        }
                    }
                }
                // -------------------------------------------------------------
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

            ApplyCalendarModeSize();

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

            if (_isTeamCalendar)
            {
                flpMeetingSidebar.Visible = true;
                _ = LoadAndRenderTeamMeetingsAsync(); // 미팅 목록 불러오기
            }
            else
            {
                flpMeetingSidebar.Visible = false;
            }
        }

        //private async Task LoadAndRenderTeamMeetingsAsync()
        //{
        //    flpMeetingSidebar.Controls.Clear();
        //    string currentTeamId = cmbTeams.SelectedValue?.ToString();
        //    if (string.IsNullOrEmpty(currentTeamId)) return;

        //    // 미팅 목록 가져오기
        //    var meetingsResponse = await ApiService.GetAsync<ApiResponse<List<MeetingDto>>>($"/api/meetings/team/{currentTeamId}");
        //    if (meetingsResponse?.Success != true || meetingsResponse.Data == null) return;

        //    foreach (var meeting in meetingsResponse.Data)
        //    {
        //        // 미팅별 카드(패널) 생성
        //        Panel pnlCard = new Panel
        //        {
        //            Width = 250,
        //            AutoSize = true,
        //            BackColor = Color.White,
        //            Margin = new Padding(10),
        //            Padding = new Padding(10),
        //            BorderStyle = BorderStyle.FixedSingle
        //        };

        //        // 제목 및 소요 시간
        //        Label lblTitle = new Label
        //        {
        //            Text = $"{meeting.Title} ({meeting.DurationMinutes}분)",
        //            Font = new Font("맑은 고딕", 11F, FontStyle.Bold),
        //            ForeColor = Color.Crimson,
        //            AutoSize = true,
        //            Location = new Point(10, 10)
        //        };
        //        pnlCard.Controls.Add(lblTitle);

        //        // 추천 슬롯 데이터 가져오기
        //        var slotsResponse = await ApiService.GetAsync<ApiResponse<List<MeetingSlotDto>>>($"/api/slots/{meeting.Id}");
        //        int yOffset = 40;

        //        if (slotsResponse?.Success == true && slotsResponse.Data != null && slotsResponse.Data.Count > 0)
        //        {
        //            Label lblSub = new Label { Text = "추천 시간대:", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(10, yOffset), AutoSize = true };
        //            pnlCard.Controls.Add(lblSub);
        //            yOffset += 20;

        //            foreach (var slot in slotsResponse.Data)
        //            {
        //                Panel slotRow = new Panel
        //                {
        //                    Location = new Point(10, yOffset),
        //                    Size = new Size(225, 24),
        //                    BackColor = Color.White
        //                };

        //                Label lblSlot = new Label
        //                {
        //                    Text = $"{slot.StartTime:MM/dd HH:mm} ~ {slot.EndTime:HH:mm}",
        //                    Location = new Point(4, 3),
        //                    Size = new Size(145, 18),
        //                    AutoEllipsis = true,
        //                    Font = new Font("맑은 고딕", 9F)
        //                };

        //                string responseStatus = GetCurrentUserSlotResponse(slot);
        //                Label lblResponse = new Label
        //                {
        //                    Text = FormatSlotResponseLabel(responseStatus),
        //                    Location = new Point(150, 3),
        //                    Size = new Size(70, 18),
        //                    TextAlign = ContentAlignment.MiddleRight,
        //                    ForeColor = GetSlotResponseColor(responseStatus),
        //                    Font = new Font("맑은 고딕", 9F, FontStyle.Bold)
        //                };

        //                slotRow.Controls.Add(lblSlot);
        //                slotRow.Controls.Add(lblResponse);
        //                pnlCard.Controls.Add(slotRow);
        //                yOffset += 26;
        //            }
        //        }
        //        else
        //        {
        //            Label lblNoSlot = new Label { Text = "조율 중이거나 슬롯이 없습니다.", Location = new Point(10, yOffset), AutoSize = true };
        //            pnlCard.Controls.Add(lblNoSlot);
        //            yOffset += 25;
        //        }

        //        // 응답하기 버튼 생성
        //        Button btnRespond = new Button
        //        {
        //            Text = "시간대 응답하기",
        //            BackColor = Color.CornflowerBlue,
        //            ForeColor = Color.White,
        //            FlatStyle = FlatStyle.Flat,
        //            Location = new Point(10, yOffset + 10),
        //            Size = new Size(225, 30)
        //        };
        //        btnRespond.Click += (s, e) =>
        //        {
        //            using (var respondForm = new RespondSlotForm(meeting.Id, meeting.Title))
        //            {
        //                if (respondForm.ShowDialog() == DialogResult.OK)
        //                {
        //                    _ = LoadAndRenderTeamMeetingsAsync(); // 응답 후 패널 새로고침
        //                }
        //            }
        //        };
        //        pnlCard.Controls.Add(btnRespond);
        //        pnlCard.Height = btnRespond.Bottom + 15;

        //        flpMeetingSidebar.Controls.Add(pnlCard);
        //    }
        //}

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
                bool isCreator = meeting.CreatorId == ApiService.CurrentUserId;
                bool isFinalized = meeting.Status == "finalized";

                // 미팅별 카드(패널) 생성
                Panel pnlCard = new Panel
                {
                    Width = 260, // UI 공간 확보를 위해 약간 넓힘
                    AutoSize = true,
                    BackColor = isFinalized ? Color.WhiteSmoke : Color.White, // 확정된 미팅은 배경색 변경
                    Margin = new Padding(10),
                    Padding = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // 제목 (확정 여부 표시)
                string titleText = isFinalized ? $"[확정됨] {meeting.Title}" : $"{meeting.Title} ({meeting.DurationMinutes}분)";
                Label lblTitle = new Label
                {
                    Text = titleText,
                    Font = new Font("맑은 고딕", 11F, FontStyle.Bold),
                    ForeColor = isFinalized ? Color.MediumSeaGreen : Color.Crimson,
                    AutoSize = true,
                    Location = new Point(10, 10)
                };
                pnlCard.Controls.Add(lblTitle);

                // 추천 슬롯 데이터 가져오기
                var slotsResponse = await ApiService.GetAsync<ApiResponse<List<MeetingSlotDto>>>($"/api/slots/{meeting.Id}");
                int yOffset = 40;

                if (slotsResponse?.Success == true && slotsResponse.Data != null && slotsResponse.Data.Count > 0)
                {
                    Label lblSub = new Label { Text = "추천 시간대 및 응답 현황:", Font = new Font("맑은 고딕", 9F, FontStyle.Bold), Location = new Point(10, yOffset), AutoSize = true };
                    pnlCard.Controls.Add(lblSub);
                    yOffset += 20;

                    foreach (var slot in slotsResponse.Data)
                    {
                        Panel slotRow = new Panel
                        {
                            Location = new Point(10, yOffset),
                            Size = new Size(240, isCreator && !isFinalized ? 50 : 40), // 생성자면 버튼 공간 추가
                            BackColor = Color.Transparent
                        };

                        // 1. 시간대 라벨
                        Label lblSlot = new Label
                        {
                            Text = $"{slot.StartTime:MM/dd HH:mm} ~ {slot.EndTime:HH:mm}",
                            Location = new Point(0, 3),
                            Size = new Size(160, 18),
                            AutoEllipsis = true,
                            Font = new Font("맑은 고딕", 9F)
                        };

                        // 2. 나의 응답 상태 라벨
                        string responseStatus = GetCurrentUserSlotResponse(slot);
                        Label lblMyResponse = new Label
                        {
                            Text = FormatSlotResponseLabel(responseStatus),
                            Location = new Point(165, 3),
                            Size = new Size(65, 18),
                            TextAlign = ContentAlignment.MiddleRight,
                            ForeColor = GetSlotResponseColor(responseStatus),
                            Font = new Font("맑은 고딕", 9F, FontStyle.Bold)
                        };

                        // 3. 팀원 전체 응답 요약 (참석 2 | 불참 1 | 미정 0)
                        int yesCount = slot.Responses?.Count(r => r.Response?.ToLower() == "yes") ?? 0;
                        int noCount = slot.Responses?.Count(r => r.Response?.ToLower() == "no") ?? 0;
                        int maybeCount = slot.Responses?.Count(r => r.Response?.ToLower() == "maybe" || string.IsNullOrEmpty(r.Response)) ?? 0;

                        Label lblSummary = new Label
                        {
                            Text = $"참석 {yesCount} | 불참 {noCount} | 미정 {maybeCount}",
                            Location = new Point(0, 22),
                            AutoSize = true,
                            Font = new Font("맑은 고딕", 8F),
                            ForeColor = Color.DimGray
                        };

                        slotRow.Controls.Add(lblSlot);
                        slotRow.Controls.Add(lblMyResponse);
                        slotRow.Controls.Add(lblSummary);

                        // 4. 팀장(생성자)을 위한 '미팅 확정' 버튼
                        if (isCreator && !isFinalized)
                        {
                            Button btnFinalize = new Button
                            {
                                Text = "확정",
                                Location = new Point(180, 20),
                                Size = new Size(50, 24),
                                BackColor = Color.MediumSeaGreen,
                                ForeColor = Color.White,
                                FlatStyle = FlatStyle.Flat,
                                Font = new Font("맑은 고딕", 8F, FontStyle.Bold)
                            };

                            btnFinalize.Click += async (s, e) =>
                            {
                                var confirmResult = MessageBox.Show($"이 시간대로 미팅을 확정하시겠습니까?\n{lblSlot.Text}", "미팅 확정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (confirmResult == DialogResult.Yes)
                                {
                                    btnFinalize.Enabled = false;
                                    var requestPayload = new FinalizeMeetingRequest { SlotId = slot.Id };
                                    var finalizeRes = await ApiService.PostAsync<FinalizeMeetingRequest, ApiResponse<object>>($"/api/meetings/{meeting.Id}/finalize", requestPayload);

                                    if (finalizeRes?.Success == true)
                                    {
                                        MessageBox.Show("미팅이 성공적으로 확정되었습니다!");
                                        await LoadAndRenderTeamMeetingsAsync(); // UI 새로고침
                                    }
                                    else
                                    {
                                        MessageBox.Show("미팅 확정 실패: " + finalizeRes?.Message);
                                        btnFinalize.Enabled = true;
                                    }
                                }
                            };
                            slotRow.Controls.Add(btnFinalize);
                        }

                        pnlCard.Controls.Add(slotRow);
                        yOffset += slotRow.Height + 5;
                    }
                }
                else
                {
                    Label lblNoSlot = new Label { Text = "조율 중이거나 슬롯이 없습니다.", Location = new Point(10, yOffset), AutoSize = true };
                    pnlCard.Controls.Add(lblNoSlot);
                    yOffset += 25;
                }

                // 응답하기 버튼 (확정되지 않은 미팅일 때만 표시)
                if (!isFinalized)
                {
                    Button btnRespond = new Button
                    {
                        Text = "시간대 응답하기",
                        BackColor = Color.CornflowerBlue,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Location = new Point(10, yOffset + 5),
                        Size = new Size(240, 30)
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
                }
                else
                {
                    pnlCard.Height = yOffset + 10;
                }

                flpMeetingSidebar.Controls.Add(pnlCard);
            }
        }

        private void UpdateCalendarAreaSize()
        {
            int bottomPadding = 20;
            int legendGap = 8;
            int legendHeight = 82;

            if (_isTeamCalendar)
            {
                flpMeetingSidebar.Location = new Point(
                    this.ClientSize.Width - flpMeetingSidebar.Width - FormPadding,
                    pnlCalendar.Top
                );

                pnlCalendar.Width = Math.Max(
                    700,
                    flpMeetingSidebar.Left - pnlCalendar.Left - SidebarGap
                );

                int calendarHeight = this.ClientSize.Height - pnlCalendar.Top - legendGap - legendHeight - bottomPadding;
                pnlCalendar.Height = Math.Max(420, calendarHeight);
                flpMeetingSidebar.Height = pnlCalendar.Height;

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
                pnlCalendar.Width = this.ClientSize.Width - (pnlCalendar.Left * 2);
                int calendarHeight = this.ClientSize.Height - pnlCalendar.Top - bottomPadding;
                pnlCalendar.Height = Math.Max(500, calendarHeight);
                tlpTeamLegend.Location = new Point(pnlCalendar.Left, pnlCalendar.Bottom + legendGap);
                tlpTeamLegend.Size = new Size(pnlCalendar.Width, legendHeight);
            }
        }

        private void ApplyCalendarModeSize()
        {
            int targetWidth = _isTeamCalendar ? TeamClientWidth : PersonalClientWidth;
            if (this.ClientSize.Width != targetWidth)
            {
                this.ClientSize = new Size(targetWidth, this.ClientSize.Height);
                this.CenterToScreen();
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
