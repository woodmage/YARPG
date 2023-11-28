namespace YARPG
{
    partial class Settings
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
            label1 = new Label();
            soundeffectsCB = new CheckBox();
            label2 = new Label();
            verbosityNUD = new NumericUpDown();
            label3 = new Label();
            monsterstatusCB = new CheckBox();
            playerstatusCB = new CheckBox();
            label4 = new Label();
            minimapdrawmmCB = new CheckBox();
            donotupdatemainmapCB = new CheckBox();
            monsterstatusonlowhpCB = new CheckBox();
            playerstatusonlowhpCB = new CheckBox();
            useminimapCB = new CheckBox();
            label5 = new Label();
            mainzoomNUD = new NumericUpDown();
            label6 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)verbosityNUD).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mainzoomNUD).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 9);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 0;
            label1.Text = "Sound";
            // 
            // soundeffectsCB
            // 
            soundeffectsCB.AutoSize = true;
            soundeffectsCB.Location = new Point(3, 27);
            soundeffectsCB.Name = "soundeffectsCB";
            soundeffectsCB.Size = new Size(98, 19);
            soundeffectsCB.TabIndex = 1;
            soundeffectsCB.Text = "Sound Effects";
            soundeffectsCB.UseVisualStyleBackColor = true;
            soundeffectsCB.CheckedChanged += SoundEffectsChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 9);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 2;
            label2.Text = "Verbosity";
            // 
            // verbosityNUD
            // 
            verbosityNUD.Location = new Point(4, 27);
            verbosityNUD.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            verbosityNUD.Name = "verbosityNUD";
            verbosityNUD.Size = new Size(54, 23);
            verbosityNUD.TabIndex = 3;
            verbosityNUD.Value = new decimal(new int[] { 3, 0, 0, 0 });
            verbosityNUD.ValueChanged += VerbosityChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 9);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 4;
            label3.Text = "Status Bars";
            // 
            // monsterstatusCB
            // 
            monsterstatusCB.AutoSize = true;
            monsterstatusCB.Location = new Point(3, 27);
            monsterstatusCB.Name = "monsterstatusCB";
            monsterstatusCB.Size = new Size(75, 19);
            monsterstatusCB.TabIndex = 5;
            monsterstatusCB.Text = "Monsters";
            monsterstatusCB.UseVisualStyleBackColor = true;
            monsterstatusCB.CheckedChanged += MonsterStatusChanged;
            // 
            // playerstatusCB
            // 
            playerstatusCB.AutoSize = true;
            playerstatusCB.Location = new Point(3, 52);
            playerstatusCB.Name = "playerstatusCB";
            playerstatusCB.Size = new Size(58, 19);
            playerstatusCB.TabIndex = 6;
            playerstatusCB.Text = "Player";
            playerstatusCB.UseVisualStyleBackColor = true;
            playerstatusCB.CheckedChanged += PlayerStatusChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 11);
            label4.Name = "label4";
            label4.Size = new Size(100, 15);
            label4.TabIndex = 7;
            label4.Text = "Minimap Options";
            // 
            // minimapdrawmmCB
            // 
            minimapdrawmmCB.AutoSize = true;
            minimapdrawmmCB.Location = new Point(3, 29);
            minimapdrawmmCB.Name = "minimapdrawmmCB";
            minimapdrawmmCB.Size = new Size(157, 19);
            minimapdrawmmCB.TabIndex = 8;
            minimapdrawmmCB.Text = "Draw on Magic Mapping";
            minimapdrawmmCB.UseVisualStyleBackColor = true;
            minimapdrawmmCB.CheckedChanged += MiniMapDrawOnMMChanged;
            // 
            // donotupdatemainmapCB
            // 
            donotupdatemainmapCB.AutoSize = true;
            donotupdatemainmapCB.Location = new Point(3, 27);
            donotupdatemainmapCB.Name = "donotupdatemainmapCB";
            donotupdatemainmapCB.Size = new Size(162, 19);
            donotupdatemainmapCB.TabIndex = 9;
            donotupdatemainmapCB.Text = "Do Not Update Main Map";
            donotupdatemainmapCB.UseVisualStyleBackColor = true;
            donotupdatemainmapCB.CheckedChanged += DoNotUpdateMainOnMMChanged;
            // 
            // monsterstatusonlowhpCB
            // 
            monsterstatusonlowhpCB.AutoSize = true;
            monsterstatusonlowhpCB.Location = new Point(84, 27);
            monsterstatusonlowhpCB.Name = "monsterstatusonlowhpCB";
            monsterstatusonlowhpCB.Size = new Size(105, 19);
            monsterstatusonlowhpCB.TabIndex = 10;
            monsterstatusonlowhpCB.Text = "On Low Health";
            monsterstatusonlowhpCB.UseVisualStyleBackColor = true;
            monsterstatusonlowhpCB.CheckedChanged += MonsterStatusLowHPChanged;
            // 
            // playerstatusonlowhpCB
            // 
            playerstatusonlowhpCB.AutoSize = true;
            playerstatusonlowhpCB.Location = new Point(84, 52);
            playerstatusonlowhpCB.Name = "playerstatusonlowhpCB";
            playerstatusonlowhpCB.Size = new Size(105, 19);
            playerstatusonlowhpCB.TabIndex = 10;
            playerstatusonlowhpCB.Text = "On Low Health";
            playerstatusonlowhpCB.UseVisualStyleBackColor = true;
            playerstatusonlowhpCB.CheckedChanged += PlayerStatusLowHPChanged;
            // 
            // useminimapCB
            // 
            useminimapCB.AutoSize = true;
            useminimapCB.Location = new Point(3, 54);
            useminimapCB.Name = "useminimapCB";
            useminimapCB.Size = new Size(96, 19);
            useminimapCB.TabIndex = 11;
            useminimapCB.Text = "Use MiniMap";
            useminimapCB.UseVisualStyleBackColor = true;
            useminimapCB.CheckedChanged += UseMiniMapChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(3, 9);
            label5.Name = "label5";
            label5.Size = new Size(106, 15);
            label5.TabIndex = 12;
            label5.Text = "Main Map Options";
            // 
            // mainzoomNUD
            // 
            mainzoomNUD.Location = new Point(3, 52);
            mainzoomNUD.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            mainzoomNUD.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            mainzoomNUD.Name = "mainzoomNUD";
            mainzoomNUD.Size = new Size(60, 23);
            mainzoomNUD.TabIndex = 13;
            mainzoomNUD.Value = new decimal(new int[] { 15, 0, 0, 0 });
            mainzoomNUD.ValueChanged += MainZoomChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(69, 56);
            label6.Name = "label6";
            label6.Size = new Size(69, 15);
            label6.TabIndex = 14;
            label6.Text = "Zoom Level";
            // 
            // button1
            // 
            button1.BackColor = Color.LightSalmon;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(83, 108);
            button1.Name = "button1";
            button1.Size = new Size(78, 33);
            button1.TabIndex = 15;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = false;
            button1.Click += CancelClick;
            // 
            // button2
            // 
            button2.BackColor = Color.Aqua;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(268, 108);
            button2.Name = "button2";
            button2.Size = new Size(78, 33);
            button2.TabIndex = 15;
            button2.Text = "Defaults";
            button2.UseVisualStyleBackColor = false;
            button2.Click += DefaultsClick;
            // 
            // button3
            // 
            button3.BackColor = Color.Gold;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(616, 108);
            button3.Name = "button3";
            button3.Size = new Size(78, 33);
            button3.TabIndex = 15;
            button3.Text = "Accept";
            button3.UseVisualStyleBackColor = false;
            button3.Click += AcceptClick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.WhiteSmoke;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(soundeffectsCB);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(110, 80);
            panel1.TabIndex = 16;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(label2);
            panel2.Controls.Add(verbosityNUD);
            panel2.Location = new Point(128, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(75, 80);
            panel2.TabIndex = 17;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(label3);
            panel3.Controls.Add(monsterstatusCB);
            panel3.Controls.Add(playerstatusCB);
            panel3.Controls.Add(monsterstatusonlowhpCB);
            panel3.Controls.Add(playerstatusonlowhpCB);
            panel3.Location = new Point(209, 12);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 80);
            panel3.TabIndex = 18;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(label4);
            panel4.Controls.Add(minimapdrawmmCB);
            panel4.Controls.Add(useminimapCB);
            panel4.Location = new Point(416, 12);
            panel4.Name = "panel4";
            panel4.Size = new Size(175, 80);
            panel4.TabIndex = 19;
            // 
            // panel5
            // 
            panel5.BackColor = Color.White;
            panel5.Controls.Add(label5);
            panel5.Controls.Add(donotupdatemainmapCB);
            panel5.Controls.Add(label6);
            panel5.Controls.Add(mainzoomNUD);
            panel5.Location = new Point(597, 12);
            panel5.Name = "panel5";
            panel5.Size = new Size(175, 80);
            panel5.TabIndex = 20;
            // 
            // button4
            // 
            button4.BackColor = Color.LawnGreen;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            button4.Location = new Point(446, 108);
            button4.Name = "button4";
            button4.Size = new Size(78, 33);
            button4.TabIndex = 15;
            button4.Text = "Advanced";
            button4.UseVisualStyleBackColor = false;
            button4.Click += AdvancedClick;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(783, 153);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Settings";
            Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)verbosityNUD).EndInit();
            ((System.ComponentModel.ISupportInitialize)mainzoomNUD).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private CheckBox soundeffectsCB;
        private Label label2;
        private NumericUpDown verbosityNUD;
        private Label label3;
        private CheckBox monsterstatusCB;
        private CheckBox playerstatusCB;
        private Label label4;
        private CheckBox minimapdrawmmCB;
        private CheckBox donotupdatemainmapCB;
        private CheckBox monsterstatusonlowhpCB;
        private CheckBox playerstatusonlowhpCB;
        private CheckBox useminimapCB;
        private Label label5;
        private NumericUpDown mainzoomNUD;
        private Label label6;
        private Button button1;
        private Button button2;
        private Button button3;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Button button4;
    }
}