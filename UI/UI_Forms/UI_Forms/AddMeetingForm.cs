using System;
using System.Drawing;
using System.Windows.Forms;
using UI_Forms.Models;
using System.Threading.Tasks;

namespace UI_Forms
{
    public partial class AddMeetingForm : Form
    {
        private const string PlaceholderTitle = "미팅 제목을 입력하세요";
        private const string PlaceholderDesc = "미팅 설명을 입력하세요 (선택)";
        private string _teamId;

        public AddMeetingForm(string teamId)
        {
            InitializeComponent();
            _teamId = teamId;
            SetupPlaceholder();
            SetupDurationComboBox();

            dtpDeadline.Value = DateTime.Today.AddDays(7); // 기본 마감일은 7일 뒤
        }

        private void SetupPlaceholder()
        {
            // 1. 제목 텍스트박스 플레이스홀더 설정
            txtTitle.Text = PlaceholderTitle;
            txtTitle.ForeColor = Color.Gray;

            txtTitle.Enter += (s, e) => {
                if (txtTitle.Text == PlaceholderTitle)
                {
                    txtTitle.Text = "";
                    txtTitle.ForeColor = Color.Black;
                }
            };

            txtTitle.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    txtTitle.Text = PlaceholderTitle;
                    txtTitle.ForeColor = Color.Gray;
                }
            };

            // 2. 설명 텍스트박스 플레이스홀더 설정 (디자이너에서 분리해 온 부분!)
            txtDescription.Text = PlaceholderDesc;
            txtDescription.ForeColor = Color.Gray;

            txtDescription.Enter += (s, e) => {
                if (txtDescription.Text == PlaceholderDesc)
                {
                    txtDescription.Text = "";
                    txtDescription.ForeColor = Color.Black;
                }
            };

            txtDescription.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    txtDescription.Text = PlaceholderDesc;
                    txtDescription.ForeColor = Color.Gray;
                }
            };
        }

        private void SetupDurationComboBox()
        {
            cmbDuration.Items.AddRange(new string[] { "30분", "60분", "90분", "120분", "180분" });
            cmbDuration.SelectedIndex = 1; // 기본값 60분
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || txtTitle.Text == PlaceholderTitle)
            {
                MessageBox.Show("미팅 제목을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int durationMinutes = int.Parse(cmbDuration.SelectedItem.ToString().Replace("분", ""));
            string description = (txtDescription.Text == PlaceholderDesc) ? "" : txtDescription.Text;

            btnSave.Enabled = false;
            btnSave.Text = "추천 진행 중...";

            try
            {
                // 1. 미팅 생성 페이로드
                var payload = new
                {
                    team_id = _teamId,
                    title = txtTitle.Text,
                    description = description,
                    duration_minutes = durationMinutes,
                    creator_id = ApiService.CurrentUserId,
                    deadline_date = dtpDeadline.Value.ToString("yyyy-MM-dd")
                };

                // 미팅 생성 API 호출
                var createResponse = await ApiService.PostAsync<object, ApiResponse<MeetingCreateData>>("/api/meetings", payload);

                if (createResponse != null && createResponse.Success && createResponse.Data != null)
                {
                    string newMeetingId = createResponse.Data.MeetingId;

                    // 2. 미팅 생성 성공 시, 즉시 AI 추천 API 호출
                    var aiPayload = new { meeting_id = newMeetingId };
                    var aiResponse = await ApiService.PostAsync<object, ApiResponse<object>>("/api/ai-recommend-slots", aiPayload);

                    if (aiResponse != null && aiResponse.Success)
                    {
                        MessageBox.Show("미팅 생성 및 AI 추천이 성공적으로 완료되었습니다!", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("미팅은 생성되었으나 AI 추천에 실패했습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.DialogResult = DialogResult.OK; // 미팅 자체는 생성되었으므로 달력 갱신을 위해 OK 반환
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("미팅 생성에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "AI 추천받기";
            }
        }


        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}