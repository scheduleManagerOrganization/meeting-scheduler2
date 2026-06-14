namespace UI_Forms
{
    partial class RespondSlotForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.FlowLayoutPanel flpSlots;
        private System.Windows.Forms.Button btnSubmit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.flpSlots = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblHeader.Location = new System.Drawing.Point(20, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(253, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "시간대별 참석 여부를 선택해주세요";
            // 
            // flpSlots
            // 
            this.flpSlots.AutoScroll = true;
            this.flpSlots.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flpSlots.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpSlots.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSlots.Location = new System.Drawing.Point(20, 60);
            this.flpSlots.Name = "flpSlots";
            this.flpSlots.Padding = new System.Windows.Forms.Padding(5);
            this.flpSlots.Size = new System.Drawing.Size(340, 320);
            this.flpSlots.TabIndex = 1;
            this.flpSlots.WrapContents = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(140, 400);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 35);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "응답 제출";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // RespondSlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.flpSlots);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RespondSlotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AI 슬롯 응답";
            this.Load += new System.EventHandler(this.RespondSlotForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
