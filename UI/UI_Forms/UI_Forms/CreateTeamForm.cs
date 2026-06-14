using System;
using System.Windows.Forms;
using UI_Forms.Models; 

namespace UI_Forms
{
    public partial class CreateTeamForm : Form
    {
        public CreateTeamForm()
        {
            InitializeComponent();
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeamName.Text))
            {
                MessageBox.Show("팀 이름을 입력해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnCreate.Enabled = false;

            try
            {
                // API 명세에 맞춘 페이로드 구성
                var payload = new
                {
                    team_name = txtTeamName.Text.Trim(),
                    owner_id = ApiService.CurrentUserId,
                    description = txtDescription.Text.Trim()
                };

                // POST 요청 전송
                var response = await ApiService.PostAsync<object, ApiResponse<TeamCreateResponseData>>("/api/teams", payload);

                if (response != null && response.Success && response.Data != null)
                {
                    MessageBox.Show($"팀이 성공적으로 생성되었습니다!\n\n초대 코드: {response.Data.join_code}\n(팀원들에게 이 코드를 공유하세요)",
                                    "생성 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response?.Error ?? "팀 생성에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"서버와 통신 중 오류가 발생했습니다.\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCreate.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}