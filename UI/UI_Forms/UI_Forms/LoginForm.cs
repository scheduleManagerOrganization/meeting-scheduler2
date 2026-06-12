using UI_Forms.Models;
using System;
using System.Windows.Forms;

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
                    ApiService.SetAuthData(response.Data.Token, response.Data.UserId, response.Data.Name);

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

        private async void btnInitDb_Click(object sender, EventArgs e)
        {
            // 사용자에게 한 번 더 확인받기 (실수로 누르는 것 방지)
            DialogResult dialogResult = MessageBox.Show(
                "정말로 DB를 초기화하시겠습니까?\n모든 데이터가 날아가고 기본 테스트 계정만 남습니다.",
                "DB 초기화 경고",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                btnInitDb.Enabled = false; // 중복 클릭 방지

                try
                {
                    // 전송할 데이터가 없으므로 빈 익명 객체를 생성해서 보냄
                    var emptyRequest = new { };

                    // API 호출 (POST /api/init-db)
                    // 응답 데이터(Data)의 상세 타입이 중요하지 않으므로 object로 받음
                    var response = await ApiService.PostAsync<object, ApiResponse<object>>("/api/init-db", emptyRequest);

                    if (response != null && response.Success)
                    {
                        MessageBox.Show("DB가 완벽하게 초기화되었습니다!\n이제 깔끔한 상태로 다시 테스트해 보세요.",
                                        "초기화 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"DB 초기화 실패: {response?.Error}",
                                        "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"서버와 통신 중 오류가 발생했습니다.\n{ex.Message}",
                                    "네트워크 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnInitDb.Enabled = true;
                }
            }
        }
    }
}