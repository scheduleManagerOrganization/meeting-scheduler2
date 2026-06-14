using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UI_Forms.Models; // 🌟 Models 네임스페이스 포함

namespace UI_Forms
{
    public partial class JoinTeamForm : Form
    {
        public JoinTeamForm()
        {
            InitializeComponent();
        }

        // 1. 팀 참가 버튼 클릭 이벤트
        private async void btnJoin_Click(object sender, EventArgs e)
        {
            string joinCode = txtJoinCode.Text.Trim();
            if (string.IsNullOrEmpty(joinCode))
            {
                MessageBox.Show("초대 코드를 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var payload = new
            {
                join_code = joinCode,
                user_id = ApiService.CurrentUserId
            };

            // 🌟 수정된 핵심 부분: ApiResponse가 <T>를 요구하므로 <object>를 넣어줍니다.
            // ApiService.PostAsync<요청타입, 응답타입> 형식에 완벽하게 일치합니다.
            var response = await ApiService.PostAsync<object, ApiResponse<object>>("/api/teams/join", payload);

            if (response != null && response.Success)
            {
                MessageBox.Show("성공적으로 팀에 참가했습니다!", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // 성공 시 메인 폼 캘린더 갱신을 위해 OK 반환
                this.Close();
            }
            else
            {
                MessageBox.Show(response?.Error ?? "팀 참가에 실패했거나 이미 소속된 팀입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. 내 팀 초대코드 불러오기 이벤트
        private async void btnShowMyCodes_Click(object sender, EventArgs e)
        {
            string userId = ApiService.CurrentUserId;

            // GET 요청은 Request Body가 없으므로 응답 타입(ApiResponse<List<TeamDto>>)만 명시
            var response = await ApiService.GetAsync<ApiResponse<List<TeamDto>>>($"/api/teams/{userId}");

            if (response != null && response.Success && response.Data != null)
            {
                StringBuilder sb = new StringBuilder();
                if (response.Data.Count == 0)
                {
                    sb.AppendLine("현재 속한 팀이 없습니다.");
                }
                else
                {
                    foreach (var team in response.Data)
                    {
                        sb.AppendLine($"[{team.TeamName}] 초대코드: {team.JoinCode}");
                        sb.AppendLine("----------------------------------------");
                    }
                }

                txtMyTeamsCodes.Text = sb.ToString();
            }
            else
            {
                MessageBox.Show("팀 목록을 불러오는데 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}