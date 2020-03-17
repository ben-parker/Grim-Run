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
            this.physDmgLabel = new System.Windows.Forms.Label();
            this.physDmg = new System.Windows.Forms.Label();
            this.piercingDmg = new System.Windows.Forms.Label();
            this.piercingDmgLabel = new System.Windows.Forms.Label();
            this.fireDmg = new System.Windows.Forms.Label();
            this.fireDmgLabel = new System.Windows.Forms.Label();
            this.coldDmg = new System.Windows.Forms.Label();
            this.coldDmgLabel = new System.Windows.Forms.Label();
            this.vitalityDmg = new System.Windows.Forms.Label();
            this.vitalityDmgLabel = new System.Windows.Forms.Label();
            this.acidDmg = new System.Windows.Forms.Label();
            this.acidDmgLabel = new System.Windows.Forms.Label();
            this.aetherDmg = new System.Windows.Forms.Label();
            this.aetherDmgLabel = new System.Windows.Forms.Label();
            this.chaosDmg = new System.Windows.Forms.Label();
            this.chaosDmgLabel = new System.Windows.Forms.Label();
            this.lightningDmg = new System.Windows.Forms.Label();
            this.lightningDmgLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.bleedingDmg = new System.Windows.Forms.Label();
            this.bleedingDmgLabel = new System.Windows.Forms.Label();
            this.percentLifeDmg = new System.Windows.Forms.Label();
            this.percentLifeDmgLabel = new System.Windows.Forms.Label();
            this.dmgToPlayerLog = new System.Windows.Forms.TextBox();
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
            this.totalDamageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalDamageLabel.Location = new System.Drawing.Point(12, 108);
            this.totalDamageLabel.Name = "totalDamageLabel";
            this.totalDamageLabel.Size = new System.Drawing.Size(184, 31);
            this.totalDamageLabel.TabIndex = 2;
            this.totalDamageLabel.Text = "Total Damage";
            // 
            // totalDamageDisplay
            // 
            this.totalDamageDisplay.AutoSize = true;
            this.totalDamageDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalDamageDisplay.Location = new System.Drawing.Point(223, 108);
            this.totalDamageDisplay.Name = "totalDamageDisplay";
            this.totalDamageDisplay.Size = new System.Drawing.Size(29, 31);
            this.totalDamageDisplay.TabIndex = 3;
            this.totalDamageDisplay.Text = "0";
            // 
            // physDmgLabel
            // 
            this.physDmgLabel.AutoSize = true;
            this.physDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.physDmgLabel.Location = new System.Drawing.Point(18, 165);
            this.physDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.physDmgLabel.Name = "physDmgLabel";
            this.physDmgLabel.Size = new System.Drawing.Size(66, 20);
            this.physDmgLabel.TabIndex = 4;
            this.physDmgLabel.Text = "Physical";
            // 
            // physDmg
            // 
            this.physDmg.AutoSize = true;
            this.physDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.physDmg.Location = new System.Drawing.Point(90, 165);
            this.physDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.physDmg.Name = "physDmg";
            this.physDmg.Size = new System.Drawing.Size(18, 20);
            this.physDmg.TabIndex = 5;
            this.physDmg.Text = "0";
            // 
            // piercingDmg
            // 
            this.piercingDmg.AutoSize = true;
            this.piercingDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.piercingDmg.Location = new System.Drawing.Point(90, 195);
            this.piercingDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.piercingDmg.Name = "piercingDmg";
            this.piercingDmg.Size = new System.Drawing.Size(18, 20);
            this.piercingDmg.TabIndex = 7;
            this.piercingDmg.Text = "0";
            // 
            // piercingDmgLabel
            // 
            this.piercingDmgLabel.AutoSize = true;
            this.piercingDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.piercingDmgLabel.Location = new System.Drawing.Point(18, 195);
            this.piercingDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.piercingDmgLabel.Name = "piercingDmgLabel";
            this.piercingDmgLabel.Size = new System.Drawing.Size(65, 20);
            this.piercingDmgLabel.TabIndex = 6;
            this.piercingDmgLabel.Text = "Piercing";
            // 
            // fireDmg
            // 
            this.fireDmg.AutoSize = true;
            this.fireDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fireDmg.Location = new System.Drawing.Point(89, 253);
            this.fireDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.fireDmg.Name = "fireDmg";
            this.fireDmg.Size = new System.Drawing.Size(18, 20);
            this.fireDmg.TabIndex = 9;
            this.fireDmg.Text = "0";
            // 
            // fireDmgLabel
            // 
            this.fireDmgLabel.AutoSize = true;
            this.fireDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fireDmgLabel.Location = new System.Drawing.Point(18, 254);
            this.fireDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.fireDmgLabel.Name = "fireDmgLabel";
            this.fireDmgLabel.Size = new System.Drawing.Size(36, 20);
            this.fireDmgLabel.TabIndex = 8;
            this.fireDmgLabel.Text = "Fire";
            // 
            // coldDmg
            // 
            this.coldDmg.AutoSize = true;
            this.coldDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coldDmg.Location = new System.Drawing.Point(89, 283);
            this.coldDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.coldDmg.Name = "coldDmg";
            this.coldDmg.Size = new System.Drawing.Size(18, 20);
            this.coldDmg.TabIndex = 11;
            this.coldDmg.Text = "0";
            // 
            // coldDmgLabel
            // 
            this.coldDmgLabel.AutoSize = true;
            this.coldDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coldDmgLabel.Location = new System.Drawing.Point(18, 284);
            this.coldDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.coldDmgLabel.Name = "coldDmgLabel";
            this.coldDmgLabel.Size = new System.Drawing.Size(41, 20);
            this.coldDmgLabel.TabIndex = 10;
            this.coldDmgLabel.Text = "Cold";
            // 
            // vitalityDmg
            // 
            this.vitalityDmg.AutoSize = true;
            this.vitalityDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vitalityDmg.Location = new System.Drawing.Point(268, 193);
            this.vitalityDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.vitalityDmg.Name = "vitalityDmg";
            this.vitalityDmg.Size = new System.Drawing.Size(18, 20);
            this.vitalityDmg.TabIndex = 13;
            this.vitalityDmg.Text = "0";
            // 
            // vitalityDmgLabel
            // 
            this.vitalityDmgLabel.AutoSize = true;
            this.vitalityDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vitalityDmgLabel.Location = new System.Drawing.Point(197, 194);
            this.vitalityDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.vitalityDmgLabel.Name = "vitalityDmgLabel";
            this.vitalityDmgLabel.Size = new System.Drawing.Size(55, 20);
            this.vitalityDmgLabel.TabIndex = 12;
            this.vitalityDmgLabel.Text = "Vitality";
            // 
            // acidDmg
            // 
            this.acidDmg.AutoSize = true;
            this.acidDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acidDmg.Location = new System.Drawing.Point(268, 163);
            this.acidDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.acidDmg.Name = "acidDmg";
            this.acidDmg.Size = new System.Drawing.Size(18, 20);
            this.acidDmg.TabIndex = 15;
            this.acidDmg.Text = "0";
            // 
            // acidDmgLabel
            // 
            this.acidDmgLabel.AutoSize = true;
            this.acidDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acidDmgLabel.Location = new System.Drawing.Point(197, 164);
            this.acidDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.acidDmgLabel.Name = "acidDmgLabel";
            this.acidDmgLabel.Size = new System.Drawing.Size(40, 20);
            this.acidDmgLabel.TabIndex = 14;
            this.acidDmgLabel.Text = "Acid";
            // 
            // aetherDmg
            // 
            this.aetherDmg.AutoSize = true;
            this.aetherDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aetherDmg.Location = new System.Drawing.Point(269, 224);
            this.aetherDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.aetherDmg.Name = "aetherDmg";
            this.aetherDmg.Size = new System.Drawing.Size(18, 20);
            this.aetherDmg.TabIndex = 17;
            this.aetherDmg.Text = "0";
            // 
            // aetherDmgLabel
            // 
            this.aetherDmgLabel.AutoSize = true;
            this.aetherDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aetherDmgLabel.Location = new System.Drawing.Point(197, 224);
            this.aetherDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.aetherDmgLabel.Name = "aetherDmgLabel";
            this.aetherDmgLabel.Size = new System.Drawing.Size(57, 20);
            this.aetherDmgLabel.TabIndex = 16;
            this.aetherDmgLabel.Text = "Aether";
            // 
            // chaosDmg
            // 
            this.chaosDmg.AutoSize = true;
            this.chaosDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chaosDmg.Location = new System.Drawing.Point(269, 254);
            this.chaosDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.chaosDmg.Name = "chaosDmg";
            this.chaosDmg.Size = new System.Drawing.Size(18, 20);
            this.chaosDmg.TabIndex = 19;
            this.chaosDmg.Text = "0";
            // 
            // chaosDmgLabel
            // 
            this.chaosDmgLabel.AutoSize = true;
            this.chaosDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chaosDmgLabel.Location = new System.Drawing.Point(197, 254);
            this.chaosDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.chaosDmgLabel.Name = "chaosDmgLabel";
            this.chaosDmgLabel.Size = new System.Drawing.Size(55, 20);
            this.chaosDmgLabel.TabIndex = 18;
            this.chaosDmgLabel.Text = "Chaos";
            // 
            // lightningDmg
            // 
            this.lightningDmg.AutoSize = true;
            this.lightningDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lightningDmg.Location = new System.Drawing.Point(89, 313);
            this.lightningDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lightningDmg.Name = "lightningDmg";
            this.lightningDmg.Size = new System.Drawing.Size(18, 20);
            this.lightningDmg.TabIndex = 21;
            this.lightningDmg.Text = "0";
            // 
            // lightningDmgLabel
            // 
            this.lightningDmgLabel.AutoSize = true;
            this.lightningDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lightningDmgLabel.Location = new System.Drawing.Point(17, 313);
            this.lightningDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lightningDmgLabel.Name = "lightningDmgLabel";
            this.lightningDmgLabel.Size = new System.Drawing.Size(74, 20);
            this.lightningDmgLabel.TabIndex = 20;
            this.lightningDmgLabel.Text = "Lightning";
            // 
            // resetButton
            // 
            this.resetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetButton.Location = new System.Drawing.Point(506, 153);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(233, 121);
            this.resetButton.TabIndex = 22;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // bleedingDmg
            // 
            this.bleedingDmg.AutoSize = true;
            this.bleedingDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bleedingDmg.Location = new System.Drawing.Point(89, 223);
            this.bleedingDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.bleedingDmg.Name = "bleedingDmg";
            this.bleedingDmg.Size = new System.Drawing.Size(18, 20);
            this.bleedingDmg.TabIndex = 24;
            this.bleedingDmg.Text = "0";
            // 
            // bleedingDmgLabel
            // 
            this.bleedingDmgLabel.AutoSize = true;
            this.bleedingDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bleedingDmgLabel.Location = new System.Drawing.Point(18, 224);
            this.bleedingDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.bleedingDmgLabel.Name = "bleedingDmgLabel";
            this.bleedingDmgLabel.Size = new System.Drawing.Size(71, 20);
            this.bleedingDmgLabel.TabIndex = 23;
            this.bleedingDmgLabel.Text = "Bleeding";
            // 
            // percentLifeDmg
            // 
            this.percentLifeDmg.AutoSize = true;
            this.percentLifeDmg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percentLifeDmg.Location = new System.Drawing.Point(269, 284);
            this.percentLifeDmg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.percentLifeDmg.Name = "percentLifeDmg";
            this.percentLifeDmg.Size = new System.Drawing.Size(18, 20);
            this.percentLifeDmg.TabIndex = 26;
            this.percentLifeDmg.Text = "0";
            // 
            // percentLifeDmgLabel
            // 
            this.percentLifeDmgLabel.AutoSize = true;
            this.percentLifeDmgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percentLifeDmgLabel.Location = new System.Drawing.Point(197, 284);
            this.percentLifeDmgLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.percentLifeDmgLabel.Name = "percentLifeDmgLabel";
            this.percentLifeDmgLabel.Size = new System.Drawing.Size(53, 20);
            this.percentLifeDmgLabel.TabIndex = 25;
            this.percentLifeDmgLabel.Text = "% Life";
            // 
            // dmgToPlayerLog
            // 
            this.dmgToPlayerLog.Location = new System.Drawing.Point(21, 393);
            this.dmgToPlayerLog.Multiline = true;
            this.dmgToPlayerLog.Name = "dmgToPlayerLog";
            this.dmgToPlayerLog.Size = new System.Drawing.Size(545, 309);
            this.dmgToPlayerLog.TabIndex = 27;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 753);
            this.Controls.Add(this.dmgToPlayerLog);
            this.Controls.Add(this.percentLifeDmg);
            this.Controls.Add(this.percentLifeDmgLabel);
            this.Controls.Add(this.bleedingDmg);
            this.Controls.Add(this.bleedingDmgLabel);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.lightningDmg);
            this.Controls.Add(this.lightningDmgLabel);
            this.Controls.Add(this.chaosDmg);
            this.Controls.Add(this.chaosDmgLabel);
            this.Controls.Add(this.aetherDmg);
            this.Controls.Add(this.aetherDmgLabel);
            this.Controls.Add(this.acidDmg);
            this.Controls.Add(this.acidDmgLabel);
            this.Controls.Add(this.vitalityDmg);
            this.Controls.Add(this.vitalityDmgLabel);
            this.Controls.Add(this.coldDmg);
            this.Controls.Add(this.coldDmgLabel);
            this.Controls.Add(this.fireDmg);
            this.Controls.Add(this.fireDmgLabel);
            this.Controls.Add(this.piercingDmg);
            this.Controls.Add(this.piercingDmgLabel);
            this.Controls.Add(this.physDmg);
            this.Controls.Add(this.physDmgLabel);
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
        private System.Windows.Forms.Label physDmgLabel;
        private System.Windows.Forms.Label physDmg;
        private System.Windows.Forms.Label piercingDmg;
        private System.Windows.Forms.Label piercingDmgLabel;
        private System.Windows.Forms.Label fireDmg;
        private System.Windows.Forms.Label fireDmgLabel;
        private System.Windows.Forms.Label coldDmg;
        private System.Windows.Forms.Label coldDmgLabel;
        private System.Windows.Forms.Label vitalityDmg;
        private System.Windows.Forms.Label vitalityDmgLabel;
        private System.Windows.Forms.Label acidDmg;
        private System.Windows.Forms.Label acidDmgLabel;
        private System.Windows.Forms.Label aetherDmg;
        private System.Windows.Forms.Label aetherDmgLabel;
        private System.Windows.Forms.Label chaosDmg;
        private System.Windows.Forms.Label chaosDmgLabel;
        private System.Windows.Forms.Label lightningDmg;
        private System.Windows.Forms.Label lightningDmgLabel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label bleedingDmg;
        private System.Windows.Forms.Label bleedingDmgLabel;
        private System.Windows.Forms.Label percentLifeDmg;
        private System.Windows.Forms.Label percentLifeDmgLabel;
        private System.Windows.Forms.TextBox dmgToPlayerLog;
    }
}

