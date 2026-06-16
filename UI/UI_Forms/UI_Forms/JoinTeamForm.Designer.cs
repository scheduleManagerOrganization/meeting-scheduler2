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
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 19);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "참가할 초대 코드 입력";
            // 
            // txtJoinCode
            // 
            this.txtJoinCode.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtJoinCode.Location = new System.Drawing.Point(30, 44);
            this.txtJoinCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtJoinCode.Name = "txtJoinCode";
            this.txtJoinCode.Size = new System.Drawing.Size(220, 25);
            this.txtJoinCode.TabIndex = 1;
            // 
            // btnJoin
            // 
            this.btnJoin.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnJoin.FlatAppearance.BorderSize = 0;
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnJoin.ForeColor = System.Drawing.Color.White;
            this.btnJoin.Location = new System.Drawing.Point(260, 42);
            this.btnJoin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(90, 27);
            this.btnJoin.TabIndex = 2;
            this.btnJoin.Text = "참가";
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnShowMyCodes
            // 
            this.btnShowMyCodes.BackColor = System.Drawing.Color.White;
            this.btnShowMyCodes.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnShowMyCodes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMyCodes.Font = new System.Drawing.Font("맑은 고딕", 9.5F);
            this.btnShowMyCodes.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.btnShowMyCodes.Location = new System.Drawing.Point(30, 100);
            this.btnShowMyCodes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnShowMyCodes.Name = "btnShowMyCodes";
            this.btnShowMyCodes.Size = new System.Drawing.Size(320, 28);
            this.btnShowMyCodes.TabIndex = 3;
            this.btnShowMyCodes.Text = "내 소속 팀 초대코드 확인하기";
            this.btnShowMyCodes.UseVisualStyleBackColor = false;
            this.btnShowMyCodes.Click += new System.EventHandler(this.btnShowMyCodes_Click);
            // 
            // txtMyTeamsCodes
            // 
            this.txtMyTeamsCodes.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtMyTeamsCodes.Font = new System.Drawing.Font("맑은 고딕", 9.5F);
            this.txtMyTeamsCodes.Location = new System.Drawing.Point(30, 140);
            this.txtMyTeamsCodes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMyTeamsCodes.Multiline = true;
            this.txtMyTeamsCodes.Name = "txtMyTeamsCodes";
            this.txtMyTeamsCodes.ReadOnly = true;
            this.txtMyTeamsCodes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMyTeamsCodes.Size = new System.Drawing.Size(320, 89);
            this.txtMyTeamsCodes.TabIndex = 4;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.LightGray;
            this.pnlDivider.Location = new System.Drawing.Point(30, 84);
            this.pnlDivider.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(320, 1);
            this.pnlDivider.TabIndex = 5;
            // 
            // JoinTeamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(380, 248);
            this.Controls.Add(this.pnlDivider);
            this.Controls.Add(this.txtMyTeamsCodes);
            this.Controls.Add(this.btnShowMyCodes);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.txtJoinCode);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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