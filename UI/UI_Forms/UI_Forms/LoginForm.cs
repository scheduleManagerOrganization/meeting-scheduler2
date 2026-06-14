using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // 폼 로드 이벤트 연결
            this.Load += LoginForm_Load;
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            // 프로그램 시작과 함께 서버 상태 확인
            await CheckServerStatusAsync();
        }

        private async Task CheckServerStatusAsync()
        {
            try
            {
                // ApiService를 통해 /health 엔드포인트 호출
                var response = await ApiService.GetAsync<HealthResponse>("/health");

                // 정상적이면 아무 작업도 하지 않고 넘어감 (Silent)
                if (response == null || response.Status != "healthy")
                {
                    MessageBox.Show("서버 상태가 불안정합니다. 잠시 후 다시 시도해 주세요.", "서버 상태 알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception)
            {
                // 통신 실패 (서버가 닫혀 있거나 깨어나는 데 실패함)
                MessageBox.Show("서버와 연결할 수 없습니다.\n백엔드 서버가 구동 중인지 확인해 주세요.", "서버 연결 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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