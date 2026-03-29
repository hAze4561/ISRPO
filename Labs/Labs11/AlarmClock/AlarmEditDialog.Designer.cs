namespace AlarmClock
{
    partial class AlarmEditDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.chkRepeatDaily = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(30, 29);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(299, 22);
            this.dateTimePicker1.TabIndex = 0;
            this.dateTimePicker1.Value = new System.DateTime(2026, 3, 29, 10, 2, 0, 0);
            // 
            // txtLabel
            // 
            this.txtLabel.Location = new System.Drawing.Point(23, 153);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(331, 22);
            this.txtLabel.TabIndex = 1;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(29, 67);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(157, 20);
            this.chkActive.TabIndex = 2;
            this.chkActive.Text = "Будильник активен";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // chkRepeatDaily
            // 
            this.chkRepeatDaily.AutoSize = true;
            this.chkRepeatDaily.Location = new System.Drawing.Point(30, 105);
            this.chkRepeatDaily.Name = "chkRepeatDaily";
            this.chkRepeatDaily.Size = new System.Drawing.Size(175, 20);
            this.chkRepeatDaily.TabIndex = 3;
            this.chkRepeatDaily.Text = "Повторять ежедневно";
            this.chkRepeatDaily.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(46, 181);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(99, 29);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "ОК";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(179, 181);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 29);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // AlarmEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 213);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkRepeatDaily);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.txtLabel);
            this.Controls.Add(this.dateTimePicker1);
            this.Name = "AlarmEditDialog";
            this.Text = "Настройка бульника";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.CheckBox chkRepeatDaily;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}