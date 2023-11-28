namespace YARPG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel = new Panel();
            box = new PictureBox();
            info = new RichTextBox();
            minimap = new PictureBox();
            panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)box).BeginInit();
            ((System.ComponentModel.ISupportInitialize)minimap).BeginInit();
            SuspendLayout();
            // 
            // panel
            // 
            panel.Controls.Add(box);
            panel.Location = new Point(0, 0);
            panel.Name = "panel";
            panel.Size = new Size(798, 284);
            panel.TabIndex = 0;
            // 
            // box
            // 
            box.Location = new Point(14, 14);
            box.Name = "box";
            box.Size = new Size(774, 258);
            box.TabIndex = 0;
            box.TabStop = false;
            box.MouseDown += Box_MouseDown;
            box.MouseMove += Box_MouseMove;
            // 
            // info
            // 
            info.Location = new Point(10, 290);
            info.Name = "info";
            info.ReadOnly = true;
            info.Size = new Size(643, 148);
            info.TabIndex = 1;
            info.TabStop = false;
            info.Text = "";
            // 
            // minimap
            // 
            minimap.BackColor = Color.Black;
            minimap.Location = new Point(659, 290);
            minimap.Name = "minimap";
            minimap.Size = new Size(129, 148);
            minimap.TabIndex = 2;
            minimap.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(minimap);
            Controls.Add(info);
            Controls.Add(panel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "Form1";
            Text = "Yet Another Role Playing Game";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            Resize += Form1_Resize;
            panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)box).EndInit();
            ((System.ComponentModel.ISupportInitialize)minimap).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel;
        private RichTextBox info;
        private PictureBox box;
        private PictureBox minimap;
    }
}