namespace YARPG
{
    partial class AttackCreation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttackCreation));
            label1 = new Label();
            textToHit = new TextBox();
            label2 = new Label();
            comboDamages = new ComboBox();
            label3 = new Label();
            comboDamageTypes = new ComboBox();
            label4 = new Label();
            textRange = new TextBox();
            label5 = new Label();
            textArea = new TextBox();
            label6 = new Label();
            comboEffect = new ComboBox();
            button1 = new Button();
            button2 = new Button();
            label7 = new Label();
            textMaxRange = new TextBox();
            label8 = new Label();
            textName = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 55);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 0;
            label1.Text = "To Hit bonus";
            // 
            // textToHit
            // 
            textToHit.Location = new Point(87, 52);
            textToHit.Name = "textToHit";
            textToHit.Size = new Size(72, 23);
            textToHit.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(182, 84);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 2;
            label2.Text = "Damages";
            // 
            // comboDamages
            // 
            comboDamages.FormattingEnabled = true;
            comboDamages.Location = new Point(244, 81);
            comboDamages.Name = "comboDamages";
            comboDamages.Size = new Size(121, 23);
            comboDamages.TabIndex = 3;
            comboDamages.SelectedIndexChanged += DamagesChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(371, 84);
            label3.Name = "label3";
            label3.Size = new Size(83, 15);
            label3.TabIndex = 2;
            label3.Text = "Damage Types";
            // 
            // comboDamageTypes
            // 
            comboDamageTypes.FormattingEnabled = true;
            comboDamageTypes.Location = new Point(460, 81);
            comboDamageTypes.Name = "comboDamageTypes";
            comboDamageTypes.Size = new Size(121, 23);
            comboDamageTypes.TabIndex = 3;
            comboDamageTypes.SelectedIndexChanged += DamageTypesChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(165, 55);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 0;
            label4.Text = "Range";
            // 
            // textRange
            // 
            textRange.Location = new Point(211, 52);
            textRange.Name = "textRange";
            textRange.Size = new Size(72, 23);
            textRange.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(472, 55);
            label5.Name = "label5";
            label5.Size = new Size(31, 15);
            label5.TabIndex = 0;
            label5.Text = "Area";
            // 
            // textArea
            // 
            textArea.Location = new Point(509, 52);
            textArea.Name = "textArea";
            textArea.Size = new Size(72, 23);
            textArea.TabIndex = 1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 84);
            label6.Name = "label6";
            label6.Size = new Size(37, 15);
            label6.TabIndex = 2;
            label6.Text = "Effect";
            // 
            // comboEffect
            // 
            comboEffect.FormattingEnabled = true;
            comboEffect.Location = new Point(55, 81);
            comboEffect.Name = "comboEffect";
            comboEffect.Size = new Size(121, 23);
            comboEffect.TabIndex = 3;
            comboEffect.SelectedIndexChanged += EffectChanged;
            // 
            // button1
            // 
            button1.BackColor = Color.LightCoral;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(130, 121);
            button1.Name = "button1";
            button1.Size = new Size(75, 40);
            button1.TabIndex = 4;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = false;
            button1.Click += CancelClicked;
            // 
            // button2
            // 
            button2.BackColor = Color.LawnGreen;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(367, 121);
            button2.Name = "button2";
            button2.Size = new Size(75, 40);
            button2.TabIndex = 4;
            button2.Text = "Okay";
            button2.UseVisualStyleBackColor = false;
            button2.Click += OkayClicked;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(289, 55);
            label7.Name = "label7";
            label7.Size = new Size(98, 15);
            label7.TabIndex = 5;
            label7.Text = "Maximum Range";
            // 
            // textMaxRange
            // 
            textMaxRange.Location = new Point(393, 52);
            textMaxRange.Name = "textMaxRange";
            textMaxRange.Size = new Size(72, 23);
            textMaxRange.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(11, 26);
            label8.Name = "label8";
            label8.Size = new Size(90, 15);
            label8.TabIndex = 0;
            label8.Text = "Name of Attack";
            // 
            // textName
            // 
            textName.Location = new Point(107, 23);
            textName.Name = "textName";
            textName.Size = new Size(258, 23);
            textName.TabIndex = 1;
            // 
            // AttackCreation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(597, 173);
            Controls.Add(label7);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(comboEffect);
            Controls.Add(label6);
            Controls.Add(comboDamageTypes);
            Controls.Add(label3);
            Controls.Add(comboDamages);
            Controls.Add(label2);
            Controls.Add(textArea);
            Controls.Add(label5);
            Controls.Add(textMaxRange);
            Controls.Add(textRange);
            Controls.Add(label4);
            Controls.Add(textName);
            Controls.Add(label8);
            Controls.Add(textToHit);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AttackCreation";
            Text = "Attack Creation";
            Load += ACload;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textToHit;
        private Label label2;
        private ComboBox comboDamages;
        private Label label3;
        private ComboBox comboDamageTypes;
        private Label label4;
        private TextBox textRange;
        private Label label5;
        private TextBox textArea;
        private Label label6;
        private ComboBox comboEffect;
        private Button button1;
        private Button button2;
        private Label label7;
        private TextBox textMaxRange;
        private Label label8;
        private TextBox textName;
    }
}