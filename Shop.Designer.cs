namespace YARPG
{
    partial class Shop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Shop));
            label1 = new Label();
            listWeapons = new ListBox();
            label2 = new Label();
            listArmors = new ListBox();
            label3 = new Label();
            listPotions = new ListBox();
            label4 = new Label();
            listScrolls = new ListBox();
            label5 = new Label();
            listLights = new ListBox();
            label6 = new Label();
            listInventory = new ListBox();
            button1 = new Button();
            button2 = new Button();
            textGold = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 16);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 0;
            label1.Text = "Weapons";
            // 
            // listWeapons
            // 
            listWeapons.FormattingEnabled = true;
            listWeapons.ItemHeight = 15;
            listWeapons.Location = new Point(16, 40);
            listWeapons.Name = "listWeapons";
            listWeapons.Size = new Size(120, 364);
            listWeapons.TabIndex = 1;
            listWeapons.SelectedIndexChanged += WeaponSelect;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(151, 16);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 0;
            label2.Text = "Armors";
            // 
            // listArmors
            // 
            listArmors.FormattingEnabled = true;
            listArmors.ItemHeight = 15;
            listArmors.Location = new Point(152, 40);
            listArmors.Name = "listArmors";
            listArmors.Size = new Size(120, 364);
            listArmors.TabIndex = 1;
            listArmors.SelectedIndexChanged += ArmorSelect;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(288, 16);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 0;
            label3.Text = "Potions";
            // 
            // listPotions
            // 
            listPotions.FormattingEnabled = true;
            listPotions.ItemHeight = 15;
            listPotions.Location = new Point(289, 40);
            listPotions.Name = "listPotions";
            listPotions.Size = new Size(120, 364);
            listPotions.TabIndex = 1;
            listPotions.SelectedIndexChanged += PotionSelect;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(426, 16);
            label4.Name = "label4";
            label4.Size = new Size(41, 15);
            label4.TabIndex = 0;
            label4.Text = "Scrolls";
            // 
            // listScrolls
            // 
            listScrolls.FormattingEnabled = true;
            listScrolls.ItemHeight = 15;
            listScrolls.Location = new Point(427, 40);
            listScrolls.Name = "listScrolls";
            listScrolls.Size = new Size(120, 364);
            listScrolls.TabIndex = 1;
            listScrolls.SelectedIndexChanged += ScrollSelect;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(566, 16);
            label5.Name = "label5";
            label5.Size = new Size(39, 15);
            label5.TabIndex = 0;
            label5.Text = "Lights";
            // 
            // listLights
            // 
            listLights.FormattingEnabled = true;
            listLights.ItemHeight = 15;
            listLights.Location = new Point(567, 40);
            listLights.Name = "listLights";
            listLights.Size = new Size(120, 364);
            listLights.TabIndex = 1;
            listLights.SelectedIndexChanged += LightSelect;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(709, 54);
            label6.Name = "label6";
            label6.Size = new Size(57, 15);
            label6.TabIndex = 0;
            label6.Text = "Inventory";
            // 
            // listInventory
            // 
            listInventory.BackColor = Color.Yellow;
            listInventory.FormattingEnabled = true;
            listInventory.ItemHeight = 15;
            listInventory.Location = new Point(709, 85);
            listInventory.Name = "listInventory";
            listInventory.Size = new Size(120, 319);
            listInventory.TabIndex = 1;
            listInventory.SelectedIndexChanged += InventorySelect;
            // 
            // button1
            // 
            button1.BackColor = Color.LightCoral;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(199, 426);
            button1.Name = "button1";
            button1.Size = new Size(75, 36);
            button1.TabIndex = 2;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Cancel;
            // 
            // button2
            // 
            button2.BackColor = Color.LawnGreen;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = SystemColors.ControlText;
            button2.Location = new Point(566, 426);
            button2.Name = "button2";
            button2.Size = new Size(75, 36);
            button2.TabIndex = 2;
            button2.Text = "Done";
            button2.UseVisualStyleBackColor = false;
            button2.Click += Done;
            // 
            // textGold
            // 
            textGold.Location = new Point(709, 16);
            textGold.Name = "textGold";
            textGold.Size = new Size(120, 23);
            textGold.TabIndex = 3;
            // 
            // Shop
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(845, 478);
            Controls.Add(textGold);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listInventory);
            Controls.Add(label6);
            Controls.Add(listLights);
            Controls.Add(label5);
            Controls.Add(listScrolls);
            Controls.Add(label4);
            Controls.Add(listPotions);
            Controls.Add(label3);
            Controls.Add(listArmors);
            Controls.Add(label2);
            Controls.Add(listWeapons);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Shop";
            Text = "Shop";
            Load += ShopLoad;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox listWeapons;
        private Label label2;
        private ListBox listArmors;
        private Label label3;
        private ListBox listPotions;
        private Label label4;
        private ListBox listScrolls;
        private Label label5;
        private ListBox listLights;
        private Label label6;
        private ListBox listInventory;
        private Button button1;
        private Button button2;
        private TextBox textGold;
    }
}