using UI_Forms.Models;
using System;
using System.Windows.Forms;
using UI_Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("이메일과 비밀번호를 입력해주세요.", "알림");
                return;
            }

            var requestData = new LoginRequest
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text
            };

            btnLogin.Enabled = false;

            try
            {
                // 로그인 API 호출 (POST /api/auth/login)
                var response = await ApiService.PostAsync<LoginRequest, ApiResponse<LoginResponseData>>("/api/auth/login", requestData);

                if (response.Success && response.Data != null)
                {
                    // 토큰 전역 저장
                    ApiService.SetAuthData(response.Data.Token, response.Data.UserId);

                    MessageBox.Show($"{response.Data.Name}님 환영합니다!", "로그인 성공");

                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show($"로그인 실패: {response.Error}", "오류");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류");
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        // 🌟 회원가입 모달 창 띄우기 이벤트 🌟
        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (RegisterForm registerForm = new RegisterForm())
            {
                // 모달 폼으로 띄우기 (회원가입 창이 닫히기 전까지 뒤의 로그인 폼 조작 불가)
                registerForm.ShowDialog();
            }
        }
    }
}