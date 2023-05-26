using System.Drawing;
using System.Windows.Forms;
namespace BNetInstaller
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
            button1 = new Button();
            checkBox1 = new CheckBox();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            checkBox3 = new CheckBox();
            label3 = new Label();
            label4 = new Label();
            statusStrip1 = new StatusStrip();
            toolStripSplitButton1 = new ToolStripDropDownButton();
            ruToolStripMenuItem = new ToolStripMenuItem();
            engToolStripMenuItem = new ToolStripMenuItem();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(10, 10);
            button1.Name = "button1";
            button1.Size = new Size(135, 50);
            button1.TabIndex = 0;
            button1.Text = "Играть";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 66);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(130, 19);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "Запомнить пароль";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.None;
            button2.Enabled = false;
            button2.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(150, 10);
            button2.Name = "button2";
            button2.Size = new Size(135, 50);
            button2.TabIndex = 4;
            button2.Text = "Обновить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(155, 88);
            label1.Name = "label1";
            label1.Size = new Size(98, 15);
            label1.TabIndex = 6;
            label1.Text = "Текущая версия:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(155, 103);
            label2.Name = "label2";
            label2.Size = new Size(68, 15);
            label2.TabIndex = 7;
            label2.Text = "неизвестна";
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(155, 66);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(125, 19);
            checkBox3.TabIndex = 8;
            checkBox3.Text = "Проверка файлов";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 88);
            label3.Name = "label3";
            label3.Size = new Size(114, 15);
            label3.TabIndex = 9;
            label3.Text = "Актуальная версия:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 103);
            label4.Name = "label4";
            label4.Size = new Size(68, 15);
            label4.TabIndex = 10;
            label4.Text = "неизвестна";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            statusStrip1.ImeMode = ImeMode.NoControl;
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripSplitButton1, toolStripProgressBar1, toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 129);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(294, 22);
            statusStrip1.TabIndex = 12;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[] { ruToolStripMenuItem, engToolStripMenuItem });
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new Size(13, 20);
            toolStripSplitButton1.Text = "toolStripSplitButton1";
            toolStripSplitButton1.TextDirection = ToolStripTextDirection.Horizontal;
            // 
            // ruToolStripMenuItem
            // 
            ruToolStripMenuItem.Name = "ruToolStripMenuItem";
            ruToolStripMenuItem.Size = new Size(141, 22);
            ruToolStripMenuItem.Text = "Русский";
            ruToolStripMenuItem.Click += ruToolStripMenuItem_Click;
            // 
            // engToolStripMenuItem
            // 
            engToolStripMenuItem.Name = "engToolStripMenuItem";
            engToolStripMenuItem.Size = new Size(141, 22);
            engToolStripMenuItem.Text = "Английский";
            engToolStripMenuItem.Click += engToolStripMenuItem_Click;
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(125, 16);
            toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(39, 17);
            toolStripStatusLabel1.Text = "Status";
            toolStripStatusLabel1.Click += toolStripStatusLabel1_Click;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            ClientSize = new Size(294, 151);
            Controls.Add(statusStrip1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(checkBox3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "D4 Launcher";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private CheckBox checkBox1;
        private Button button2;
        private Label label1;
        private Label label2;
        private CheckBox checkBox3;
        private Label label3;
        private Label label4;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripDropDownButton toolStripSplitButton1;
        private ToolStripMenuItem ruToolStripMenuItem;
        private ToolStripMenuItem engToolStripMenuItem;
    }
}
