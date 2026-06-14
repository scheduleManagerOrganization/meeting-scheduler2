namespace UI_Forms
{
    partial class AddScheduleForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.ComboBox cmbStartHour;
        private System.Windows.Forms.Label lblStartColon;
        private System.Windows.Forms.ComboBox cmbStartMinute;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.ComboBox cmbEndHour;
        private System.Windows.Forms.Label lblEndColon;
        private System.Windows.Forms.ComboBox cmbEndMinute;

        // 🌟 추가된 반복 관련 컨트롤
        private System.Windows.Forms.CheckBox chkRecurrence;
        private System.Windows.Forms.Panel pnlRecurrence;
        private System.Windows.Forms.ComboBox cmbRecurCount;
        private System.Windows.Forms.Label lblRecurEvery;
        private System.Windows.Forms.ComboBox cmbRecurType;
        private System.Windows.Forms.Label lblRecurEnd;
        private System.Windows.Forms.DateTimePicker dtpRecurEnd;

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
            this.chkRecurrence = new System.Windows.Forms.CheckBox();
            this.pnlRecurrence = new System.Windows.Forms.Panel();
            this.cmbRecurCount = new System.Windows.Forms.ComboBox();
            this.cmbRecurType = new System.Windows.Forms.ComboBox();
            this.lblRecurEvery = new System.Windows.Forms.Label();
            this.lblRecurEnd = new System.Windows.Forms.Label();
            this.dtpRecurEnd = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlRecurrence.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(20, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(95, 25);
            this.lblHeader.TabIndex = 25;
            this.lblHeader.Text = "일정 추가";
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtTitle.Location = new System.Drawing.Point(25, 60);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(285, 27);
            this.txtTitle.TabIndex = 24;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(22, 108);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(29, 12);
            this.lblStart.TabIndex = 23;
            this.lblStart.Text = "시작";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(60, 105);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(125, 21);
            this.dtpStartDate.TabIndex = 22;
            // 
            // cmbStartHour
            // 
            this.cmbStartHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartHour.Location = new System.Drawing.Point(195, 105);
            this.cmbStartHour.Name = "cmbStartHour";
            this.cmbStartHour.Size = new System.Drawing.Size(48, 20);
            this.cmbStartHour.TabIndex = 21;
            // 
            // lblStartColon
            // 
            this.lblStartColon.AutoSize = true;
            this.lblStartColon.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblStartColon.Location = new System.Drawing.Point(245, 107);
            this.lblStartColon.Name = "lblStartColon";
            this.lblStartColon.Size = new System.Drawing.Size(13, 19);
            this.lblStartColon.TabIndex = 20;
            this.lblStartColon.Text = ":";
            // 
            // cmbStartMinute
            // 
            this.cmbStartMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartMinute.Location = new System.Drawing.Point(260, 105);
            this.cmbStartMinute.Name = "cmbStartMinute";
            this.cmbStartMinute.Size = new System.Drawing.Size(48, 20);
            this.cmbStartMinute.TabIndex = 19;
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(22, 148);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(29, 12);
            this.lblEnd.TabIndex = 18;
            this.lblEnd.Text = "종료";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(60, 145);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(125, 21);
            this.dtpEndDate.TabIndex = 17;
            // 
            // cmbEndHour
            // 
            this.cmbEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndHour.Location = new System.Drawing.Point(195, 145);
            this.cmbEndHour.Name = "cmbEndHour";
            this.cmbEndHour.Size = new System.Drawing.Size(48, 20);
            this.cmbEndHour.TabIndex = 16;
            // 
            // lblEndColon
            // 
            this.lblEndColon.AutoSize = true;
            this.lblEndColon.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblEndColon.Location = new System.Drawing.Point(245, 147);
            this.lblEndColon.Name = "lblEndColon";
            this.lblEndColon.Size = new System.Drawing.Size(13, 19);
            this.lblEndColon.TabIndex = 15;
            this.lblEndColon.Text = ":";
            // 
            // cmbEndMinute
            // 
            this.cmbEndMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndMinute.Location = new System.Drawing.Point(260, 145);
            this.cmbEndMinute.Name = "cmbEndMinute";
            this.cmbEndMinute.Size = new System.Drawing.Size(48, 20);
            this.cmbEndMinute.TabIndex = 14;
            // 
            // chkRecurrence
            // 
            this.chkRecurrence.AutoSize = true;
            this.chkRecurrence.Location = new System.Drawing.Point(25, 185);
            this.chkRecurrence.Name = "chkRecurrence";
            this.chkRecurrence.Size = new System.Drawing.Size(76, 16);
            this.chkRecurrence.TabIndex = 10;
            this.chkRecurrence.Text = "반복 일정";
            // 
            // pnlRecurrence
            // 
            this.pnlRecurrence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecurrence.Controls.Add(this.cmbRecurCount);
            this.pnlRecurrence.Controls.Add(this.cmbRecurType);
            this.pnlRecurrence.Controls.Add(this.lblRecurEvery);
            this.pnlRecurrence.Controls.Add(this.lblRecurEnd);
            this.pnlRecurrence.Controls.Add(this.dtpRecurEnd);
            this.pnlRecurrence.Location = new System.Drawing.Point(25, 210);
            this.pnlRecurrence.Name = "pnlRecurrence";
            this.pnlRecurrence.Size = new System.Drawing.Size(285, 65);
            this.pnlRecurrence.TabIndex = 11;
            this.pnlRecurrence.Visible = false;
            // 
            // cmbRecurCount
            // 
            this.cmbRecurCount.FormattingEnabled = true;
            this.cmbRecurCount.Location = new System.Drawing.Point(10, 8);
            this.cmbRecurCount.Name = "cmbRecurCount";
            this.cmbRecurCount.Size = new System.Drawing.Size(45, 20);
            this.cmbRecurCount.TabIndex = 0;
            // 
            // cmbRecurType
            // 
            this.cmbRecurType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRecurType.FormattingEnabled = true;
            this.cmbRecurType.Location = new System.Drawing.Point(60, 8);
            this.cmbRecurType.Name = "cmbRecurType";
            this.cmbRecurType.Size = new System.Drawing.Size(45, 20);
            this.cmbRecurType.TabIndex = 1;
            // 
            // lblRecurEvery
            // 
            this.lblRecurEvery.AutoSize = true;
            this.lblRecurEvery.Location = new System.Drawing.Point(110, 11);
            this.lblRecurEvery.Name = "lblRecurEvery";
            this.lblRecurEvery.Size = new System.Drawing.Size(29, 12);
            this.lblRecurEvery.TabIndex = 2;
            this.lblRecurEvery.Text = "마다";
            // 
            // lblRecurEnd
            // 
            this.lblRecurEnd.AutoSize = true;
            this.lblRecurEnd.Location = new System.Drawing.Point(8, 38);
            this.lblRecurEnd.Name = "lblRecurEnd";
            this.lblRecurEnd.Size = new System.Drawing.Size(41, 12);
            this.lblRecurEnd.TabIndex = 3;
            this.lblRecurEnd.Text = "종료일";
            // 
            // dtpRecurEnd
            // 
            this.dtpRecurEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRecurEnd.Location = new System.Drawing.Point(60, 34);
            this.dtpRecurEnd.Name = "dtpRecurEnd";
            this.dtpRecurEnd.Size = new System.Drawing.Size(115, 21);
            this.dtpRecurEnd.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(145, 190);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 35);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LightGray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(235, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddScheduleForm
            // 
            this.ClientSize = new System.Drawing.Size(340, 240);
            this.Controls.Add(this.pnlRecurrence);
            this.Controls.Add(this.chkRecurrence);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
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
            this.pnlRecurrence.ResumeLayout(false);
            this.pnlRecurrence.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}