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

        private void ApplyResponsiveFormSize()
        {
            Rectangle workArea = Screen.FromControl(this).WorkingArea;
            int targetWidth = Math.Min(1109, workArea.Width - 40);
            int targetHeight = Math.Min(800, workArea.Height - 60);

            // 작은 화면에서도 캘린더 하단이 잘리지 않도록 폼 크기를 현재 작업 영역 안으로 제한합니다.
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

                var fetchTasks = new List<Task>();
                for (int i = 0; i < totalDays; i++)
                {
                    int index = i;
                    DateTime targetDate = startDate.AddDays(index);
                    string dateStr = targetDate.ToString("yyyy-MM-dd");

                    fetchTasks.Add(RenderDaySchedulesAsync(index, userId, dateStr, renderVersion));
                }
                await Task.WhenAll(fetchTasks);

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

        private async Task RenderDaySchedulesAsync(int index, string userId, string dateStr, int renderVersion)
        {
            if (_isTeamCalendar)
            {
                string selectedTeamId = cmbTeams.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(selectedTeamId)) return;

                var response = await ApiService.GetAsync<ApiResponse<List<TeamAvailabilityData>>>($"/api/availability/team/{selectedTeamId}/{dateStr}");
                if (!IsActiveRenderCell(renderVersion, index, dateStr)) return;
                if (response?.Success != true || response.Data == null) return;

                // 팀 캘린더는 같은 날짜 안에서 시작이 빠른 순, 종료가 늦은 순으로 쌓아 겹친 시간의 길이 차이를 눈에 보이게 합니다.
                var teamSlots = response.Data
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
                    _dayControls[index].AddTeamAvailabilitySlot(
                        item.Slot.Start,
                        item.Slot.End,
                        userColor);
                }

                return;
            }

            var personalResponse = await ApiService.GetAsync<ApiResponse<AvailabilityData>>($"/api/availability/{userId}/{dateStr}");
            if (!IsActiveRenderCell(renderVersion, index, dateStr)) return;
            if (personalResponse?.Success != true || personalResponse.Data?.Slots == null) return;

            foreach (var slot in personalResponse.Data.Slots)
            {
                string displayTitle = $"{slot.Start} - {slot.End}";
                bool isFullBox = slot.Start == "00:00" && slot.End == "23:59";
                _dayControls[index].AddScheduleSlot(displayTitle, Color.CornflowerBlue, isFullBox);
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
            // 기존 AddScheduleForm 대신 새로 만든 AddAvailabilityForm 호출
            using (AddAvailabilityForm addForm = new AddAvailabilityForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    await RenderCalendarAsync(); // 다이얼로그 확인 후 꺼지면 자동 새로고침
                }
            }
        }

        private void UpdateCalendarModeUi()
        {
            string userName = ApiService.CurrentUserName ?? "사용자";

            // 팀 선택 콤보박스는 팀 캘린더 모드에서만 보여 사용자가 현재 모드를 헷갈리지 않게 합니다.
            cmbTeams.Visible = _isTeamCalendar;
            tlpTeamLegend.Visible = _isTeamCalendar;
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

            // 개인 캘린더는 범례가 없으므로 하단 여백까지 캘린더가 채우고, 팀 캘린더는 범례 영역을 남겨둡니다.
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

        private bool IsActiveRenderCell(int renderVersion, int index, string dateStr)
        {
            // 달/모드 전환 전에 시작된 늦은 API 응답이 현재 달의 같은 인덱스 칸에 섞이는 것을 막습니다.
            return renderVersion == _renderVersion
                && index >= 0
                && index < _dayControls.Count
                && _dayControls[index].Date.ToString("yyyy-MM-dd") == dateStr;
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

            if (!_isTeamCalendar || _teamUserNames.Count == 0)
            {
                return;
            }

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
