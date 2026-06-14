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
        private System.Windows.Forms.FlowLayoutPanel flpMeetingSidebar;

        private System.Windows.Forms.Button btnCreateTeam;
        private System.Windows.Forms.Button btnJoinTeam;

        // 🌟 새롭게 추가되는 컨트롤들
        private System.Windows.Forms.Button btnAddMeeting;
        private System.Windows.Forms.Panel pnlMeetingLegend;
        private System.Windows.Forms.Label lblLegendMeetingTitle;
        private System.Windows.Forms.Label lblLegendAttending;
        private System.Windows.Forms.Label lblLegendNotAttending;
        private System.Windows.Forms.Label lblLegendNotResponded;

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
            this.btnCreateTeam = new System.Windows.Forms.Button();
            this.btnJoinTeam = new System.Windows.Forms.Button();
            this.btnAddMeeting = new System.Windows.Forms.Button();
            this.flpMeetingSidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.flpMeetingSidebar.AutoScroll = true;
            this.flpMeetingSidebar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flpMeetingSidebar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpMeetingSidebar.Location = new System.Drawing.Point(970, 96);
            this.flpMeetingSidebar.Name = "flpMeetingSidebar";
            this.flpMeetingSidebar.Size = new System.Drawing.Size(280, 528);
            this.flpMeetingSidebar.TabIndex = 20;
            this.flpMeetingSidebar.Visible = false;
            this.Controls.Add(this.flpMeetingSidebar);

            // 미팅 범례 초기화
            this.pnlMeetingLegend = new System.Windows.Forms.Panel();
            this.lblLegendMeetingTitle = new System.Windows.Forms.Label();
            this.lblLegendAttending = new System.Windows.Forms.Label();
            this.lblLegendNotAttending = new System.Windows.Forms.Label();
            this.lblLegendNotResponded = new System.Windows.Forms.Label();

            this.pnlMeetingLegend.SuspendLayout();
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
            this.btnAddAvailability.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnAddAvailability.ForeColor = System.Drawing.Color.White;
            this.btnAddAvailability.Location = new System.Drawing.Point(848, 16);
            this.btnAddAvailability.Name = "btnAddAvailability";
            this.btnAddAvailability.Size = new System.Drawing.Size(102, 28);
            this.btnAddAvailability.TabIndex = 7;
            this.btnAddAvailability.Text = "+ 일정 추가";
            this.btnAddAvailability.UseVisualStyleBackColor = false;
            this.btnAddAvailability.Click += new System.EventHandler(this.btnAddAvailability_Click);

            // 
            // cmbTeams
            // 
            this.cmbTeams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeams.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbTeams.Location = new System.Drawing.Point(590, 16);
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
            this.cmbViewType.Location = new System.Drawing.Point(736, 16);
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
            this.btnNext.Location = new System.Drawing.Point(381, 60);
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
            this.btnToday.Location = new System.Drawing.Point(426, 60);
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
            this.btnCalendarMode.Location = new System.Drawing.Point(497, 60);
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
            this.tlpTeamLegend.Name = "tlpTeamLegend";
            this.tlpTeamLegend.RowCount = 1;
            this.tlpTeamLegend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlpTeamLegend.Size = new System.Drawing.Size(730, 62);
            this.tlpTeamLegend.TabIndex = 10;
            this.tlpTeamLegend.Visible = false;

            // 
            // pnlMeetingLegend (미팅 범례)
            // 
            this.pnlMeetingLegend.BackColor = System.Drawing.Color.White;
            this.pnlMeetingLegend.Controls.Add(this.lblLegendMeetingTitle);
            this.pnlMeetingLegend.Controls.Add(this.lblLegendAttending);
            this.pnlMeetingLegend.Controls.Add(this.lblLegendNotAttending);
            this.pnlMeetingLegend.Controls.Add(this.lblLegendNotResponded);
            this.pnlMeetingLegend.Location = new System.Drawing.Point(760, 558);
            this.pnlMeetingLegend.Name = "pnlMeetingLegend";
            this.pnlMeetingLegend.Size = new System.Drawing.Size(190, 62);
            this.pnlMeetingLegend.TabIndex = 14;
            this.pnlMeetingLegend.Visible = false;

            // 
            // lblLegendMeetingTitle
            // 
            this.lblLegendMeetingTitle.AutoSize = true;
            this.lblLegendMeetingTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblLegendMeetingTitle.ForeColor = System.Drawing.Color.Crimson;
            this.lblLegendMeetingTitle.Location = new System.Drawing.Point(6, 6);
            this.lblLegendMeetingTitle.Name = "lblLegendMeetingTitle";
            this.lblLegendMeetingTitle.Size = new System.Drawing.Size(95, 15);
            this.lblLegendMeetingTitle.Text = "■ 팀 미팅 일정";

            // 
            // lblLegendAttending
            // 
            this.lblLegendAttending.AutoSize = true;
            this.lblLegendAttending.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblLegendAttending.ForeColor = System.Drawing.Color.DimGray;
            this.lblLegendAttending.Location = new System.Drawing.Point(6, 26);
            this.lblLegendAttending.Name = "lblLegendAttending";
            this.lblLegendAttending.Size = new System.Drawing.Size(56, 15);
            this.lblLegendAttending.Text = "✓ : 참석";

            // 
            // lblLegendNotAttending
            // 
            this.lblLegendNotAttending.AutoSize = true;
            this.lblLegendNotAttending.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblLegendNotAttending.ForeColor = System.Drawing.Color.DimGray;
            this.lblLegendNotAttending.Location = new System.Drawing.Point(66, 26);
            this.lblLegendNotAttending.Name = "lblLegendNotAttending";
            this.lblLegendNotAttending.Size = new System.Drawing.Size(51, 15);
            this.lblLegendNotAttending.Text = "X : 불참";

            // 
            // lblLegendNotResponded
            // 
            this.lblLegendNotResponded.AutoSize = true;
            this.lblLegendNotResponded.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.lblLegendNotResponded.ForeColor = System.Drawing.Color.DimGray;
            this.lblLegendNotResponded.Location = new System.Drawing.Point(120, 26);
            this.lblLegendNotResponded.Name = "lblLegendNotResponded";
            this.lblLegendNotResponded.Size = new System.Drawing.Size(63, 15);
            this.lblLegendNotResponded.Text = "! : 미정";

            // 
            // btnAddMeeting (미팅 추가)
            // 
            this.btnAddMeeting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMeeting.BackColor = System.Drawing.Color.Crimson;
            this.btnAddMeeting.FlatAppearance.BorderSize = 0;
            this.btnAddMeeting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMeeting.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnAddMeeting.ForeColor = System.Drawing.Color.White;
            this.btnAddMeeting.Location = new System.Drawing.Point(848, 58);
            this.btnAddMeeting.Name = "btnAddMeeting";
            this.btnAddMeeting.Size = new System.Drawing.Size(102, 28);
            this.btnAddMeeting.TabIndex = 13;
            this.btnAddMeeting.Text = "미팅 추가";
            this.btnAddMeeting.UseVisualStyleBackColor = false;
            this.btnAddMeeting.Visible = false;
            this.btnAddMeeting.Click += new System.EventHandler(this.btnAddMeeting_Click);

            // 
            // btnCreateTeam
            // 
            this.btnCreateTeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateTeam.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCreateTeam.FlatAppearance.BorderSize = 0;
            this.btnCreateTeam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateTeam.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnCreateTeam.ForeColor = System.Drawing.Color.White;
            this.btnCreateTeam.Location = new System.Drawing.Point(734, 58); // 기존 848에서 좌측으로 이동
            this.btnCreateTeam.Name = "btnCreateTeam";
            this.btnCreateTeam.Size = new System.Drawing.Size(102, 28);
            this.btnCreateTeam.TabIndex = 11;
            this.btnCreateTeam.Text = "팀 생성";
            this.btnCreateTeam.UseVisualStyleBackColor = false;
            this.btnCreateTeam.Visible = false;
            this.btnCreateTeam.Click += new System.EventHandler(this.btnCreateTeam_Click);

            // 
            // btnJoinTeam
            // 
            this.btnJoinTeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJoinTeam.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnJoinTeam.FlatAppearance.BorderSize = 0;
            this.btnJoinTeam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoinTeam.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnJoinTeam.ForeColor = System.Drawing.Color.White;
            this.btnJoinTeam.Location = new System.Drawing.Point(620, 58); // 기존 734에서 좌측으로 이동
            this.btnJoinTeam.Name = "btnJoinTeam";
            this.btnJoinTeam.Size = new System.Drawing.Size(102, 28);
            this.btnJoinTeam.TabIndex = 12;
            this.btnJoinTeam.Text = "팀 참가";
            this.btnJoinTeam.UseVisualStyleBackColor = false;
            this.btnJoinTeam.Visible = false;
            this.btnJoinTeam.Click += new System.EventHandler(this.btnJoinTeam_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(970, 640);
            this.Controls.Add(this.btnAddMeeting);
            this.Controls.Add(this.pnlMeetingLegend);
            this.Controls.Add(this.btnJoinTeam);
            this.Controls.Add(this.btnCreateTeam);
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
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AI 팀 캘린더 메인";
            this.pnlMeetingLegend.ResumeLayout(false);
            this.pnlMeetingLegend.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
