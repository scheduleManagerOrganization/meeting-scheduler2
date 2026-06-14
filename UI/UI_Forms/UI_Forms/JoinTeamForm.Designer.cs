namespace UI_Forms
{
    partial class JoinTeamForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtJoinCode;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnShowMyCodes;
        private System.Windows.Forms.TextBox txtMyTeamsCodes;
        private System.Windows.Forms.Panel pnlDivider;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtJoinCode = new System.Windows.Forms.TextBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnShowMyCodes = new System.Windows.Forms.Button();
            this.txtMyTeamsCodes = new System.Windows.Forms.TextBox();
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(161, 19);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "참가할 초대 코드 입력";

            // txtJoinCode
            this.txtJoinCode.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtJoinCode.Location = new System.Drawing.Point(30, 55);
            this.txtJoinCode.Name = "txtJoinCode";
            this.txtJoinCode.Size = new System.Drawing.Size(220, 25);
            this.txtJoinCode.TabIndex = 1;

            // btnJoin (MediumSeaGreen 디자인 적용)
            this.btnJoin.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnJoin.FlatAppearance.BorderSize = 0;
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnJoin.ForeColor = System.Drawing.Color.White;
            this.btnJoin.Location = new System.Drawing.Point(260, 53);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(90, 29);
            this.btnJoin.TabIndex = 2;
            this.btnJoin.Text = "참가";
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);

            // pnlDivider (구분선)
            this.pnlDivider.BackColor = System.Drawing.Color.LightGray;
            this.pnlDivider.Location = new System.Drawing.Point(30, 105);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(320, 1);
            this.pnlDivider.TabIndex = 5;

            // btnShowMyCodes (MediumSeaGreen 테두리 강조 스타일 적용)
            this.btnShowMyCodes.BackColor = System.Drawing.Color.White;
            this.btnShowMyCodes.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnShowMyCodes.FlatAppearance.BorderSize = 1;
            this.btnShowMyCodes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMyCodes.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnShowMyCodes.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.btnShowMyCodes.Location = new System.Drawing.Point(30, 125);
            this.btnShowMyCodes.Name = "btnShowMyCodes";
            this.btnShowMyCodes.Size = new System.Drawing.Size(320, 35);
            this.btnShowMyCodes.TabIndex = 3;
            this.btnShowMyCodes.Text = "내 소속 팀 초대코드 확인하기";
            this.btnShowMyCodes.UseVisualStyleBackColor = false;
            this.btnShowMyCodes.Click += new System.EventHandler(this.btnShowMyCodes_Click);

            // txtMyTeamsCodes
            this.txtMyTeamsCodes.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtMyTeamsCodes.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMyTeamsCodes.Location = new System.Drawing.Point(30, 175);
            this.txtMyTeamsCodes.Multiline = true;
            this.txtMyTeamsCodes.Name = "txtMyTeamsCodes";
            this.txtMyTeamsCodes.ReadOnly = true;
            this.txtMyTeamsCodes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMyTeamsCodes.Size = new System.Drawing.Size(320, 110);
            this.txtMyTeamsCodes.TabIndex = 4;

            // JoinTeamForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(380, 310);
            this.Controls.Add(this.pnlDivider);
            this.Controls.Add(this.txtMyTeamsCodes);
            this.Controls.Add(this.btnShowMyCodes);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.txtJoinCode);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JoinTeamForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "팀 참가";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}