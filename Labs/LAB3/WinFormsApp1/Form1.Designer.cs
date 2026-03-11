namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            button1 = new Button();
            label2 = new Label();
            label3 = new Label();
            lblResult = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(205, 25);
            label1.Name = "label1";
            label1.Size = new Size(385, 22);
            label1.TabIndex = 0;
            label1.Text = "Генератор и проверка счастливых билетов";
            // 
            // button1
            // 
            button1.Location = new Point(285, 76);
            button1.Name = "button1";
            button1.Size = new Size(208, 46);
            button1.TabIndex = 1;
            button1.Text = "Сгенерировать билет";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 12F, FontStyle.Italic, GraphicsUnit.Point, 204);
            label2.Location = new Point(276, 142);
            label2.Name = "label2";
            label2.Size = new Size(227, 19);
            label2.TabIndex = 2;
            label2.Text = "Нажмите кнопку для генерации";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 9F, FontStyle.Italic, GraphicsUnit.Point, 204);
            label3.Location = new Point(137, 397);
            label3.Name = "label3";
            label3.Size = new Size(542, 15);
            label3.TabIndex = 3;
            label3.Text = "Счастливый билет - это билет, у которого сумма первых трёх цифр равна сумме последних трех цифр";
            // 
            // lblResult
            // 
            lblResult.AutoSize = true;
            lblResult.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblResult.Location = new Point(328, 221);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(0, 19);
            lblResult.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblResult);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Label label2;
        private Label label3;
        private Label lblResult;
    }
}
