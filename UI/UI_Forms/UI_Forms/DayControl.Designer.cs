namespace UI_Forms
{
    partial class DayControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.FlowLayoutPanel flpSchedules;

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
            this.lblDay = new System.Windows.Forms.Label();
            this.flpSchedules = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDay.Location = new System.Drawing.Point(5, 5);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(22, 17);
            this.lblDay.TabIndex = 0;
            this.lblDay.Text = "00";
            // 
            // flpSchedules
            // 
            this.flpSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSchedules.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSchedules.Location = new System.Drawing.Point(0, 25);
            this.flpSchedules.Name = "flpSchedules";
            this.flpSchedules.Size = new System.Drawing.Size(130, 55);
            this.flpSchedules.TabIndex = 1;
            this.flpSchedules.WrapContents = false;
            // 
            // DayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.flpSchedules);
            this.Controls.Add(this.lblDay);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DayControl";
            this.Size = new System.Drawing.Size(130, 80);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}