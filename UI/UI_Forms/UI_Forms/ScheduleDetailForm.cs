using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class ScheduleDetailForm : Form
    {
        private DateTime _scheduleDate;
        private SlotDto _selectedSlot;
        private string _teamId;

        public ScheduleDetailForm(DateTime date, SlotDto slot, string teamId = null)
        {
            InitializeComponent();
            _scheduleDate = date;
            _selectedSlot = slot;
            _teamId = teamId; // 팀 일정인 경우 팀 ID를 받아옵니다.

            LoadScheduleDetails();
        }

        private void LoadScheduleDetails()
        {
            lblTitle.Text = string.IsNullOrWhiteSpace(_selectedSlot.Title) ? "(제목 없음)" : _selectedSlot.Title;
            lblTime.Text = $"시간: {_selectedSlot.Start} ~ {_selectedSlot.End}";
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("이 일정을 정말 삭제하시겠습니까?", "일정 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult != DialogResult.Yes) return;

            btnDelete.Enabled = false;
            btnDelete.Text = "삭제 중...";

            try
            {
                string userId = ApiService.CurrentUserId;
                string dateStr = _scheduleDate.ToString("yyyy-MM-dd");

                // 1단계: 기존 일정 전체 불러오기 (GET)
                string getUrl = $"/api/availability/{userId}/{dateStr}";
                var getResponse = await ApiService.GetAsync<ApiResponse<AvailabilityData>>(getUrl);

                var slots = getResponse?.Data?.Slots ?? new List<SlotDto>();

                // 2단계: 리스트에서 삭제할 일정 찾아서 제거 (Remove)
                var slotToRemove = slots.FirstOrDefault(s =>
                    s.Start == _selectedSlot.Start &&
                    s.End == _selectedSlot.End &&
                    s.Title == _selectedSlot.Title);

                if (slotToRemove != null)
                {
                    slots.Remove(slotToRemove);
                }

                // 3단계: 일정이 제거된 새 리스트를 다시 덮어쓰기 (POST)
                var payload = new
                {
                    user_id = userId,
                    date = dateStr,
                    team_id = _teamId, // 개인 일정이면 null, 팀 일정이면 teamId 포함
                    slots = slots
                };

                var postResponse = await ApiService.PostAsync<object, ApiResponse<object>>("/api/availability", payload);

                if (postResponse != null && postResponse.Success)
                {
                    MessageBox.Show("일정이 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // 메인 폼에 성공을 알려 달력 새로고침을 유도
                    this.Close();
                }
                else
                {
                    MessageBox.Show("일정 삭제에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDelete.Enabled = true;
                btnDelete.Text = "일정 삭제";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}