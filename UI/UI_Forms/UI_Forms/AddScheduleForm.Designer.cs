namespace UI_Forms
{
    partial class AddScheduleForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStartDate;

        // 🌟 분할된 시간/분 콤보박스
        private System.Windows.Forms.ComboBox cmbStartHour;
        private System.Windows.Forms.Label lblStartColon;
        private System.Windows.Forms.ComboBox cmbStartMinute;

        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEndDate;

        // 🌟 분할된 시간/분 콤보박스
        private System.Windows.Forms.ComboBox cmbEndHour;
        private System.Windows.Forms.Label lblEndColon;
        private System.Windows.Forms.ComboBox cmbEndMinute;

        private System.Windows.Forms.FlowLayoutPanel flpColors;
        private System.Windows.Forms.Label lblSelectedColor;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.cmbStartHour = new System.Windows.Forms.ComboBox();
            this.lblStartColon = new System.Windows.Forms.Label();
            this.cmbStartMinute = new System.Windows.Forms.ComboBox();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.cmbEndHour = new System.Windows.Forms.ComboBox();
            this.lblEndColon = new System.Windows.Forms.Label();
            this.cmbEndMinute = new System.Windows.Forms.ComboBox();
            this.flpColors = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSelectedColor = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(20, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(126, 25);
            this.lblHeader.Text = "일정 추가";
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtTitle.Location = new System.Drawing.Point(25, 60);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(285, 27);
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(22, 108);
            this.lblStart.Size = new System.Drawing.Size(31, 15);
            this.lblStart.Text = "시작";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(60, 105);
            this.dtpStartDate.Size = new System.Drawing.Size(125, 23);
            // 
            // cmbStartHour
            // 
            this.cmbStartHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartHour.Location = new System.Drawing.Point(195, 105);
            this.cmbStartHour.Size = new System.Drawing.Size(48, 23);
            // 
            // lblStartColon
            // 
            this.lblStartColon.AutoSize = true;
            this.lblStartColon.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblStartColon.Location = new System.Drawing.Point(245, 107);
            this.lblStartColon.Size = new System.Drawing.Size(12, 19);
            this.lblStartColon.Text = ":";
            // 
            // cmbStartMinute
            // 
            this.cmbStartMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartMinute.Location = new System.Drawing.Point(260, 105);
            this.cmbStartMinute.Size = new System.Drawing.Size(48, 23);
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(22, 148);
            this.lblEnd.Size = new System.Drawing.Size(31, 15);
            this.lblEnd.Text = "종료";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(60, 145);
            this.dtpEndDate.Size = new System.Drawing.Size(125, 23);
            // 
            // cmbEndHour
            // 
            this.cmbEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndHour.Location = new System.Drawing.Point(195, 145);
            this.cmbEndHour.Size = new System.Drawing.Size(48, 23);
            // 
            // lblEndColon
            // 
            this.lblEndColon.AutoSize = true;
            this.lblEndColon.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblEndColon.Location = new System.Drawing.Point(245, 147);
            this.lblEndColon.Size = new System.Drawing.Size(12, 19);
            this.lblEndColon.Text = ":";
            // 
            // cmbEndMinute
            // 
            this.cmbEndMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndMinute.Location = new System.Drawing.Point(260, 145);
            this.cmbEndMinute.Size = new System.Drawing.Size(48, 23);
            // 
            // flpColors
            // 
            this.flpColors.Location = new System.Drawing.Point(25, 185);
            this.flpColors.Size = new System.Drawing.Size(240, 40);
            // 
            // lblSelectedColor
            // 
            this.lblSelectedColor.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblSelectedColor.Location = new System.Drawing.Point(280, 190);
            this.lblSelectedColor.Size = new System.Drawing.Size(30, 30);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(145, 245);
            this.btnSave.Size = new System.Drawing.Size(75, 35);
            this.btnSave.Text = "저장";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LightGray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(235, 245);
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.Text = "취소";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddScheduleForm
            // 
            this.ClientSize = new System.Drawing.Size(340, 305);
            this.Controls.Add(this.lblSelectedColor);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.flpColors);
            this.Controls.Add(this.cmbEndMinute);
            this.Controls.Add(this.lblEndColon);
            this.Controls.Add(this.cmbEndHour);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.cmbStartMinute);
            this.Controls.Add(this.lblStartColon);
            this.Controls.Add(this.cmbStartHour);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddScheduleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "일정 추가";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}