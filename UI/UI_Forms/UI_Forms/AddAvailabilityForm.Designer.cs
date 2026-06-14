namespace UI_Forms
{
    partial class AddAvailabilityForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.FlowLayoutPanel flpTimeSlots;
        private System.Windows.Forms.Button btnAddRow;
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
            this.flpTimeSlots = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeader.Location = new System.Drawing.Point(23, 20);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(198, 32);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "가능 시간대 제출";
            // 
            // flpTimeSlots
            // 
            this.flpTimeSlots.AutoScroll = true;
            this.flpTimeSlots.BackColor = System.Drawing.Color.White;
            this.flpTimeSlots.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTimeSlots.Location = new System.Drawing.Point(23, 82);
            this.flpTimeSlots.Name = "flpTimeSlots";
            this.flpTimeSlots.Size = new System.Drawing.Size(526, 338);
            this.flpTimeSlots.TabIndex = 1;
            this.flpTimeSlots.WrapContents = false;
            // 
            // btnAddRow
            // 
            this.btnAddRow.BackColor = System.Drawing.Color.White;
            this.btnAddRow.FlatAppearance.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.btnAddRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRow.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddRow.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.btnAddRow.Location = new System.Drawing.Point(23, 435);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(149, 40);
            this.btnAddRow.TabIndex = 2;
            this.btnAddRow.Text = "+ 시간대 추가";
            this.btnAddRow.UseVisualStyleBackColor = false;
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(309, 435);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 40);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LightGray;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(434, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 40);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(29, 51);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(198, 25);
            this.txtTitle.TabIndex = 5;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // AddAvailabilityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(571, 500);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAddRow);
            this.Controls.Add(this.flpTimeSlots);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddAvailabilityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "가용 시간대 입력";
            this.Load += new System.EventHandler(this.AddAvailabilityForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtTitle;
    }
}