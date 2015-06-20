//    #define BLOCK_O

//   #define BLOCK_I_G
//    #define BLOCK_I_V

//    #define BLOCK_S_G
//    #define BLOCK_S_V

//    #define BLOCK_Z_G
//    #define BLOCK_Z_V

//    #define BLOCK_L_GD
//    #define BLOCK_L_GU
//    #define BLOCK_L_VR
//    #define BLOCK_L_VL

//    #define BLOCK_J_GD
//    #define BLOCK_J_GU
//    #define BLOCK_J_VR
//    #define BLOCK_J_VL

//    #define BLOCK_T_GD
//    #define BLOCK_T_GU
//    #define BLOCK_T_VR
//    #define BLOCK_T_VL

//    #define OBSTRUCTION
//    #define OBSTRUCTION_1

//    #define MANUAL

//    #define MESSAGE

////////////////////////////////////////////////////////////////
// Программа «Tetris» - классическая игра «Тетрис»            //
//                                                            //
//                           Автор: Абакумов Никита, 2014 год //
////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tetris
{
    public partial class MainForm : Form
    {
        private int nLines; // Количество набранных линий    
        private int level = 1;

        private int FIELD_WIDTH = 10;
        private int FIELD_HEIGHT = 20;
        
        private int CELL_WIDTH;
        private int CELL_HEIGHT;

        private int speed;

        private int levelStep = 0;
        private int incrementSpeed = 200;


        private GameStatus gameStatus; // статус игры        

        private Graphics g; // графическая поверхность формы

        private Field field;

        private Block block;
        private Block[] blocks;

        private Thread gameThread;
        private Thread pauseThread;

        private Random rnd;

        public MainForm()
        {
            InitializeComponent();

            int size = (int)(SystemInformation.PrimaryMonitorSize.Height * 0.025);

            CELL_WIDTH = size;
            CELL_HEIGHT = size;

            field = new Field(FIELD_HEIGHT, FIELD_WIDTH, CELL_HEIGHT, CELL_WIDTH);

            // Устанавливаем размер формы в соответствии
            // с размером игрового поля

            myPanel.Location = new Point(0, menuStrip1.Height);
            myPanel.Size = new Size(CELL_WIDTH * FIELD_WIDTH + 1,
                CELL_HEIGHT * FIELD_HEIGHT + 1);

            this.ClientSize = new System.Drawing.Size(CELL_WIDTH * FIELD_WIDTH + 1,
                CELL_HEIGHT * FIELD_HEIGHT + menuStrip1.Height + 1 + ssInfo.Height);

            this.MinimumSize = new Size(this.Width, this.Height);

            gameStatus = GameStatus.Stop;
            blocks = new Block[20];
            rnd = new Random();
        }

        private void NewGame()
        {
            field.Clear();
            gameStatus = GameStatus.Stop;
#if OBSTRUCTION
            // Препятствия для тестирования
            int x = 1;
            int y = 4;
            /*for (int row = y; row < y + 4; row++)
                for (int col = x; col < x + 1; col++)
                    field.GameField[row, col] = 1;
            x = 8;
            y = 2;
            for (int row = y; row < y + 4; row++)
                for (int col = x; col < x + 1; col++)
                    field.GameField[row, col] = 1;

            x = 2;
            y = 13;
            for (int row = y; row < y + 1; row++)
                for (int col = x; col < x + 4; col++)
                    field.GameField[row, col] = 1;*/

            x = 4;
            y = 2;
            for (int row = y; row < y + 13; row++)
                for (int col = x; col < x + 4; col++)
                    field.GameField[row, col] = 1;

#endif
            gameThread = new Thread(Play);
            gameThread.Start();
            nLines = 0; // Нет набранных линий
            level = 1;
            speed = 1000;
            levelStep = 0;

            SetInformation();
        }

        private void Play()
        {
#if BLOCK_O
            block = new BlockO(field);
#elif BLOCK_I_G
            block = new BlockI(State.Gorizontal, field);
            //block = new BlockO(field);
#elif BLOCK_I_V
            block = new BlockI(State.Vertical, field);
#elif BLOCK_S_G
            block = new BlockS(State.Gorizontal, field);
#elif BLOCK_S_V
            block = new BlockS(State.Vertical, field);
#elif BLOCK_Z_G
            block = new BlockZ(State.Gorizontal, field);
#elif BLOCK_Z_V
            block = new BlockZ(State.Vertical, field);
#elif BLOCK_L_GD
            block = new BlockL(State.GorizontalDown, field);
#elif BLOCK_L_GU
            block = new BlockL(State.GorizontalUp, field);
#elif BLOCK_L_VR
            block = new BlockL(State.VerticalRight, field);
#elif BLOCK_L_VL
            block = new BlockL(State.VerticalLeft, field);
#elif BLOCK_J_GD
            block = new BlockJ(State.GorizontalDown, field);
#elif BLOCK_J_GU
            block = new BlockJ(State.GorizontalUp, field);
#elif BLOCK_J_VR
            block = new BlockJ(State.VerticalRight, field);
#elif BLOCK_J_VL
            block = new BlockJ(State.VerticalLeft, field);
#elif BLOCK_T_GD
            block = new BlockT(State.GorizontalDown, field);
#elif BLOCK_T_GU
            block = new BlockT(State.GorizontalUp, field);
#elif BLOCK_T_VR
            block = new BlockT(State.VerticalRight, field);
#elif BLOCK_T_VL
            block = new BlockT(State.VerticalLeft, field);
#endif

#if MANUAL
            block.Create();
            myPanel.Invalidate();
#else
            int i;
            CreateBlocks();

            if (gameStatus == GameStatus.Stop)
            {
                i = rnd.Next(0, 19);
                block = blocks[i];
                block.Create();
                myPanel.Invalidate();
                gameStatus = GameStatus.Play;
            }

            while (true)
            {
                Thread.Sleep(speed);

                if (block.MoveDown() == false)
                {
                    if (block.Steps == 0)
                    {
                        StopGame("\nКонец игры");
                    }

                    nLines += CountLines();
                    SetInformation();

                    if (nLines % 100 == 0 && speed > 100 && nLines != levelStep)
                    {
                        if (level == 10 && nLines == 1000)
                        {
                            StopGame("Поздравляем, Вы завершили игру!\nВы король тетриса.");
                        }

                        LevelUp();
                    }

                    i = rnd.Next(0, 19);
                    block = blocks[i];

                    if (block.Create() == false)
                    {
                        block.CreateBlock();
                        myPanel.Invalidate();
                        StopGame("\nКонец игры");
                    }
                }

                myPanel.Invalidate();
            }
#endif
        }

        private void LevelUp()
        {
            level++;
            if (speed <= 400)
                incrementSpeed = 50;
            speed -= incrementSpeed;
            levelStep += 100;

            SetInformation();
        }

        private void SetInformation()
        {
            tsslLines.Text = String.Format("Линии: {0}", nLines);
            tsslLevel.Text = String.Format("Уровень: {0}", level);

            //tsslWidth.Visible = true;
            //tsslHeight.Visible = true;
            //tsslWidth.Text = "Widht: " + this.ClientSize.Width.ToString();
            //tsslHeight.Text = "Height: " + this.ClientSize.Height.ToString();

            //tsslWidth.Text = "Widht: " + myPanel.Width.ToString();
            //tsslHeight.Text = "Height: " + myPanel.Height.ToString(); 
        }

        private int CountLines()
        {
            bool linesFlag;
            int nLines = 0;

            for (int row = 0; row < field.Height; row++)
            {
                linesFlag = true;
                for (int col = 0; col < field.Width; col++)
                {
                    if (field.GameField[row, col] == 0)
                    {
                        linesFlag = false;
                        break;
                    }
                }
                if (linesFlag == true)
                {
                    nLines++;
                    field.ClearRow(row);
                    for (int r = row - 1; r >= 0; r--)
                        field.DownRow(r);
                }
            }

            return nLines;
        }

        private void CreateBlocks()
        {
            blocks[0] = new BlockO(field);
            blocks[1] = new BlockO(field);
            blocks[2] = new BlockI(State.Gorizontal, field);
            blocks[3] = new BlockI(State.Vertical, field);
            blocks[4] = new BlockS(State.Gorizontal, field);
            blocks[5] = new BlockS(State.Vertical, field);
            blocks[6] = new BlockZ(State.Gorizontal, field);
            blocks[7] = new BlockZ(State.Vertical, field);
            blocks[8] = new BlockL(State.GorizontalDown, field);
            blocks[9] = new BlockL(State.GorizontalUp, field);
            blocks[10] = new BlockL(State.VerticalRight, field);
            blocks[11] = new BlockL(State.VerticalLeft, field);
            blocks[12] = new BlockJ(State.GorizontalDown, field);
            blocks[13] = new BlockJ(State.GorizontalUp, field);
            blocks[14] = new BlockJ(State.VerticalRight, field);
            blocks[15] = new BlockJ(State.VerticalLeft, field);
            blocks[16] = new BlockT(State.GorizontalDown, field);
            blocks[17] = new BlockT(State.GorizontalUp, field);
            blocks[18] = new BlockT(State.VerticalRight, field);
            blocks[19] = new BlockT(State.VerticalLeft, field);
        }

        private void StopGame(string message)
        {
            string _message = String.Format("Линии: {0}\nУровень: {1}\n" +
                            message, nLines, level);
            MessageBox.Show(message);
            gameStatus = GameStatus.Stop;
            field.Clear();
            myPanel.Invalidate();
            gameThread.Abort();
        }

        private void SaveGame()
        {
            string filePath = Application.StartupPath + "\\data.dat";
            FileStream fileStream = File.Create(filePath);

            BinaryFormatter bf = new BinaryFormatter();

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Opacity = 0;
                this.WindowState = FormWindowState.Normal;
            }

            object[] gameState = { nLines, level, speed, levelStep, 
                                   block, field, gameStatus, myPanel.Size, 
                                   this.ClientSize, this.Location, this.Size, this.WindowState,
                                   tempLocation, tempSize, tempWindowStatus, tempWindowState, 
                                   windowStatus, tsmiFullScreen.Text};            

            bf.Serialize(fileStream, gameState);
            fileStream.Close();
        }

        private void LoadGame()
        {
            FileStream fileStream = null;
            
            try
            {
                string filePath = Application.StartupPath + "\\data.dat";

                FileInfo fi = new FileInfo(filePath);
                if (fi.Exists == false)
                    return;

                fileStream = File.OpenRead(filePath);
                BinaryFormatter bf = new BinaryFormatter();

                object[] gameState = (object[])bf.Deserialize(fileStream);

                nLines = (int)gameState[0];
                level = (int)gameState[1];
                speed = (int)gameState[2];
                levelStep = (int)gameState[3];

                block = (Block)gameState[4];
                field = (Field)gameState[5];
                gameStatus = (GameStatus)gameState[6];  
                myPanel.Size = (Size)gameState[7];

                this.ClientSize = (Size)gameState[8];
                this.Location = (Point)gameState[9];
                this.Size = (Size)gameState[10];  
                this.WindowState = (FormWindowState)gameState[11];

                tempLocation = (Point)gameState[12];
                tempSize = (Size)gameState[13];
                tempWindowStatus = (WindowStatus)gameState[14];
                tempWindowState = (FormWindowState)gameState[15];

                windowStatus = (WindowStatus)gameState[16];
                tsmiFullScreen.Text = (string)gameState[17];

                if (windowStatus == WindowStatus.FullScreen)
                    SetFullScreen();  

                SetInformation();

                if (gameStatus != GameStatus.Stop)
                {
                    gameThread = new Thread(Play);
                    gameThread.Start();
                }

                if (gameStatus == GameStatus.Pause)
                {
                    gameThread.Suspend();
                    tsmiPause.Text = "Продолжить";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        private void Exit()
        {
            if (gameThread != null)
            {
                if (gameThread.ThreadState == ThreadState.Suspended)
                    gameThread.Resume();

                gameThread.Abort();
            }

            if (pauseThread != null)
            {
                if (pauseThread.ThreadState == ThreadState.Suspended)
                    pauseThread.Resume();

                pauseThread.Abort();
            }
            SaveGame();

            Application.Exit();
        }

        private void Pause()
        {
            Field tempField = new Field();

            while (true)
            {
                tempField = field;
                field.Clear();
                myPanel.Invalidate();
                Thread.Sleep(1000);
                field = tempField;
                myPanel.Invalidate();
                Thread.Sleep(1000);
            }
        }

        private void SetFullScreen()
        {   
            windowStatus = WindowStatus.FullScreen;

            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.None;
            tsmiFullScreen.Text = "Окно";

            this.Location = new Point(0, 0);
            this.Height = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
            this.Width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;            
        }

        private void SetInWindow()
        {            
            this.FormBorderStyle = FormBorderStyle.Sizable;
            tsmiFullScreen.Text = "Полный экран";

            windowStatus = tempWindowStatus;
            this.Location = tempLocation;
            this.Size = tempSize;
            this.WindowState = tempWindowState;
        }
               
        private void myPanel_Paint(object sender, PaintEventArgs e)
        {
            SetInformation();
            g = e.Graphics;

            myPanel.Location = new Point((this.ClientSize.Width - myPanel.Width) / 2,
            (this.ClientSize.Height - myPanel.Height) / 2 + 1);

            field.Draw(g, myPanel.Size);
        }
        
        private void MainForm_Resize(object sender, EventArgs e)
        {   
            int difWidth = 0;
            int difHeight = 0;

            int width = 0;
            int height = 0;            

            myPanel.Invalidate();            

            difWidth = this.ClientSize.Width - myPanel.Width;    
            difHeight = this.ClientSize.Height - menuStrip1.Height - ssInfo.Height - myPanel.Height;           
            
            if (difWidth > difHeight)
            {
                width = (int)((double)(myPanel.Height + difHeight) / (double)field.Height * (double)field.Width) + 1;
                  
                height = myPanel.Height + difHeight;                

                myPanel.Size = new Size(width, height);
            }
            else if (difWidth < difHeight)
            {            
                height = (int)((double)(myPanel.Width + difWidth) / (double)field.Width * field.Height) + 1;

                if (height > this.ClientSize.Height - menuStrip1.Height - ssInfo.Height)
                {
                    height = this.ClientSize.Height - menuStrip1.Height - ssInfo.Height;
                    difHeight = this.ClientSize.Height - menuStrip1.Height - ssInfo.Height - height;
                    width = (int)((double)(height + difHeight) / (double)field.Height * (double)field.Width) + 1;
                    myPanel.Size = new Size(width, height);
                }
                else
                    myPanel.Size = new Size(myPanel.Width + difWidth, height);                 
            }
            else
            {
                myPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - menuStrip1.Height - ssInfo.Height);               
            }
        }

       
        enum GameStatus
        {
            Play,
            Pause,
            Stop
        }
               
        enum WindowStatus
        {
            FullScreen,
            MaximumWindow,
            InWindow
        }

        Point tempLocation;
        Size tempSize;
        WindowStatus tempWindowStatus;
        FormWindowState tempWindowState;

        WindowStatus windowStatus = WindowStatus.InWindow;
    }
}
