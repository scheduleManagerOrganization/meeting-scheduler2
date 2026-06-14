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

            // 설명칸이 플레이스홀더 상태면 빈 문자열로 서버에 보냄
            string description = (txtDescription.Text == PlaceholderDesc) ? "" : txtDescription.Text;

            btnSave.Enabled = false;

            try
            {
                var payload = new
                {
                    team_id = _teamId,
                    title = txtTitle.Text,
                    description = description,
                    duration_minutes = durationMinutes,
                    creator_id = ApiService.CurrentUserId,
                    deadline_date = dtpDeadline.Value.ToString("yyyy-MM-dd")
                };

                var postResponse = await ApiService.PostAsync<object, ApiResponse<object>>("/api/meetings", payload);

                if (postResponse != null && postResponse.Success)
                {
                    MessageBox.Show("팀 미팅이 성공적으로 생성되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
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
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}