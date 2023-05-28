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
            button_play = new Button();
            checkBox_store_password = new CheckBox();
            button_update = new Button();
            label1 = new Label();
            label_current_version = new Label();
            checkbox_check_files = new CheckBox();
            label3 = new Label();
            label_actual_version = new Label();
            statusStrip1 = new StatusStrip();
            toolStripSplitButton1 = new ToolStripDropDownButton();
            ruToolStripMenuItem = new ToolStripMenuItem();
            engToolStripMenuItem = new ToolStripMenuItem();
            progressbar = new ToolStripProgressBar();
            statusLabel = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button_play
            // 
            button_play.Enabled = false;
            button_play.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            button_play.Location = new Point(10, 10);
            button_play.Name = "button_play";
            button_play.Size = new Size(135, 50);
            button_play.TabIndex = 0;
            button_play.Text = "Играть";
            button_play.UseVisualStyleBackColor = true;
            button_play.Click += button_play_Click;
            // 
            // checkBox_store_password
            // 
            checkBox_store_password.AutoSize = true;
            checkBox_store_password.Location = new Point(12, 66);
            checkBox_store_password.Name = "checkBox_store_password";
            checkBox_store_password.Size = new Size(130, 19);
            checkBox_store_password.TabIndex = 1;
            checkBox_store_password.Text = "Запомнить пароль";
            checkBox_store_password.UseVisualStyleBackColor = true;
            checkBox_store_password.CheckedChanged += checkBox_store_password_CheckedChanged;
            // 
            // button_update
            // 
            button_update.Anchor = AnchorStyles.None;
            button_update.Enabled = false;
            button_update.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            button_update.Location = new Point(150, 10);
            button_update.Name = "button_update";
            button_update.Size = new Size(135, 50);
            button_update.TabIndex = 4;
            button_update.Text = "Обновить";
            button_update.UseVisualStyleBackColor = true;
            button_update.Click += button_update_Click;
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
            // label_current_version
            // 
            label_current_version.AutoSize = true;
            label_current_version.Location = new Point(155, 103);
            label_current_version.Name = "label_current_version";
            label_current_version.Size = new Size(68, 15);
            label_current_version.TabIndex = 7;
            label_current_version.Text = "неизвестна";
            // 
            // checkbox_check_files
            // 
            checkbox_check_files.AutoSize = true;
            checkbox_check_files.Location = new Point(155, 66);
            checkbox_check_files.Name = "checkbox_check_files";
            checkbox_check_files.Size = new Size(125, 19);
            checkbox_check_files.TabIndex = 8;
            checkbox_check_files.Text = "Проверка файлов";
            checkbox_check_files.UseVisualStyleBackColor = true;
            checkbox_check_files.CheckedChanged += checkbox_check_files_CheckedChanged;
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
            // label_actual_version
            // 
            label_actual_version.AutoSize = true;
            label_actual_version.Location = new Point(12, 103);
            label_actual_version.Name = "label_actual_version";
            label_actual_version.Size = new Size(68, 15);
            label_actual_version.TabIndex = 10;
            label_actual_version.Text = "неизвестна";
            label_actual_version.TextAlign = ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            statusStrip1.ImeMode = ImeMode.NoControl;
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripSplitButton1, progressbar, statusLabel });
            statusStrip1.Location = new Point(0, 126);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(294, 25);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 12;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[] { ruToolStripMenuItem, engToolStripMenuItem });
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new Size(13, 23);
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
            // progressbar
            // 
            progressbar.Name = "progressbar";
            progressbar.Size = new Size(110, 19);
            progressbar.Visible = false;
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(0, 20);
            statusLabel.Click += toolStripStatusLabel1_Click;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            ClientSize = new Size(294, 151);
            Controls.Add(statusStrip1);
            Controls.Add(label_actual_version);
            Controls.Add(label3);
            Controls.Add(checkbox_check_files);
            Controls.Add(label_current_version);
            Controls.Add(label1);
            Controls.Add(button_update);
            Controls.Add(checkBox_store_password);
            Controls.Add(button_play);
            FormBorderStyle = FormBorderStyle.FixedDialog;
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

        private Button button_play;
        private CheckBox checkBox_store_password;
        private Button button_update;
        private Label label1;
        private Label label_current_version;
        private CheckBox checkbox_check_files;
        private Label label3;
        private Label label_actual_version;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar progressbar;
        private ToolStripStatusLabel statusLabel;
        private ToolStripDropDownButton toolStripSplitButton1;
        private ToolStripMenuItem ruToolStripMenuItem;
        private ToolStripMenuItem engToolStripMenuItem;
    }
}
