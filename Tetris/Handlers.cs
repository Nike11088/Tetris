using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Tetris
{
    partial class MainForm
    {
        private void tsmiNewGame_Click(object sender, EventArgs e)
        {
            if (gameThread != null)
            {
                if (gameThread.ThreadState == ThreadState.Suspended)
                {
                    tsmiPause.Text = "Пауза";
                    gameStatus = GameStatus.Play;
                    gameThread.Resume();
                }
                gameThread.Abort();
            }

            NewGame();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void tsmiPause_Click(object sender, EventArgs e)
        {   
            if (gameThread == null)
                return;

            if (gameStatus == GameStatus.Play)
            {
                //pauseThread = new Thread(Pause);
                gameThread.Suspend();                

                tsmiPause.Text = "Продолжить";
                gameStatus = GameStatus.Pause;
            }
            else if (gameStatus == GameStatus.Pause)
            {
                tsmiPause.Text = "Пауза";
                gameStatus = GameStatus.Play;
              
                //pauseThread.Abort();
                gameThread.Resume();
            }
        }

        private void tsmiFullScreen_Click(object sender, EventArgs e)
        {
            if (windowStatus == WindowStatus.InWindow ||
                windowStatus == WindowStatus.MaximumWindow)
            {
                tempLocation = this.Location;
                tempSize = this.Size;
                tempWindowState = this.WindowState;

                if (this.WindowState == FormWindowState.Maximized)
                    tempWindowStatus = WindowStatus.MaximumWindow;
                else if (this.WindowState == FormWindowState.Normal)
                    tempWindowStatus = WindowStatus.InWindow;

                SetFullScreen();
            }
            else
            {
                SetInWindow();

                switch (windowStatus)
                {
                    case WindowStatus.InWindow:
                        this.WindowState = FormWindowState.Normal;
                        break;
                    case WindowStatus.MaximumWindow:
                        this.WindowState = FormWindowState.Maximized;
                        break;
                }
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
           
        }              

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameStatus == GameStatus.Stop ||
                gameStatus == GameStatus.Pause)
                return;

            if (e.KeyCode == Keys.D ||
                e.KeyCode == Keys.Right)
                block.MoveRight();           

            if (e.KeyCode == Keys.A ||
                e.KeyCode == Keys.Left)
                block.MoveLeft();                
            
            if (e.KeyCode == Keys.S ||
                e.KeyCode == Keys.Down)
                block.MoveDown();

            if (e.KeyCode == Keys.W ||
                e.KeyCode == Keys.Up)
                block.Rotate();

            if (e.KeyCode == Keys.F1)
            {
                if (level == 10)
                    return;
                level++;
                if (speed == 400)
                    incrementSpeed = 50;                
                speed -= incrementSpeed;
                               
                tsslLevel.Text = String.Format("Уровень: {0}", level);
            }

            if (e.KeyCode == Keys.F2)
            {
                if (level == 1)
                    return;
                level--;
                if (speed == 400)
                    incrementSpeed = 200;
                speed += incrementSpeed;                
                tsslLevel.Text = String.Format("Уровень: {0}", level);
            }

            /*if (e.KeyCode == Keys.NumPad6)
            {
                this.ClientSize = new Size(this.ClientSize.Width + 373, this.ClientSize.Height + 748);
                //this.ClientSize = new Size(250, this.ClientSize.Height);
            }
            if (e.KeyCode == Keys.NumPad4)
            {
                this.ClientSize = new Size(this.ClientSize.Width - 1, this.ClientSize.Height);
                //this.ClientSize = new Size(250, this.ClientSize.Height);
            }


            if (e.KeyCode == Keys.NumPad2)
            {
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 1);
            } 
            if (e.KeyCode == Keys.NumPad8)
            {
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height - 1);
            }*/

            myPanel.Invalidate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadGame();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            Exit();
        }        
    }
}
