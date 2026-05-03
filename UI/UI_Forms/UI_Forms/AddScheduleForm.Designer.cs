namespace UI_Forms
{
    partial class AddScheduleForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.ComboBox cmbStartHour;
        private System.Windows.Forms.ComboBox cmbStartMinute;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.ComboBox cmbEndHour;
        private System.Windows.Forms.ComboBox cmbEndMinute;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.Label lblColon2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.cmbStartHour = new System.Windows.Forms.ComboBox();
            this.cmbStartMinute = new System.Windows.Forms.ComboBox();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.cmbEndHour = new System.Windows.Forms.ComboBox();
            this.cmbEndMinute = new System.Windows.Forms.ComboBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.btnColor = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.lblColon2 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(67, 15);
            this.lblTitle.Text = "일정 제목:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(100, 27);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(240, 25);

            // lblStart
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(30, 80);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(67, 15);
            this.lblStart.Text = "시작 시간:";

            // dtpStart (날짜 전용)
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(100, 75);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(120, 25);

            // cmbStartHour (시)
            this.cmbStartHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartHour.FormattingEnabled = true;
            this.cmbStartHour.Location = new System.Drawing.Point(230, 75);
            this.cmbStartHour.Name = "cmbStartHour";
            this.cmbStartHour.Size = new System.Drawing.Size(45, 23);

            // lblColon1 (:)
            this.lblColon1.AutoSize = true;
            this.lblColon1.Location = new System.Drawing.Point(277, 78);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(10, 15);
            this.lblColon1.Text = ":";

            // cmbStartMinute (분)
            this.cmbStartMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartMinute.FormattingEnabled = true;
            this.cmbStartMinute.Location = new System.Drawing.Point(290, 75);
            this.cmbStartMinute.Name = "cmbStartMinute";
            this.cmbStartMinute.Size = new System.Drawing.Size(45, 23);

            // lblEnd
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(30, 130);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(67, 15);
            this.lblEnd.Text = "종료 시간:";

            // dtpEnd (날짜 전용)
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(100, 125);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(120, 25);

            // cmbEndHour (시)
            this.cmbEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndHour.FormattingEnabled = true;
            this.cmbEndHour.Location = new System.Drawing.Point(230, 125);
            this.cmbEndHour.Name = "cmbEndHour";
            this.cmbEndHour.Size = new System.Drawing.Size(45, 23);

            // lblColon2 (:)
            this.lblColon2.AutoSize = true;
            this.lblColon2.Location = new System.Drawing.Point(277, 128);
            this.lblColon2.Name = "lblColon2";
            this.lblColon2.Size = new System.Drawing.Size(10, 15);
            this.lblColon2.Text = ":";

            // cmbEndMinute (분)
            this.cmbEndMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndMinute.FormattingEnabled = true;
            this.cmbEndMinute.Location = new System.Drawing.Point(290, 125);
            this.cmbEndMinute.Name = "cmbEndMinute";
            this.cmbEndMinute.Size = new System.Drawing.Size(45, 23);

            // lblColor
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(30, 180);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(67, 15);
            this.lblColor.Text = "일정 색상:";

            // btnColor
            this.btnColor.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor.Location = new System.Drawing.Point(100, 175);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(60, 25);
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(100, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(220, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // AddScheduleForm
            this.ClientSize = new System.Drawing.Size(380, 290);
            this.Controls.Add(this.cmbEndMinute);
            this.Controls.Add(this.lblColon2);
            this.Controls.Add(this.cmbEndHour);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.cmbStartMinute);
            this.Controls.Add(this.lblColon1);
            this.Controls.Add(this.cmbStartHour);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Name = "AddScheduleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "새 일정 추가";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}