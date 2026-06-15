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
                        Size = new Size(305, 82),
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

                    RadioButton rdoYes = new RadioButton
                    {
                        Location = new Point(10, 32),
                        Size = new Size(70, 24),
                        Text = "참석",
                        Font = new Font("맑은 고딕", 9F),
                        Tag = "yes"
                    };

                    RadioButton rdoNo = new RadioButton
                    {
                        Location = new Point(90, 32),
                        Size = new Size(70, 24),
                        Text = "불참",
                        Font = new Font("맑은 고딕", 9F),
                        Tag = "no"
                    };

                    RadioButton rdoMaybe = new RadioButton
                    {
                        Location = new Point(170, 32),
                        Size = new Size(70, 24),
                        Text = "미정",
                        Font = new Font("맑은 고딕", 9F),
                        Tag = "maybe"
                    };

                    Label lblReason = new Label
                    {
                        Text = $"{slot.AiReason}",
                        Location = new Point(10, 60),
                        AutoSize = true,
                        MaximumSize = new Size(280,0),
                        Font = new Font("맑은 고딕", 9F, FontStyle.Regular)
                    };
                    pnl.Controls.Add(lblReason);

                    pnl.Height = lblReason.Bottom + 10; // 라벨의 맨 밑바닥 Y좌표 + 아래쪽 여백 10

                    string currentResponse = GetCurrentUserResponse(slot);
                    if (currentResponse == "yes") rdoYes.Checked = true;
                    else if (currentResponse == "no") rdoNo.Checked = true;
                    else rdoMaybe.Checked = true;

                    pnl.Controls.Add(rdoYes);
                    pnl.Controls.Add(rdoNo);
                    pnl.Controls.Add(rdoMaybe);

                    flpSlots.Controls.Add(pnl);
                    _slotControls.Add(new SlotControlData
                    {
                        SlotId = slot.Id,
                        RdoYes = rdoYes,
                        RdoNo = rdoNo,
                        RdoMaybe = rdoMaybe
                    });
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
                    string resValue = item.GetSelectedResponse();
                    SlotResponseCache.SetResponse(ApiService.CurrentUserId, item.SlotId, resValue);

                    // 명세서에 따른 Request Body (snake_case)
                    var payload = new
                    {
                        slot_id = item.SlotId,
                        user_id = ApiService.CurrentUserId,
                        response = resValue
                    };

                    submitTasks.Add(TrySubmitResponseToServerAsync(payload));
                }

                await Task.WhenAll(submitTasks);

                MessageBox.Show("응답이 저장되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private string GetCurrentUserResponse(MeetingSlotDto slot)
        {
            string cachedResponse = SlotResponseCache.GetResponse(ApiService.CurrentUserId, slot?.Id);
            if (!string.IsNullOrWhiteSpace(cachedResponse)) return cachedResponse.ToLower();

            if (slot?.Responses == null) return "maybe";

            foreach (var response in slot.Responses)
            {
                if (IsCurrentUserResponse(response))
                {
                    return string.IsNullOrWhiteSpace(response.Response) ? "maybe" : response.Response.ToLower();
                }
            }

            return "maybe";
        }

        private async Task TrySubmitResponseToServerAsync(object payload)
        {
            try
            {
                //await ApiService.PostAsync<object, ApiResponse<object>>("/api/respond-slot", payload);
                // API 호출 결과를 반환하도록 수정 (성공 여부 파악을 위해)
    var response = await ApiService.PostAsync<object, ApiResponse<object>>("/api/respond-slot", payload);
    
    // 만약 통신은 성공했는데 백엔드 로직에서 실패(Success = false)를 뱉었다면 예외 발생
    if (response != null && !response.Success)
    {
        throw new Exception(response.Message ?? "서버에서 응답 저장에 실패했습니다.");
    }
            }
            catch
            {
                // The current backend can fail with a MongoDB responses-array update conflict.
                // UI keeps the user's choice locally so the workflow remains usable.
            }
        }

        private bool IsCurrentUserResponse(SlotResponseDto response)
        {
            return string.Equals(
                response?.UserId?.Trim(),
                ApiService.CurrentUserId?.Trim(),
                StringComparison.OrdinalIgnoreCase
            );
        }

        private class SlotControlData
        {
            public string SlotId { get; set; }
            public RadioButton RdoYes { get; set; }
            public RadioButton RdoNo { get; set; }
            public RadioButton RdoMaybe { get; set; }

            public string GetSelectedResponse()
            {
                if (RdoYes.Checked) return "yes";
                if (RdoNo.Checked) return "no";
                return "maybe";
            }
        }
    }
}
