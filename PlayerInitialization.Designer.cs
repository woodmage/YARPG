namespace YARPG
{
    partial class PlayerInitialization
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerInitialization));
            label1 = new Label();
            textName = new TextBox();
            label2 = new Label();
            textRace = new TextBox();
            comboBaseRace = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            comboAlignment = new ComboBox();
            label5 = new Label();
            numericUpDownSTR = new NumericUpDown();
            label6 = new Label();
            numericUpDownDEX = new NumericUpDown();
            label7 = new Label();
            numericUpDownCON = new NumericUpDown();
            label8 = new Label();
            numericUpDownINT = new NumericUpDown();
            label9 = new Label();
            numericUpDownWIS = new NumericUpDown();
            label10 = new Label();
            numericUpDownCHA = new NumericUpDown();
            label11 = new Label();
            textBonus = new TextBox();
            buttonReRoll = new Button();
            label12 = new Label();
            textGold = new TextBox();
            label13 = new Label();
            textAC = new TextBox();
            label14 = new Label();
            textHP = new TextBox();
            label19 = new Label();
            textDarkVision = new TextBox();
            label20 = new Label();
            comboClass = new ComboBox();
            buttonShop = new Button();
            buttonCancel = new Button();
            buttonDone = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSTR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDEX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCON).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownINT).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownWIS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCHA).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // textName
            // 
            textName.Location = new Point(57, 7);
            textName.Name = "textName";
            textName.Size = new Size(100, 23);
            textName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(163, 9);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 2;
            label2.Text = "BaseRace";
            // 
            // textRace
            // 
            textRace.Location = new Point(390, 6);
            textRace.Name = "textRace";
            textRace.Size = new Size(100, 23);
            textRace.TabIndex = 3;
            // 
            // comboBaseRace
            // 
            comboBaseRace.FormattingEnabled = true;
            comboBaseRace.Location = new Point(225, 6);
            comboBaseRace.Name = "comboBaseRace";
            comboBaseRace.Size = new Size(121, 23);
            comboBaseRace.TabIndex = 4;
            comboBaseRace.SelectedIndexChanged += BaseRaceChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(352, 9);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 5;
            label3.Text = "Race";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(496, 10);
            label4.Name = "label4";
            label4.Size = new Size(63, 15);
            label4.TabIndex = 6;
            label4.Text = "Alignment";
            // 
            // comboAlignment
            // 
            comboAlignment.FormattingEnabled = true;
            comboAlignment.Location = new Point(565, 7);
            comboAlignment.Name = "comboAlignment";
            comboAlignment.Size = new Size(121, 23);
            comboAlignment.TabIndex = 7;
            comboAlignment.SelectedIndexChanged += AlignmentChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 48);
            label5.Name = "label5";
            label5.Size = new Size(26, 15);
            label5.TabIndex = 8;
            label5.Text = "STR";
            // 
            // numericUpDownSTR
            // 
            numericUpDownSTR.Location = new Point(44, 46);
            numericUpDownSTR.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            numericUpDownSTR.Name = "numericUpDownSTR";
            numericUpDownSTR.Size = new Size(60, 23);
            numericUpDownSTR.TabIndex = 9;
            numericUpDownSTR.ValueChanged += StrChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(110, 48);
            label6.Name = "label6";
            label6.Size = new Size(28, 15);
            label6.TabIndex = 8;
            label6.Text = "DEX";
            // 
            // numericUpDownDEX
            // 
            numericUpDownDEX.Location = new Point(144, 46);
            numericUpDownDEX.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            numericUpDownDEX.Name = "numericUpDownDEX";
            numericUpDownDEX.Size = new Size(60, 23);
            numericUpDownDEX.TabIndex = 9;
            numericUpDownDEX.ValueChanged += DexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(210, 48);
            label7.Name = "label7";
            label7.Size = new Size(33, 15);
            label7.TabIndex = 8;
            label7.Text = "CON";
            // 
            // numericUpDownCON
            // 
            numericUpDownCON.Location = new Point(249, 46);
            numericUpDownCON.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            numericUpDownCON.Name = "numericUpDownCON";
            numericUpDownCON.Size = new Size(60, 23);
            numericUpDownCON.TabIndex = 9;
            numericUpDownCON.ValueChanged += ConChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(315, 48);
            label8.Name = "label8";
            label8.Size = new Size(25, 15);
            label8.TabIndex = 8;
            label8.Text = "INT";
            // 
            // numericUpDownINT
            // 
            numericUpDownINT.Location = new Point(346, 46);
            numericUpDownINT.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            numericUpDownINT.Name = "numericUpDownINT";
            numericUpDownINT.Size = new Size(60, 23);
            numericUpDownINT.TabIndex = 9;
            numericUpDownINT.ValueChanged += IntChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(412, 48);
            label9.Name = "label9";
            label9.Size = new Size(27, 15);
            label9.TabIndex = 8;
            label9.Text = "WIS";
            // 
            // numericUpDownWIS
            // 
            numericUpDownWIS.Location = new Point(445, 46);
            numericUpDownWIS.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            numericUpDownWIS.Name = "numericUpDownWIS";
            numericUpDownWIS.Size = new Size(60, 23);
            numericUpDownWIS.TabIndex = 9;
            numericUpDownWIS.ValueChanged += WisChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(511, 48);
            label10.Name = "label10";
            label10.Size = new Size(32, 15);
            label10.TabIndex = 8;
            label10.Text = "CHA";
            // 
            // numericUpDownCHA
            // 
            numericUpDownCHA.Location = new Point(549, 46);
            numericUpDownCHA.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            numericUpDownCHA.Name = "numericUpDownCHA";
            numericUpDownCHA.Size = new Size(60, 23);
            numericUpDownCHA.TabIndex = 9;
            numericUpDownCHA.ValueChanged += ChaChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(615, 48);
            label11.Name = "label11";
            label11.Size = new Size(40, 15);
            label11.TabIndex = 10;
            label11.Text = "Bonus";
            // 
            // textBonus
            // 
            textBonus.Location = new Point(661, 45);
            textBonus.Name = "textBonus";
            textBonus.Size = new Size(35, 23);
            textBonus.TabIndex = 11;
            // 
            // buttonReRoll
            // 
            buttonReRoll.BackColor = Color.Cyan;
            buttonReRoll.FlatStyle = FlatStyle.Flat;
            buttonReRoll.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonReRoll.Location = new Point(12, 81);
            buttonReRoll.Name = "buttonReRoll";
            buttonReRoll.Size = new Size(75, 30);
            buttonReRoll.TabIndex = 12;
            buttonReRoll.Text = "ReRoll";
            buttonReRoll.UseVisualStyleBackColor = false;
            buttonReRoll.Click += ReRollStats;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(511, 85);
            label12.Name = "label12";
            label12.Size = new Size(32, 15);
            label12.TabIndex = 13;
            label12.Text = "Gold";
            // 
            // textGold
            // 
            textGold.Location = new Point(549, 82);
            textGold.Name = "textGold";
            textGold.Size = new Size(70, 23);
            textGold.TabIndex = 14;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(260, 85);
            label13.Name = "label13";
            label13.Size = new Size(23, 15);
            label13.TabIndex = 15;
            label13.Text = "AC";
            // 
            // textAC
            // 
            textAC.Location = new Point(289, 82);
            textAC.Name = "textAC";
            textAC.Size = new Size(34, 23);
            textAC.TabIndex = 16;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(329, 85);
            label14.Name = "label14";
            label14.Size = new Size(23, 15);
            label14.TabIndex = 17;
            label14.Text = "HP";
            // 
            // textHP
            // 
            textHP.Location = new Point(358, 82);
            textHP.Name = "textHP";
            textHP.Size = new Size(30, 23);
            textHP.TabIndex = 18;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(394, 85);
            label19.Name = "label19";
            label19.Size = new Size(63, 15);
            label19.TabIndex = 17;
            label19.Text = "DarkVision";
            // 
            // textDarkVision
            // 
            textDarkVision.Location = new Point(463, 82);
            textDarkVision.Name = "textDarkVision";
            textDarkVision.Size = new Size(42, 23);
            textDarkVision.TabIndex = 18;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(93, 85);
            label20.Name = "label20";
            label20.Size = new Size(34, 15);
            label20.TabIndex = 19;
            label20.Text = "Class";
            // 
            // comboClass
            // 
            comboClass.FormattingEnabled = true;
            comboClass.Location = new Point(133, 82);
            comboClass.Name = "comboClass";
            comboClass.Size = new Size(121, 23);
            comboClass.TabIndex = 7;
            comboClass.SelectedIndexChanged += ClassChanged;
            // 
            // buttonShop
            // 
            buttonShop.BackColor = Color.Yellow;
            buttonShop.FlatStyle = FlatStyle.Flat;
            buttonShop.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonShop.Location = new Point(625, 82);
            buttonShop.Name = "buttonShop";
            buttonShop.Size = new Size(75, 29);
            buttonShop.TabIndex = 12;
            buttonShop.Text = "Shop";
            buttonShop.UseVisualStyleBackColor = false;
            buttonShop.Click += Shop;
            // 
            // buttonCancel
            // 
            buttonCancel.BackColor = Color.LightCoral;
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            buttonCancel.Location = new Point(179, 164);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 28);
            buttonCancel.TabIndex = 20;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = false;
            buttonCancel.Click += Cancel;
            // 
            // buttonDone
            // 
            buttonDone.BackColor = Color.LawnGreen;
            buttonDone.FlatStyle = FlatStyle.Flat;
            buttonDone.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            buttonDone.Location = new Point(440, 164);
            buttonDone.Name = "buttonDone";
            buttonDone.Size = new Size(75, 28);
            buttonDone.TabIndex = 21;
            buttonDone.Text = "Done";
            buttonDone.UseVisualStyleBackColor = false;
            buttonDone.Click += Done;
            // 
            // PlayerInitialization
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(708, 199);
            Controls.Add(buttonDone);
            Controls.Add(buttonCancel);
            Controls.Add(label20);
            Controls.Add(textDarkVision);
            Controls.Add(label19);
            Controls.Add(textHP);
            Controls.Add(label14);
            Controls.Add(textAC);
            Controls.Add(label13);
            Controls.Add(textGold);
            Controls.Add(label12);
            Controls.Add(buttonShop);
            Controls.Add(buttonReRoll);
            Controls.Add(textBonus);
            Controls.Add(label11);
            Controls.Add(numericUpDownCHA);
            Controls.Add(label10);
            Controls.Add(numericUpDownWIS);
            Controls.Add(label9);
            Controls.Add(numericUpDownINT);
            Controls.Add(label8);
            Controls.Add(numericUpDownCON);
            Controls.Add(label7);
            Controls.Add(numericUpDownDEX);
            Controls.Add(label6);
            Controls.Add(numericUpDownSTR);
            Controls.Add(label5);
            Controls.Add(comboClass);
            Controls.Add(comboAlignment);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(comboBaseRace);
            Controls.Add(textRace);
            Controls.Add(label2);
            Controls.Add(textName);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PlayerInitialization";
            Text = "Player Initialization";
            Load += PILoad;
            ((System.ComponentModel.ISupportInitialize)numericUpDownSTR).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDEX).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCON).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownINT).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownWIS).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCHA).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textName;
        private Label label2;
        private TextBox textRace;
        private ComboBox comboBaseRace;
        private Label label3;
        private Label label4;
        private ComboBox comboAlignment;
        private Label label5;
        private NumericUpDown numericUpDownSTR;
        private Label label6;
        private NumericUpDown numericUpDownDEX;
        private Label label7;
        private NumericUpDown numericUpDownCON;
        private Label label8;
        private NumericUpDown numericUpDownINT;
        private Label label9;
        private NumericUpDown numericUpDownWIS;
        private Label label10;
        private NumericUpDown numericUpDownCHA;
        private Label label11;
        private TextBox textBonus;
        private Button buttonReRoll;
        private Label label12;
        private TextBox textGold;
        private Label label13;
        private TextBox textAC;
        private Label label14;
        private TextBox textHP;
        private Label label19;
        private TextBox textDarkVision;
        private Label label20;
        private ComboBox comboClass;
        private Button buttonShop;
        private Button buttonCancel;
        private Button buttonDone;
    }
}