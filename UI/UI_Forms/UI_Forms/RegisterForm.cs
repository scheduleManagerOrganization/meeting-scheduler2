using System;
using System.Windows.Forms;
using System.Xml.Linq;
using UI_Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            // 1. 입력값 검증
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("모든 항목을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. 가입 요청 데이터 세팅
            var requestData = new RegisterRequest
            {
                Name = txtName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text
            };

            btnSubmit.Enabled = false; // 중복 클릭 방지

            try
            {
                // 3. API 호출 (POST /api/auth/register)
                var response = await ApiService.PostAsync<RegisterRequest, ApiResponse<LoginResponseData>>("/api/auth/register", requestData);

                if (response.Success)
                {
                    MessageBox.Show("회원가입이 완료되었습니다!\n로그인 화면에서 로그인해주세요.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close(); // 성공 시 모달 폼 닫기
                }
                else
                {
                    MessageBox.Show($"회원가입 실패: {response.Error}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "네트워크 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSubmit.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}