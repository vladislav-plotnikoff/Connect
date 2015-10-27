namespace Connect
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.stepsLabel = new System.Windows.Forms.Label();
            this.timerLabel = new System.Windows.Forms.Label();
            this.fieldPictureBox = new System.Windows.Forms.PictureBox();
            this.menuButton = new System.Windows.Forms.Button();
            this.menuPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.fieldPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // stepsLabel
            // 
            this.stepsLabel.BackColor = System.Drawing.Color.Transparent;
            this.stepsLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.stepsLabel.ForeColor = System.Drawing.Color.White;
            this.stepsLabel.Location = new System.Drawing.Point(0, 567);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Size = new System.Drawing.Size(75, 30);
            this.stepsLabel.TabIndex = 2;
            this.stepsLabel.Text = "045";
            this.stepsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerLabel
            // 
            this.timerLabel.BackColor = System.Drawing.Color.Transparent;
            this.timerLabel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.timerLabel.ForeColor = System.Drawing.Color.White;
            this.timerLabel.Location = new System.Drawing.Point(492, 568);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(75, 30);
            this.timerLabel.TabIndex = 3;
            this.timerLabel.Text = "032";
            this.timerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fieldPictureBox
            // 
            this.fieldPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.fieldPictureBox.Location = new System.Drawing.Point(0, 0);
            this.fieldPictureBox.Name = "fieldPictureBox";
            this.fieldPictureBox.Size = new System.Drawing.Size(567, 567);
            this.fieldPictureBox.TabIndex = 0;
            this.fieldPictureBox.TabStop = false;
            this.fieldPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.fieldPictureBox.MouseLeave += new System.EventHandler(this.fieldPictureBox_MouseLeave);
            this.fieldPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fieldPictureBox_MouseMove);
            // 
            // menuButton
            // 
            this.menuButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menuButton.FlatAppearance.BorderSize = 0;
            this.menuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuButton.Image = global::Connect.Properties.Resources.Menu;
            this.menuButton.Location = new System.Drawing.Point(75, 567);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(417, 30);
            this.menuButton.TabIndex = 1;
            this.menuButton.TabStop = false;
            this.menuButton.UseVisualStyleBackColor = false;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            // 
            // menuPictureBox
            // 
            this.menuPictureBox.Location = new System.Drawing.Point(0, 0);
            this.menuPictureBox.Name = "menuPictureBox";
            this.menuPictureBox.Size = new System.Drawing.Size(100, 50);
            this.menuPictureBox.TabIndex = 4;
            this.menuPictureBox.TabStop = false;
            this.menuPictureBox.Visible = false;
            this.menuPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuPictureBox_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(583, 598);
            this.Controls.Add(this.menuPictureBox);
            this.Controls.Add(this.fieldPictureBox);
            this.Controls.Add(this.stepsLabel);
            this.Controls.Add(this.menuButton);
            this.Controls.Add(this.timerLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.fieldPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox fieldPictureBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.Label stepsLabel;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.PictureBox menuPictureBox;
    }
}

