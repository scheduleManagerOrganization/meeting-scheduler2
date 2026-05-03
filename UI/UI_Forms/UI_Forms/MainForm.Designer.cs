namespace UI_Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TableLayoutPanel calendarPanel;
        private System.Windows.Forms.Button btnAddSchedule;
        private System.Windows.Forms.Panel navPanel; // 상단 바 Panel 추가
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblCurrentMonth;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnWeekView;
        private System.Windows.Forms.Button btnMonthView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.calendarPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddSchedule = new System.Windows.Forms.Button();
            this.navPanel = new System.Windows.Forms.Panel();
            this.btnWeekView = new System.Windows.Forms.Button();
            this.btnMonthView = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblCurrentMonth = new System.Windows.Forms.Label();
            this.navPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWelcome.Location = new System.Drawing.Point(20, 15);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(112, 28);
            this.lblWelcome.TabIndex = 3;
            this.lblWelcome.Text = "환영합니다";
            // 
            // calendarPanel
            // 
            this.calendarPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calendarPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.calendarPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.calendarPanel.Location = new System.Drawing.Point(20, 120);
            this.calendarPanel.Name = "calendarPanel";
            this.calendarPanel.Size = new System.Drawing.Size(760, 460);
            this.calendarPanel.TabIndex = 1;
            // 
            // btnAddSchedule
            // 
            this.btnAddSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSchedule.Location = new System.Drawing.Point(660, 10);
            this.btnAddSchedule.Name = "btnAddSchedule";
            this.btnAddSchedule.Size = new System.Drawing.Size(120, 35);
            this.btnAddSchedule.TabIndex = 2;
            this.btnAddSchedule.Text = "+ 일정 추가";
            this.btnAddSchedule.UseVisualStyleBackColor = true;
            this.btnAddSchedule.Click += new System.EventHandler(this.btnAddSchedule_Click);
            // 
            // navPanel
            // 
            this.navPanel.Controls.Add(this.btnWeekView);
            this.navPanel.Controls.Add(this.btnMonthView);
            this.navPanel.Controls.Add(this.btnToday);
            this.navPanel.Controls.Add(this.btnNext);
            this.navPanel.Controls.Add(this.btnPrev);
            this.navPanel.Controls.Add(this.lblCurrentMonth);
            this.navPanel.Location = new System.Drawing.Point(20, 60);
            this.navPanel.Name = "navPanel";
            this.navPanel.Size = new System.Drawing.Size(760, 50);
            this.navPanel.TabIndex = 0;
            // 
            // btnWeekView
            // 
            this.btnWeekView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWeekView.Location = new System.Drawing.Point(685, 10);
            this.btnWeekView.Name = "btnWeekView";
            this.btnWeekView.Size = new System.Drawing.Size(70, 30);
            this.btnWeekView.TabIndex = 0;
            this.btnWeekView.Text = "주간";
            this.btnWeekView.Click += new System.EventHandler(this.btnWeekView_Click);
            // 
            // btnMonthView
            // 
            this.btnMonthView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMonthView.Location = new System.Drawing.Point(610, 10);
            this.btnMonthView.Name = "btnMonthView";
            this.btnMonthView.Size = new System.Drawing.Size(70, 30);
            this.btnMonthView.TabIndex = 1;
            this.btnMonthView.Text = "월간";
            this.btnMonthView.Click += new System.EventHandler(this.btnMonthView_Click);
            // 
            // btnToday
            // 
            this.btnToday.Location = new System.Drawing.Point(280, 10);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(60, 30);
            this.btnToday.TabIndex = 2;
            this.btnToday.Text = "오늘";
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(230, 10);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 30);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = ">";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(5, 10);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(40, 30);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.Text = "<";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblCurrentMonth
            // 
            this.lblCurrentMonth.AutoSize = true;
            this.lblCurrentMonth.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.lblCurrentMonth.Location = new System.Drawing.Point(55, 8);
            this.lblCurrentMonth.Name = "lblCurrentMonth";
            this.lblCurrentMonth.Size = new System.Drawing.Size(176, 37);
            this.lblCurrentMonth.TabIndex = 5;
            this.lblCurrentMonth.Text = "2026년 04월";
            this.lblCurrentMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.navPanel);
            this.Controls.Add(this.calendarPanel);
            this.Controls.Add(this.btnAddSchedule);
            this.Controls.Add(this.lblWelcome);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "팀 프로젝트 캘린더 (UserControl & View Swapping)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.navPanel.ResumeLayout(false);
            this.navPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}