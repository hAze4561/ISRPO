namespace ПР4
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtVvod = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSdvig = new System.Windows.Forms.TextBox();
            this.btnShivr = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRashivr = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Исходный текст:";
            // 
            // txtVvod
            // 
            this.txtVvod.Location = new System.Drawing.Point(118, 50);
            this.txtVvod.Name = "txtVvod";
            this.txtVvod.Size = new System.Drawing.Size(633, 20);
            this.txtVvod.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Результат:";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(100, 96);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 13);
            this.lblResult.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Язык:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(65, 210);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(142, 21);
            this.comboBox1.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(222, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Сдвиг:";
            // 
            // txtSdvig
            // 
            this.txtSdvig.Location = new System.Drawing.Point(272, 211);
            this.txtSdvig.Name = "txtSdvig";
            this.txtSdvig.Size = new System.Drawing.Size(98, 20);
            this.txtSdvig.TabIndex = 7;
            // 
            // btnShivr
            // 
            this.btnShivr.Location = new System.Drawing.Point(402, 211);
            this.btnShivr.Name = "btnShivr";
            this.btnShivr.Size = new System.Drawing.Size(103, 20);
            this.btnShivr.TabIndex = 8;
            this.btnShivr.Text = "Зашифровать";
            this.btnShivr.UseVisualStyleBackColor = true;
            this.btnShivr.Click += new System.EventHandler(this.btnShivr_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(641, 211);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(103, 20);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Очистить";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRashivr
            // 
            this.btnRashivr.Location = new System.Drawing.Point(520, 211);
            this.btnRashivr.Name = "btnRashivr";
            this.btnRashivr.Size = new System.Drawing.Size(103, 20);
            this.btnRashivr.TabIndex = 9;
            this.btnRashivr.Text = "Расшифровать";
            this.btnRashivr.UseVisualStyleBackColor = true;
            this.btnRashivr.Click += new System.EventHandler(this.btnRashivr_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 295);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRashivr);
            this.Controls.Add(this.btnShivr);
            this.Controls.Add(this.txtSdvig);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtVvod);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVvod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSdvig;
        private System.Windows.Forms.Button btnShivr;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRashivr;
    }
}

