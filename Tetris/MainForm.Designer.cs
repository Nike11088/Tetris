namespace Tetris
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPause = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.ssInfo = new System.Windows.Forms.StatusStrip();
            this.tsslLines = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLevel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslWidth = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslHeight = new System.Windows.Forms.ToolStripStatusLabel();
            this.myPanel = new Tetris.MyPanel();
            this.menuStrip1.SuspendLayout();
            this.ssInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMenu,
            this.tsmiPause,
            this.tsmiFullScreen});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiMenu
            // 
            this.tsmiMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewGame,
            this.toolStripSeparator1,
            this.tsmiExit});
            this.tsmiMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsmiMenu.Name = "tsmiMenu";
            this.tsmiMenu.Size = new System.Drawing.Size(53, 20);
            this.tsmiMenu.Text = "Меню";
            // 
            // tsmiNewGame
            // 
            this.tsmiNewGame.Name = "tsmiNewGame";
            this.tsmiNewGame.Size = new System.Drawing.Size(136, 22);
            this.tsmiNewGame.Text = "Новая игра";
            this.tsmiNewGame.Click += new System.EventHandler(this.tsmiNewGame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(136, 22);
            this.tsmiExit.Text = "Выход";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiPause
            // 
            this.tsmiPause.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsmiPause.Name = "tsmiPause";
            this.tsmiPause.Size = new System.Drawing.Size(51, 20);
            this.tsmiPause.Text = "Пауза";
            this.tsmiPause.Click += new System.EventHandler(this.tsmiPause_Click);
            // 
            // tsmiFullScreen
            // 
            this.tsmiFullScreen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsmiFullScreen.Name = "tsmiFullScreen";
            this.tsmiFullScreen.Size = new System.Drawing.Size(100, 20);
            this.tsmiFullScreen.Text = "Полный экран";
            this.tsmiFullScreen.Click += new System.EventHandler(this.tsmiFullScreen_Click);
            // 
            // ssInfo
            // 
            this.ssInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ssInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslLines,
            this.tsslLevel,
            this.tsslWidth,
            this.tsslHeight});
            this.ssInfo.Location = new System.Drawing.Point(0, 239);
            this.ssInfo.Name = "ssInfo";
            this.ssInfo.Size = new System.Drawing.Size(284, 22);
            this.ssInfo.SizingGrip = false;
            this.ssInfo.TabIndex = 0;
            this.ssInfo.Text = "statusStrip1";
            // 
            // tsslLines
            // 
            this.tsslLines.Name = "tsslLines";
            this.tsslLines.Size = new System.Drawing.Size(55, 17);
            this.tsslLines.Text = "Линии: 0";
            // 
            // tsslLevel
            // 
            this.tsslLevel.Name = "tsslLevel";
            this.tsslLevel.Size = new System.Drawing.Size(65, 17);
            this.tsslLevel.Text = "Уровень: 1";
            // 
            // tsslWidth
            // 
            this.tsslWidth.Name = "tsslWidth";
            this.tsslWidth.Size = new System.Drawing.Size(56, 17);
            this.tsslWidth.Text = "tsslWidth";
            this.tsslWidth.Visible = false;
            // 
            // tsslHeight
            // 
            this.tsslHeight.Name = "tsslHeight";
            this.tsslHeight.Size = new System.Drawing.Size(60, 17);
            this.tsslHeight.Text = "tsslHeight";
            this.tsslHeight.Visible = false;
            // 
            // myPanel
            // 
            this.myPanel.BackColor = System.Drawing.Color.Maroon;
            this.myPanel.Location = new System.Drawing.Point(0, 24);
            this.myPanel.Name = "myPanel";
            this.myPanel.Size = new System.Drawing.Size(284, 237);
            this.myPanel.TabIndex = 1;
            this.myPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.myPanel_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ssInfo);
            this.Controls.Add(this.myPanel);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Тетрис";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ssInfo.ResumeLayout(false);
            this.ssInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMenu;
        private MyPanel myPanel;
        private System.Windows.Forms.ToolStripMenuItem tsmiPause;
        private System.Windows.Forms.StatusStrip ssInfo;
        private System.Windows.Forms.ToolStripStatusLabel tsslLines;
        private System.Windows.Forms.ToolStripStatusLabel tsslLevel;
        private System.Windows.Forms.ToolStripStatusLabel tsslWidth;
        private System.Windows.Forms.ToolStripStatusLabel tsslHeight;
        private System.Windows.Forms.ToolStripMenuItem tsmiFullScreen;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewGame;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;        
    }
}

