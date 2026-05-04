namespace UI_Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAddAvailability;
        private System.Windows.Forms.ComboBox cmbTeams;
        private System.Windows.Forms.ComboBox cmbViewType;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblCurrentMonth;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnCalendarMode;
        private System.Windows.Forms.Panel pnlCalendar;
        private System.Windows.Forms.TableLayoutPanel tlpTeamLegend;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnAddAvailability = new System.Windows.Forms.Button();
            this.cmbTeams = new System.Windows.Forms.ComboBox();
            this.cmbViewType = new System.Windows.Forms.ComboBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblCurrentMonth = new System.Windows.Forms.Label();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnCalendarMode = new System.Windows.Forms.Button();
            this.pnlCalendar = new System.Windows.Forms.Panel();
            this.tlpTeamLegend = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(197, 30);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "사용자님의 캘린더";
            // 
            // btnAddAvailability
            // 
            this.btnAddAvailability.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAvailability.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnAddAvailability.FlatAppearance.BorderSize = 0;
            this.btnAddAvailability.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAvailability.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddAvailability.ForeColor = System.Drawing.Color.White;
            this.btnAddAvailability.Location = new System.Drawing.Point(820, 16);
            this.btnAddAvailability.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddAvailability.Name = "btnAddAvailability";
            this.btnAddAvailability.Size = new System.Drawing.Size(130, 28);
            this.btnAddAvailability.TabIndex = 7;
            this.btnAddAvailability.Text = "+ 가용시간 추가";
            this.btnAddAvailability.UseVisualStyleBackColor = false;
            this.btnAddAvailability.Click += new System.EventHandler(this.btnAddAvailability_Click);
            // 
            // cmbTeams
            // 
            this.cmbTeams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeams.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbTeams.Location = new System.Drawing.Point(556, 20);
            this.cmbTeams.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTeams.Name = "cmbTeams";
            this.cmbTeams.Size = new System.Drawing.Size(140, 25);
            this.cmbTeams.TabIndex = 6;
            this.cmbTeams.Visible = false;
            // 
            // cmbViewType
            // 
            this.cmbViewType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViewType.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbViewType.Location = new System.Drawing.Point(705, 20);
            this.cmbViewType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbViewType.Name = "cmbViewType";
            this.cmbViewType.Size = new System.Drawing.Size(100, 25);
            this.cmbViewType.TabIndex = 5;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.White;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Location = new System.Drawing.Point(20, 60);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(35, 24);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.White;
            this.btnNext.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Location = new System.Drawing.Point(400, 60);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(35, 24);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblCurrentMonth
            // 
            this.lblCurrentMonth.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold);
            this.lblCurrentMonth.Location = new System.Drawing.Point(65, 58);
            this.lblCurrentMonth.Name = "lblCurrentMonth";
            this.lblCurrentMonth.Size = new System.Drawing.Size(330, 28);
            this.lblCurrentMonth.TabIndex = 3;
            this.lblCurrentMonth.Text = "2026년 05월";
            this.lblCurrentMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnToday
            // 
            this.btnToday.BackColor = System.Drawing.Color.White;
            this.btnToday.FlatAppearance.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToday.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnToday.Location = new System.Drawing.Point(445, 60);
            this.btnToday.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(60, 24);
            this.btnToday.TabIndex = 1;
            this.btnToday.Text = "오늘";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnCalendarMode
            // 
            this.btnCalendarMode.BackColor = System.Drawing.Color.White;
            this.btnCalendarMode.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCalendarMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendarMode.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCalendarMode.Location = new System.Drawing.Point(516, 60);
            this.btnCalendarMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCalendarMode.Name = "btnCalendarMode";
            this.btnCalendarMode.Size = new System.Drawing.Size(86, 24);
            this.btnCalendarMode.TabIndex = 9;
            this.btnCalendarMode.Text = "팀 캘린더";
            this.btnCalendarMode.UseVisualStyleBackColor = false;
            this.btnCalendarMode.Click += new System.EventHandler(this.btnCalendarMode_Click);
            // 
            // pnlCalendar
            // 
            this.pnlCalendar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlCalendar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCalendar.Location = new System.Drawing.Point(20, 96);
            this.pnlCalendar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCalendar.Name = "pnlCalendar";
            this.pnlCalendar.Size = new System.Drawing.Size(930, 528);
            this.pnlCalendar.TabIndex = 0;
            // 
            // tlpTeamLegend
            // 
            this.tlpTeamLegend.AutoScroll = true;
            this.tlpTeamLegend.BackColor = System.Drawing.Color.White;
            this.tlpTeamLegend.ColumnCount = 3;
            this.tlpTeamLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpTeamLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpTeamLegend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpTeamLegend.Location = new System.Drawing.Point(20, 558);
            this.tlpTeamLegend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlpTeamLegend.Name = "tlpTeamLegend";
            this.tlpTeamLegend.RowCount = 1;
            this.tlpTeamLegend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlpTeamLegend.Size = new System.Drawing.Size(930, 62);
            this.tlpTeamLegend.TabIndex = 10;
            this.tlpTeamLegend.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(970, 640);
            this.Controls.Add(this.tlpTeamLegend);
            this.Controls.Add(this.pnlCalendar);
            this.Controls.Add(this.btnCalendarMode);
            this.Controls.Add(this.btnToday);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblCurrentMonth);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.cmbViewType);
            this.Controls.Add(this.cmbTeams);
            this.Controls.Add(this.btnAddAvailability);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AI 팀 캘린더 메인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
