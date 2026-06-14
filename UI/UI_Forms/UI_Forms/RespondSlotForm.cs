using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Forms.Models;

namespace UI_Forms
{
    public partial class RespondSlotForm : Form
    {
        private string _meetingId;
        private string _meetingTitle;
        private List<SlotControlData> _slotControls = new List<SlotControlData>();

        public RespondSlotForm(string meetingId, string meetingTitle)
        {
            InitializeComponent();
            _meetingId = meetingId;
            _meetingTitle = meetingTitle;
            this.Text = $"{_meetingTitle} - AI 슬롯 응답";
        }

        private async void RespondSlotForm_Load(object sender, EventArgs e)
        {
            // 모델 변경: MeetingSlotDto 사용
            var response = await ApiService.GetAsync<ApiResponse<List<MeetingSlotDto>>>($"/api/slots/{_meetingId}");

            if (response?.Success == true && response.Data != null)
            {
                flpSlots.Controls.Clear();

                foreach (var slot in response.Data)
                {
                    Panel pnl = new Panel
                    {
                        Size = new Size(305, 60),
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(0, 0, 0, 10),
                        BackColor = Color.White
                    };

                    Label lblTime = new Label
                    {
                        Text = $"{slot.StartTime:MM/dd HH:mm} ~ {slot.EndTime:HH:mm}",
                        Location = new Point(10, 10),
                        AutoSize = true,
                        Font = new Font("맑은 고딕", 9F, FontStyle.Bold)
                    };
                    pnl.Controls.Add(lblTime);

                    ComboBox cmbResponse = new ComboBox
                    {
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Location = new Point(10, 32),
                        Size = new Size(110, 23),
                        Font = new Font("맑은 고딕", 9F)
                    };
                    cmbResponse.Items.AddRange(new string[] { "yes (참석)", "no (불참)", "maybe (미정)" });
                    cmbResponse.SelectedIndex = 0;
                    pnl.Controls.Add(cmbResponse);

                    flpSlots.Controls.Add(pnl);
                    _slotControls.Add(new SlotControlData { SlotId = slot.Id, CmbResponse = cmbResponse });
                }

                if (_slotControls.Count == 0)
                {
                    flpSlots.Controls.Add(new Label { Text = "표시할 추천 슬롯이 없습니다.", AutoSize = true, Location = new Point(10, 10) });
                }
            }
        }

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            btnSubmit.Enabled = false;
            try
            {
                var submitTasks = new List<Task>();

                foreach (var item in _slotControls)
                {
                    string resValue = item.CmbResponse.Text.Split(' ')[0]; // "yes", "no", "maybe"

                    // 명세서에 따른 Request Body (snake_case)
                    var payload = new
                    {
                        slot_id = item.SlotId,
                        user_id = ApiService.CurrentUserId,
                        response = resValue
                    };

                    submitTasks.Add(ApiService.PostAsync<object, ApiResponse<object>>("/api/respond-slot", payload));
                }

                await Task.WhenAll(submitTasks); // API 병렬 요청 대기

                MessageBox.Show("응답이 성공적으로 제출되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류 발생: " + ex.Message);
            }
            finally
            {
                btnSubmit.Enabled = true;
            }
        }

        // 내부 컨트롤 매핑 클래스
        private class SlotControlData
        {
            public string SlotId { get; set; }
            public ComboBox CmbResponse { get; set; }
        }
    }
}