namespace UI_Forms
{
    partial class DayControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.FlowLayoutPanel flpSchedules; // 추가됨

        private void InitializeComponent()
        {
            this.lblDay = new System.Windows.Forms.Label();
            this.flpSchedules = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();

            // lblDay
            this.lblDay.AutoSize = true;
            this.lblDay.Location = new System.Drawing.Point(5, 5);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(14, 15);
            this.lblDay.Text = "0";

            // flpSchedules (일정들이 쌓일 공간)
            this.flpSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSchedules.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSchedules.Location = new System.Drawing.Point(2, 25); // 라벨 아래부터 시작
            this.flpSchedules.Name = "flpSchedules";
            this.flpSchedules.Size = new System.Drawing.Size(96, 53);

            // DayControl
            this.Controls.Add(this.flpSchedules);
            this.Controls.Add(this.lblDay);
            this.Name = "DayControl";
            this.Size = new System.Drawing.Size(100, 80);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}