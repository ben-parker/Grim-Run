namespace GrimRun
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.InjectButton = new System.Windows.Forms.Button();
            this.totalDamageLabel = new System.Windows.Forms.Label();
            this.totalDamageDisplay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(86, 53);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(654, 20);
            this.textBox1.TabIndex = 0;
            // 
            // InjectButton
            // 
            this.InjectButton.Location = new System.Drawing.Point(572, 393);
            this.InjectButton.Name = "InjectButton";
            this.InjectButton.Size = new System.Drawing.Size(168, 45);
            this.InjectButton.TabIndex = 1;
            this.InjectButton.Text = "Inject";
            this.InjectButton.UseVisualStyleBackColor = true;
            this.InjectButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // totalDamageLabel
            // 
            this.totalDamageLabel.AutoSize = true;
            this.totalDamageLabel.Location = new System.Drawing.Point(86, 173);
            this.totalDamageLabel.Name = "totalDamageLabel";
            this.totalDamageLabel.Size = new System.Drawing.Size(74, 13);
            this.totalDamageLabel.TabIndex = 2;
            this.totalDamageLabel.Text = "Total Damage";
            // 
            // totalDamageDisplay
            // 
            this.totalDamageDisplay.AutoSize = true;
            this.totalDamageDisplay.Location = new System.Drawing.Point(166, 173);
            this.totalDamageDisplay.Name = "totalDamageDisplay";
            this.totalDamageDisplay.Size = new System.Drawing.Size(13, 13);
            this.totalDamageDisplay.TabIndex = 3;
            this.totalDamageDisplay.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.totalDamageDisplay);
            this.Controls.Add(this.totalDamageLabel);
            this.Controls.Add(this.InjectButton);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button InjectButton;
        private System.Windows.Forms.Label totalDamageLabel;
        private System.Windows.Forms.Label totalDamageDisplay;
    }
}

