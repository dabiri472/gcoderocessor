namespace GCodeProcessor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SelectFileBtn = new System.Windows.Forms.Button();
            this.ProcessBtn = new System.Windows.Forms.Button();
            this.InputFilePathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OffsetTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CountTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // SelectFileBtn
            this.SelectFileBtn.Location = new System.Drawing.Point(12, 12);
            this.SelectFileBtn.Name = "SelectFileBtn";
            this.SelectFileBtn.Size = new System.Drawing.Size(120, 30);
            this.SelectFileBtn.TabIndex = 0;
            this.SelectFileBtn.Text = "انتخاب فایل";
            this.SelectFileBtn.UseVisualStyleBackColor = true;
            this.SelectFileBtn.Click += new System.EventHandler(this.SelectFileBtn_Click);

            // InputFilePathTextBox
            this.InputFilePathTextBox.Location = new System.Drawing.Point(140, 15);
            this.InputFilePathTextBox.Name = "InputFilePathTextBox";
            this.InputFilePathTextBox.ReadOnly = true;
            this.InputFilePathTextBox.Size = new System.Drawing.Size(380, 24);
            this.InputFilePathTextBox.TabIndex = 1;

            // label1 - Offset
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Offset (mm):";

            // OffsetTextBox
            this.OffsetTextBox.Location = new System.Drawing.Point(100, 50);
            this.OffsetTextBox.Name = "OffsetTextBox";
            this.OffsetTextBox.Size = new System.Drawing.Size(100, 24);
            this.OffsetTextBox.TabIndex = 3;
            this.OffsetTextBox.Text = "55.0";

            // label2 - Count
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "تعداد بلوک:";

            // CountTextBox
            this.CountTextBox.Location = new System.Drawing.Point(300, 50);
            this.CountTextBox.Name = "CountTextBox";
            this.CountTextBox.Size = new System.Drawing.Size(80, 24);
            this.CountTextBox.TabIndex = 5;
            this.CountTextBox.Text = "20";

            // label3 - Mode
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "جهت:";

            // ModeComboBox
            this.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeComboBox.Items.AddRange(new object[] { "x", "y", "both" });
            this.ModeComboBox.Location = new System.Drawing.Point(60, 92);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(120, 24);
            this.ModeComboBox.SelectedIndex = 0;
            this.ModeComboBox.TabIndex = 7;

            // ProcessBtn
            this.ProcessBtn.Location = new System.Drawing.Point(400, 85);
            this.ProcessBtn.Name = "ProcessBtn";
            this.ProcessBtn.Size = new System.Drawing.Size(120, 35);
            this.ProcessBtn.TabIndex = 8;
            this.ProcessBtn.Text = "شروع پردازش";
            this.ProcessBtn.UseVisualStyleBackColor = true;
            this.ProcessBtn.Click += new System.EventHandler(this.ProcessBtn_Click);

            // statusLabel
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 140);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 16);
            this.statusLabel.TabIndex = 9;

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 170);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.ProcessBtn);
            this.Controls.Add(this.ModeComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CountTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OffsetTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputFilePathTextBox);
            this.Controls.Add(this.SelectFileBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "G-Code Processor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button SelectFileBtn;
        private System.Windows.Forms.TextBox InputFilePathTextBox;
        private System.Windows.Forms.Button ProcessBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox OffsetTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CountTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ModeComboBox;
        private System.Windows.Forms.Label statusLabel;
    }
}