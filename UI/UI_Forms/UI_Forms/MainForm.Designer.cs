namespace UI_Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAddSchedule;
        private System.Windows.Forms.ComboBox cmbTeams;
        private System.Windows.Forms.ComboBox cmbViewType;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblCurrentMonth;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Panel pnlCalendar;

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
            this.btnAddSchedule = new System.Windows.Forms.Button();
            this.cmbTeams = new System.Windows.Forms.ComboBox();
            this.cmbViewType = new System.Windows.Forms.ComboBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblCurrentMonth = new System.Windows.Forms.Label();
            this.btnToday = new System.Windows.Forms.Button();
            this.pnlCalendar = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(197, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "사용자님의 캘린더";
            // 
            // btnAddSchedule
            // 
            this.btnAddSchedule.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnAddSchedule.FlatAppearance.BorderSize = 0;
            this.btnAddSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSchedule.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddSchedule.ForeColor = System.Drawing.Color.White;
            this.btnAddSchedule.Location = new System.Drawing.Point(840, 16);
            this.btnAddSchedule.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddSchedule.Name = "btnAddSchedule";
            this.btnAddSchedule.Size = new System.Drawing.Size(110, 28);
            this.btnAddSchedule.TabIndex = 1;
            this.btnAddSchedule.Text = "+ 일정 추가";
            this.btnAddSchedule.UseVisualStyleBackColor = false;
            this.btnAddSchedule.Click += new System.EventHandler(this.btnAddSchedule_Click);
            // 
            // cmbTeams
            // 
            this.cmbTeams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeams.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbTeams.FormattingEnabled = true;
            this.cmbTeams.Location = new System.Drawing.Point(580, 20);
            this.cmbTeams.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTeams.Name = "cmbTeams";
            this.cmbTeams.Size = new System.Drawing.Size(140, 25);
            this.cmbTeams.TabIndex = 2;
            // 
            // cmbViewType
            // 
            this.cmbViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViewType.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbViewType.FormattingEnabled = true;
            this.cmbViewType.Location = new System.Drawing.Point(730, 20);
            this.cmbViewType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbViewType.Name = "cmbViewType";
            this.cmbViewType.Size = new System.Drawing.Size(100, 25);
            this.cmbViewType.TabIndex = 3;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.White;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
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
            this.btnNext.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnNext.Location = new System.Drawing.Point(344, 60);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(35, 24);
            this.btnNext.TabIndex = 6;
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
            this.lblCurrentMonth.TabIndex = 5;
            this.lblCurrentMonth.Text = "2026년 05월";
            this.lblCurrentMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnToday
            // 
            this.btnToday.BackColor = System.Drawing.Color.White;
            this.btnToday.FlatAppearance.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToday.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnToday.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnToday.Location = new System.Drawing.Point(389, 60);
            this.btnToday.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(60, 24);
            this.btnToday.TabIndex = 7;
            this.btnToday.Text = "오늘";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // pnlCalendar
            // 
            this.pnlCalendar.BackColor = System.Drawing.Color.LightGray;
            this.pnlCalendar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCalendar.Location = new System.Drawing.Point(20, 96);
            this.pnlCalendar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCalendar.Name = "pnlCalendar";
            this.pnlCalendar.Size = new System.Drawing.Size(924, 399);
            this.pnlCalendar.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(970, 512);
            this.Controls.Add(this.pnlCalendar);
            this.Controls.Add(this.btnToday);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblCurrentMonth);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.cmbViewType);
            this.Controls.Add(this.cmbTeams);
            this.Controls.Add(this.btnAddSchedule);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AI 캘린더 메인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}