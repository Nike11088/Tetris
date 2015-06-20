using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Tetris
{
    [Serializable]
    class Field
    {
        private int[,] field;  // Игровое поле
                              // Значение элемента массива:
                              // 0 - клетка свободна
                              // 1 - клетка занята       
           
        public int[,] GameField 
        { 
            get 
            {
                return field;
            }

            set
            {
                field = value;
            }
        }
        
        /// <summary>       
        /// Возвращает или устанавливает количество клеток по горизонтали
        /// </summary>    
        public int Width { get; private set; }

        /// <summary>       
        /// Возвращает или устанавливает количество клеток по вертикали
        /// </summary>  
        public int Height { get; private set; }               
        
        /// <summary>
        /// Доступ к клетке поля
        /// </summary>
        /*public Cell Cell
        {
            get { return cell; }
            set { cell = value; }
        }*/

        public Cell[,] Cells
        {
            get
            {
                return cells;
            }

            set
            {
                cells = value;
            }
        }

        private Cell[,] cells; // Клетка поля                     
        
        public Field() : this(15, 10) { }
        public Field(int height, int width) : this(height, width, 25, 25) { }
        public Field(int height, int width, int cellHeight, int cellWidth)
        {   
            Width = width;
            Height = height;

            cells = new Cell[Height, Width];
            field = new int[Height, Width]; 

            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++)
                {
                    cells[row, col] = new Cell(cellWidth, cellHeight);                    
                }          
        }

        /// <summary>
        /// Рисует игровое поле
        /// </summary>
        public void Draw(Graphics g, Size size)
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                    cells[row, col].Width = size.Width / Width;

                for (int col = 0; col < size.Width % Width - 1; col++)
                    cells[row, col].Width++;
            }

            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                    cells[row, col].Height = size.Height / Height;

                for (int row = 0; row < size.Height % Height - 1; row++)
                    cells[row, col].Height++;
            }

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    DrawCell(g, row, col, size);
                }
            }
        }  

        int xIncrease = 0;
        int yIncrease = 0;
        // Рисует клетку
        private void DrawCell(Graphics g, int row, int col, Size size)
        {
            int x, y;    // Координаты левого верхнего угла клетки   

            Cell cell = cells[row, col];         

            if (col != 0 && cell.Width < cells[row, col - 1].Width)
            {
                xIncrease = size.Width % Width - 1;
            }
            else if (col == 0)
                xIncrease = 0;

            x = col * cell.Width + xIncrease;

            if (row != 0 && cell.Height < cells[row - 1, col].Height)
            {
                yIncrease = size.Height % Height - 1;
            }
            else if (row == 0)
                yIncrease = 0;

            y = row * cell.Height + yIncrease;

            int i = 0;
            Brush[] brush = { Brushes.Green, Brushes.Red, Brushes.Orange,
                            Brushes.Blue, Brushes.Lime, Brushes.Purple,
                            Brushes.DarkRed};

            // Занятые клетки - цветные
            switch (field[row, col])
            {
                case 1:
                    i = 0;
                    break;
                case 2:
                    i = 1;
                    break;
                case 3:
                    i = 2;
                    break;
                case 4:
                    i = 3;
                    break;
                case 5:
                    i = 4;
                    break;
                case 6:
                    i = 5;
                    break;
                case 7:
                    i = 6;
                    break;
            }

            g.FillRectangle(brush[i], x, y, cell.Width, cell.Height); 

            // Свободные клетки - SystemBrushes.Control
            if (field[row, col] == 0)
                g.FillRectangle(SystemBrushes.Control, x, y, cell.Width, cell.Height);
                 
            // Рисует границу клетки
            g.DrawRectangle(Pens.White, x, y, cell.Width, cell.Height);
        }

        /// <summary>
        /// Очищает игровое поле
        /// </summary>
        public void Clear()
        {            
            for (int row = 0; row < Height; row++)
                for (int col = 0; col < Width; col++)
                    field[row, col] = 0;
        }        

        /// <summary>
        /// Очищает строку
        /// </summary>
        public void ClearRow(int row)
        {
            for (int col = 0; col < Width; col++)
                field[row, col] = 0;
        }

        /// <summary>
        /// Перемещат строку на одну позицию вниз
        /// </summary>
        public void DownRow(int row)
        {
            if (row >= Height - 1)
                return;

            for (int col = 0; col < Width; col++)
                field[row + 1, col] = field[row, col];                                  
        }

        /// <summary>
        /// Возвращает состояние игрового поля
        /// </summary>
        public string GetState()
        {
            string state = "";
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    state += String.Format(field[row, col].ToString() + " ");
                }
                state += Environment.NewLine.ToString();
            }

            return state;
        }
    }

    [Serializable] 
    class Cell
    {
        /// <summary>
        /// Возвращает или устанавливает ширину клетки
        /// </summary>
        public int Width { get; set; } 

        /// <summary>
        /// Возвращает или устанавливает высоту клетки
        /// </summary>
        public int Height { get; set; }

        public Cell()
        {

        }

        public Cell(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
