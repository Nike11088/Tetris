using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Tetris
{
    [Serializable]
    class Block
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Ground { get; set; }
        public State State { get; set; }
        public int Steps { get; set; }

        protected Field field = new Field();
        protected int rotate;

        public Block()
        {            
            State = State.Gorizontal;
            rotate = 3;
            Ground = false;
        }
       
        public Block(Field field) : 
            this()
        {
            this.field = field;            
        }

        virtual public bool Create()
        {
            if (CanCreate() == true)
            {
                Ground = false;
                Steps = 0;
                CreateBlock();
                return true;
            }
            else
                return false;
        }

        virtual public bool Create(int x, int y)
        {
            X = x;
            Y = y;

            if (CanCreate() == true)
            {
                Steps = 0;
                Ground = false;
                CreateBlock();
                return true;
            }
            else
                return false;
        }        

        virtual public bool MoveDown()
        {
            if (CanMoveDown() == false)
            {                
                Ground = true;
                return false;
            }

            DeleteBlock();
            Y++;
            CreateBlock();
            Steps++;

            return true;
        }

        virtual public bool MoveRight()
        {
            if (CanMoveRight() == false)
                return false;

            DeleteBlock();
            X++;
            CreateBlock();

            return true;
        }

        virtual public bool MoveLeft()
        {
            if (CanMoveLeft() == false)
                return false;

            DeleteBlock();
            X--;
            CreateBlock();

            return true;
        }        

        virtual public bool Rotate()
        { return false; }

        virtual protected bool CanCreate()
        {
            return false;
        }

        virtual public void CreateBlock()
        { }

        virtual protected void DeleteBlock()
        { }

        virtual public bool CanMoveDown()
        {  
            return false;
        }

        virtual protected bool CanMoveRight()
        {
            return false;
        }

        virtual protected bool CanMoveLeft()
        {   
            return false;
        }
    }

    [Serializable]
    class BlockO : Block
    {
        public BlockO() : 
            base()
        { }
        public BlockO(Field field) :
            this()
        { 
            this.field = field;
        }

        override public bool Create()
        {
            X = 4;
            Y = 0;

            return base.Create();
        }

        override public void CreateBlock()
        {
            for (int row = Y; row < Y + 2; row++)
                for (int col = X; col < X + 2; col++)
                    field.GameField[row, col] = 1;
        }

        override protected void DeleteBlock()
        {
            for (int row = Y; row < Y + 2; row++)
                for (int col = X; col < X + 2; col++)
                    field.GameField[row, col] = 0;
        }

        override protected bool CanCreate()
        {
            for (int row = Y; row < Y + 2; row++)
                for (int col = X; col < X + 2; col++)
                    if (field.GameField[row, col] != 0)
                        return false;

            return true;
        }

        override public bool CanMoveDown()
        {
            if (Y == field.Height - 2)
            {
                return false;
            }

            int tempY = Y;
            tempY += 2;
            for (int col = X; col < X + 2; col++)
            {
                if (field.GameField[tempY, col] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        override protected bool CanMoveRight()
        {
            if (X + 2 == field.Width)
                return false;

            int tempX = X;
            tempX += 2;
            for (int row = Y; row < Y + 2; row++)
            {
                if (field.GameField[row, tempX] != 0)
                    return false;
            }

            return true;
        }

        override protected bool CanMoveLeft()
        {
            if (X == 0)
                return false;
                        
            for (int row = Y, col = X - 1; row < Y + 2; row++)
            {
                if (field.GameField[row, col] != 0)
                    return false;
            }

            return true;
        }
    }

    [Serializable]
    class BlockI : Block
    {
        public BlockI() :
            base()
        {            
            rotate = 3;
        }
        
        public BlockI(State state, Field field) :
            this()
        {
            this.field = field;

            State = state;

            if (state == State.Gorizontal)
            {
                rotate = 3;                
            }
            else
            {
                rotate = 0;                
            }
        }

        override public bool Create()
        {
            if (State == State.Gorizontal)
            {
                X = 3;
                Y = 0;
            }
            else
            {
                X = 4;
                Y = 0;
            }

            return base.Create();           
        }        
             
        override public bool Rotate()
        {
            if (CanRotate() == false)
                return false;

            DeleteBlock(); 

            if (State == State.Gorizontal)
            {
                rotate = 0;
                State = State.Vertical;
                X = X + 1;
                Y = Y - 2;
            }
            else
            {
                rotate = 3;
                State = State.Gorizontal;
                X = X - 1;
                Y = Y + 2;
            }  

            CreateBlock(); 

            return true;
        }

        override protected bool CanCreate()
        {
            if (State == State.Gorizontal)
            {
                for (int row = Y; row < Y + 1; row++)
                    for (int col = X; col < X + 4; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
            }
            else
            {
                for (int row = Y; row < Y + 4; row++)
                    for (int col = X; col < X + 1; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
            }
            return true;
        }

        private bool CanRotate()
        {
            int tempX;
            int tempY;

            if (State == State.Gorizontal)
            {
                tempX = X + 1;
                tempY = Y - 2;
                for (int row = tempY; row < tempY + 1 + rotate; row++)
                    for (int col = tempX; col < tempX + 4 - rotate; col++)
                    {
                        if (row >= field.Height || row < 0 ||
                            col >= field.Width || col < 0)
                            return false;
                        if (X + 1 == col && Y == row)
                            continue;
                        if (field.GameField[row, col] != 0)
                            return false;                        
                    }

                if (field.GameField[Y - 1, X + 2] != 0 ||
                   field.GameField[Y - 1, X + 3] != 0 ||
                   field.GameField[Y + 1, X] != 0)
                        return false;                
            }
            else
            {
                tempX = X - 1;
                tempY = Y + 2;
                for (int row = tempY; row < tempY + 1 + rotate; row++)
                    for (int col = tempX; col < tempX + 4 - rotate; col++)
                    {
                        if (row >= field.Height || row < 0 ||
                            col >= field.Width || col < 0)
                            return false;
                        if (X == col && Y + 2 == row)
                            continue;
                        if (field.GameField[row, col] != 0)
                            return false;
                    }

                if (field.GameField[Y, X + 1] != 0 ||
                   field.GameField[Y + 1, X + 1] != 0 ||
                   field.GameField[Y + 3, X - 1] != 0)
                    return false;
            }

            return true;
        }

        override public bool CanMoveDown()
        {
            if (Y == field.Height - 4 + rotate)
                return false;

            int tempY = Y;
            tempY = tempY + 4 - rotate;

            for (int col = X; col < X + 1 + rotate; col++)
            {
                if (field.GameField[tempY, col] != 0)
                    return false;
            } 

            return true;
        }

        override protected bool CanMoveRight()
        {
            if (X + 1 + rotate == field.Width)
                return false;

            int tempX = X;
            tempX = tempX + 1 + rotate;
            for (int row = Y; row < Y + 4 - rotate; row++)
            {
                if (field.GameField[row, tempX] != 0)
                    return false;
            }

            return true;
        }

        override protected bool CanMoveLeft()
        {
            if (X == 0)
                return false;

            int tempX = X;
            tempX -= 1;
            for (int row = Y; row < Y + 4 - rotate; row++)
            {
                if (field.GameField[row, tempX] != 0)
                    return false;
            }

            return true;
        }

        override public void CreateBlock()
        {
            if (State == State.Gorizontal)
            {
                for (int row = Y; row < Y + 1; row++)
                    for (int col = X; col < X + 4; col++)
                        field.GameField[row, col] = 2;
            }
            else
            {
                for (int row = Y; row < Y + 4; row++)
                    for (int col = X; col < X + 1; col++)
                        field.GameField[row, col] = 2;
            }
        }

        override protected void DeleteBlock()
        {
            if (State == State.Gorizontal)
            {
                for (int row = Y; row < Y + 1; row++)
                    for (int col = X; col < X + 4; col++)
                        field.GameField[row, col] = 0;
            }
            else
            {
                for (int row = Y; row < Y + 4; row++)
                    for (int col = X; col < X + 1; col++)
                        field.GameField[row, col] = 0;
            }
        }        
    }

    [Serializable]
    class BlockS : Block
    {   
        public BlockS() :
            base()
        { }
        public BlockS(State state, Field field) :
            this()
        {
            this.field = field;

            State = state;
        }

        override public bool Create()
        {
            if (State == State.Gorizontal)
            {
                X = 3;
                Y = 0;
            }
            else
            {
                X = 4;
                Y = 0;
            }

            return base.Create();
        }
           
        override public bool Rotate()
        {
            if (CanRotate() == false)
                return false;            

            DeleteBlock();
            if (State == State.Gorizontal)
                State = State.Vertical;
            else
                State = State.Gorizontal;
            CreateBlock();
            return true;
        }

        override protected bool CanCreate()
        {
            if (State == State.Gorizontal)
            {
                for (int col = X + 1, row = Y; col < X + 3; col++)
                    if (field.GameField[row, col] != 0)
                        return false;

                for (int col = X, row = Y + 1; col < X + 2; col++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }
            else
            {
                for (int row = Y, col = X; row < Y + 2; row++)
                    if (field.GameField[row, col] != 0)
                        return false;

                for (int row = Y + 1, col = X + 1; row < Y + 3; row++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }

            return true;
        }

        private bool CanRotate()
        {
            if (State == State.Gorizontal)
            {  
                if ((Y - 1) < 0 || (Y + 2) >= field.Height ||
                    field.GameField[Y - 1, X + 2] != 0 || 
                    field.GameField[Y - 1, X + 1] != 0 ||
                    field.GameField[Y, X] != 0 ||
                    field.GameField[Y + 2, X] != 0 ||
                    field.GameField[Y + 2, X + 1] != 0)
                    return false;
            }
            else
            {
                if ((X + 2) >= field.Width ||
                    field.GameField[Y, X + 2] != 0 ||
                    field.GameField[Y, X + 1] != 0 ||
                    field.GameField[Y + 2, X] != 0)
                    return false;
            }

            return true;
        }

        override public bool CanMoveDown()
        {
            if (State == State.Gorizontal)
            {
                if (Y == field.Height - 2)
                    return false;

                for (int col = X, row = Y + 2; col < X + 2; col++)
                    if (field.GameField[row, col] != 0)
                        return false;

                if (field.GameField[Y + 1, X + 2] != 0)
                    return false;
            }
            else
            {
                if (Y == field.Height - 3)
                    return false;

                for (int col = X, row = Y + 2; col < X + 2; col++, row++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }

            return true;
        }

        override protected bool CanMoveRight()
        {
            if (State == State.Gorizontal)
            {
                if (X + 3 == field.Width)
                    return false;

                for (int col = X + 3, row = Y; row < Y + 2; col--, row++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }
            else
            {
                if (X + 2 == field.Width)
                    return false;

                for (int col = X + 2, row = Y + 1; row < Y + 3; row++)
                    if (field.GameField[row, col] != 0)
                        return false;

                if (field.GameField[Y, X + 1] != 0)
                    return false;
            }

            return true;
        }

        override protected bool CanMoveLeft()
        {
            if (State == State.Gorizontal)
            {
                if (X == 0)
                    return false;

                for (int col = X, row = Y; row < Y + 2; col--, row++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }
            else
            {
                if (X == 0)
                    return false;

                for (int col = X - 1, row = Y; row < Y + 2; row++)
                    if (field.GameField[row, col] != 0)
                        return false;

                if (field.GameField[Y + 2, X] != 0)
                    return false;
            }

            return true;
        }

        override public void CreateBlock()
        {
            if (State == State.Gorizontal)
            {
                for (int col = X + 1, row = Y; col < X + 3; col++)
                    field.GameField[row, col] = 3;

                for (int col = X, row = Y + 1; col < X + 2; col++)
                    field.GameField[row, col] = 3;
            }
            else
            {
                for (int row = Y, col = X; row < Y + 2; row++)
                    field.GameField[row, col] = 3;

                for (int row = Y + 1, col = X + 1; row < Y + 3; row++)
                    field.GameField[row, col] = 3;
            }
        }

        override protected void DeleteBlock()
        {
            if (State == State.Gorizontal)
            {
                for (int col = X + 1, row = Y; col < X + 3; col++)
                    field.GameField[row, col] = 0;
                for (int col = X, row = Y + 1; col < X + 2; col++)
                    field.GameField[row, col] = 0;
            }
            else
            {
                for (int row = Y, col = X; row < Y + 2; row++)
                    field.GameField[row, col] = 0;
                for (int row = Y + 1, col = X + 1; row < Y + 3; row++)
                    field.GameField[row, col] = 0;
            }
        }
    }

    [Serializable]
    class BlockZ : Block
    {
        public BlockZ() :
            base()
        { }
        public BlockZ(State state, Field field) :
            this()
        {
            this.field = field;

            State = state;            
        }

        override public bool Create()
        {
            if (State == State.Gorizontal)
            {
                X = 3;
                Y = 0;
            }
            else
            {
                X = 4;
                Y = 0;
            }

            return base.Create();
        }
        
        override public bool Rotate()
        {
            if (CanRotate() == false)
                return false;

            DeleteBlock();
            if (State == State.Gorizontal)
            {
                State = State.Vertical;
                X++;
            }
            else
            {
                State = State.Gorizontal;
                X--;
            }
            CreateBlock();
            return true;
        }

        override protected bool CanCreate()
        {
            if (State == State.Gorizontal)
            {
                for (int col = X, row = Y; col < X + 2; col++)
                    if (field.GameField[row, col] != 0)
                        return false;
                for (int col = X + 1, row = Y + 1; col < X + 3; col++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }
            else
            {
                for (int row = Y + 1, col = X; row < Y + 3; row++)
                    if (field.GameField[row, col] != 0)
                        return false;
                for (int row = Y, col = X + 1; row < Y + 2; row++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }

            return true;
        }

        private bool CanRotate()
        {
            if (State == State.Gorizontal)
            {
                if ((Y - 1) < 0 || (Y + 2) >= field.Height ||
                    field.GameField[Y - 1, X] != 0 ||
                    field.GameField[Y - 1, X + 1] != 0 ||
                    field.GameField[Y, X + 2] != 0 ||
                    field.GameField[Y + 2, X + 1] != 0 ||
                    field.GameField[Y + 2, X + 2] != 0)
                    return false;
            }
            else
            {
                if (X == 0 || 
                    field.GameField[Y, X - 1] != 0 ||
                    field.GameField[Y, X] != 0 ||
                    field.GameField[Y + 2, X + 1] != 0)
                    return false;
            }

            return true;
        }

        override public bool CanMoveDown()
        {
            if (State == State.Gorizontal)
            {
                if (Y == field.Height - 2)
                    return false;

                for (int col = X + 1, row = Y + 2; col < X + 3; col++)
                    if (field.GameField[row, col] != 0)
                        return false;

                if (field.GameField[Y + 1, X] != 0)
                    return false;
            }
            else
            {
                if (Y == field.Height - 3)
                    return false;

                for (int col = X, row = Y + 3; col < X + 2; col++, row--)
                    if (field.GameField[row, col] != 0)
                        return false;
            }

            return true;
        }

        override protected bool CanMoveRight()
        {
            if (State == State.Gorizontal)
            {
                if (X + 3 == field.Width)
                    return false;

                for (int col = X + 2, row = Y; row < Y + 2; col++, row++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }
            else
            {
                if (X + 2 == field.Width)
                    return false;

                for (int col = X + 2, row = Y; row < Y + 2; row++)
                    if (field.GameField[row, col] != 0)
                        return false;

                if (field.GameField[Y + 2, X + 1] != 0)
                    return false;
            }

            return true;
        }

        override protected bool CanMoveLeft()
        {
            if (State == State.Gorizontal)
            {
                if (X == 0)
                    return false;

                for (int col = X - 1, row = Y; row < Y + 2; col++, row++)
                    if (field.GameField[row, col] != 0)
                        return false;
            }
            else
            {
                if (X == 0)
                    return false;

                for (int col = X - 1, row = Y + 1; row < Y + 3; row++)
                    if (field.GameField[row, col] != 0)
                        return false;

                if (field.GameField[Y, X] != 0)
                    return false;
            }

            return true;
        }

        override public void CreateBlock()
        {
            if (State == State.Gorizontal)
            {
                for (int col = X, row = Y; col < X + 2; col++)
                    field.GameField[row, col] = 4;
                for (int col = X + 1, row = Y + 1; col < X + 3; col++)
                    field.GameField[row, col] = 4;
            }
            else
            {
                for (int row = Y + 1, col = X; row < Y + 3; row++)
                    field.GameField[row, col] = 4;
                for (int row = Y, col = X + 1; row < Y + 2; row++)
                    field.GameField[row, col] = 4;
            }
        }

        override protected void DeleteBlock()
        {
            if (State == State.Gorizontal)
            {
                for (int col = X, row = Y; col < X + 2; col++)
                    field.GameField[row, col] = 0;
                for (int col = X + 1, row = Y + 1; col < X + 3; col++)
                    field.GameField[row, col] = 0;

            }
            else
            {
                for (int row = Y + 1, col = X; row < Y + 3; row++)
                    field.GameField[row, col] = 0;
                for (int row = Y, col = X + 1; row < Y + 2; row++)
                    field.GameField[row, col] = 0;
            }
        }
    }

    [Serializable]
    class BlockL : Block
    {
        public BlockL() :
            base()
        {
            State = State.GorizontalDown;
        }
        public BlockL(State state, Field field) :
            this()
        {
            this.field = field;

            State = state;
        }

        override public bool Create()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    X = 3;
                    Y = 0;
                    break;
                case State.GorizontalUp:
                    X = 3;
                    Y = 0;
                    break;
                case State.VerticalRight:
                    X = 4;
                    Y = 0;
                    break;
                case State.VerticalLeft:
                    X = 4;
                    Y = 0;
                    break;
            }

            return base.Create();
        }        

        override public bool Rotate()
        {
            if (CanRotate() == false)
                return false;

            DeleteBlock();
            switch (State)
            {
                case State.GorizontalDown:                    
                    Y--;
                    State = State.VerticalLeft;
                    break;
                case State.GorizontalUp:
                    X++;
                    State = State.VerticalRight;
                    break;
                case State.VerticalRight:
                    X--;
                    Y++;
                    State = State.GorizontalDown;
                    break;
                case State.VerticalLeft:                    
                    State = State.GorizontalUp;
                    break;
            }
            CreateBlock();
            return true;
        }

        override protected bool CanCreate()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 1, X] != 0)
                        return false;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y, X + 2] != 0)
                        return false;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 2, X + 1] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y, X] != 0)
                        return false;
                    break;
            }
            return true;
        }

        private bool CanRotate()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if (Y - 1 < 0 ||
                    field.GameField[Y - 1, X] != 0 ||
                    field.GameField[Y - 1, X + 1] != 0 ||
                    field.GameField[Y + 1, X + 1] != 0 ||
                    field.GameField[Y + 1, X + 2] != 0)
                        return false;
                    break;
                case State.GorizontalUp:
                    if (Y + 2 >= field.Height ||
                    field.GameField[Y, X] != 0 ||
                    field.GameField[Y, X + 1] != 0 ||
                    field.GameField[Y + 2, X + 1] != 0 ||
                    field.GameField[Y + 2, X + 2] != 0)
                        return false;
                    break;
                case State.VerticalRight:
                    if (X == 0 ||
                        field.GameField[Y, X + 1] != 0 ||
                        field.GameField[Y + 1, X - 1] != 0 ||
                        field.GameField[Y + 1, X + 1] != 0 ||
                        field.GameField[Y + 2, X - 1] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    if (X == field.Width - 2 ||
                        field.GameField[Y, X + 2] != 0 ||
                        field.GameField[Y + 1, X] != 0 ||
                        field.GameField[Y + 1, X + 2] != 0 ||
                        field.GameField[Y + 2, X] != 0)
                        return false;
                    break;
            }

            return true;

            if (State == State.Gorizontal)
            {
                if ((Y - 1) < 0 || (Y + 2) >= field.Height ||
                    field.GameField[Y - 1, X] != 0 ||
                    field.GameField[Y - 1, X + 1] != 0 ||
                    field.GameField[Y, X + 2] != 0 ||
                    field.GameField[Y + 2, X + 1] != 0 ||
                    field.GameField[Y + 2, X + 2] != 0)
                    return false;
            }
            else
            {
                if (X == 0 || (X + 2) >= field.Width ||
                    field.GameField[Y, X - 1] != 0 ||
                    field.GameField[Y, X] != 0 ||
                    field.GameField[Y + 2, X + 1] != 0)
                    return false;
            }

            return true;
        }

        override public bool CanMoveDown()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if (Y == field.Height - 2)
                        return false;
                    for (int col = X + 1, row = Y + 1; col < X + 3; col++)
                        if (field.GameField[row, col] != 0 ||
                            field.GameField[Y + 2, X] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    if (Y == field.Height - 2)
                        return false;
                    for (int col = X, row = Y + 2; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    if (Y == field.Height - 3)
                        return false;
                    for (int col = X, row = Y + 3; col < X + 2; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalLeft:
                    if (Y == field.Height - 3)
                        return false;
                    if (field.GameField[Y + 1, X] != 0 ||
                        field.GameField[Y + 3, X + 1] != 0)
                        return false;                    
                    break;
            }
            return true;
        }

        override protected bool CanMoveRight()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if (X == field.Width - 3)
                        return false;
                    if (field.GameField[Y, X + 3] != 0 ||
                        field.GameField[Y + 1, X + 1] != 0)
                        return false;
                    break;
                case State.GorizontalUp:
                    if (X == field.Width - 3)
                        return false;
                    for (int col = X + 3, row = Y; row < Y + 2; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    if (X == field.Width - 2)
                        return false;
                    for (int col = X + 1, row = Y; row < Y + 2; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 2, X + 2] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    if (X == field.Width - 2)
                        return false;
                    for (int col = X + 2, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
            }
            return true;
        }

        override protected bool CanMoveLeft()
        {
            if (X == 0)
                return false;

            switch (State)
            {
                case State.GorizontalDown:                    
                    for (int col = X - 1, row = Y; row < Y + 2; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    if (field.GameField[Y, X + 1] != 0 ||
                        field.GameField[Y + 1, X - 1] != 0)
                        return false;
                    break;
                case State.VerticalRight:
                    for (int col = X - 1, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalLeft:
                    for (int col = X, row = Y + 1; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y, X - 1] != 0)
                        return false;
                    break;
            }
            return true;
        }

        override public void CreateBlock()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        field.GameField[row, col] = 5;
                    field.GameField[Y + 1, X] = 5;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        field.GameField[row, col] = 5;
                    field.GameField[Y, X + 2] = 5;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 5;
                    field.GameField[Y + 2, X + 1] = 5;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 5;
                    field.GameField[Y, X] = 5;
                    break;
            }
        }

        override protected void DeleteBlock()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y + 1, X] = 0;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y, X + 2] = 0;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y + 2, X + 1] = 0;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y, X] = 0;
                    break;
            }
        }
    }

    [Serializable]
    class BlockJ : Block
    {
        public BlockJ() :
            base()
        {
            State = State.GorizontalDown;
        }
        public BlockJ(State state, Field field) :
            this()
        {
            this.field = field;

            State = state;
        }

        override public bool Create()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    X = 3;
                    Y = 0;
                    break;
                case State.GorizontalUp:
                    X = 3;
                    Y = 0;
                    break;
                case State.VerticalRight:
                    X = 4;
                    Y = 0;
                    break;
                case State.VerticalLeft:
                    X = 4;
                    Y = 0;
                    break;
            }

            return base.Create();
        }

        override public bool Rotate()
        {
            if (CanRotate() == false)
                return false;

            DeleteBlock();
            switch (State)
            {
                case State.GorizontalDown:
                    Y--;
                    State = State.VerticalLeft;
                    break;
                case State.GorizontalUp:
                    X++;
                    State = State.VerticalRight;
                    break;
                case State.VerticalRight:
                    X--;
                    Y++;
                    State = State.GorizontalDown;
                    break;
                case State.VerticalLeft:
                    State = State.GorizontalUp;
                    break;
            }
            CreateBlock();
            return true;
        }

        override protected bool CanCreate()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 1, X + 2] != 0)
                        return false;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y, X] != 0)
                        return false;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y, X + 1] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 2, X] != 0)
                        return false;
                    break;
            }

            return true;
        }

        private bool CanRotate()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if ((Y - 1) < 0 ||
                    field.GameField[Y - 1, X] != 0 ||
                    field.GameField[Y - 1, X + 1] != 0 ||
                    field.GameField[Y + 1, X] != 0 ||
                    field.GameField[Y + 1, X + 1] != 0)
                        return false;
                    break;
                case State.GorizontalUp:
                    if (Y == field.Height - 2 ||
                        field.GameField[Y, X + 1] != 0 ||
                        field.GameField[Y, X + 2] != 0 ||
                        field.GameField[Y + 2, X + 1] != 0 ||
                        field.GameField[Y + 2, X + 2] != 0)
                        return false;
                    break;
                case State.VerticalRight:
                    if (X == 0 ||
                        field.GameField[Y + 1, X - 1] != 0 ||
                        field.GameField[Y + 2, X - 1] != 0 ||
                        field.GameField[Y + 1, X + 1] != 0 ||
                        field.GameField[Y + 2, X + 1] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    if (X == field.Width - 2 ||
                        field.GameField[Y, X] != 0 ||
                        field.GameField[Y + 1, X] != 0 ||
                        field.GameField[Y, X + 2] != 0 ||
                        field.GameField[Y + 1, X + 2] != 0)
                        return false;
                    break;
            }

            return true;            
        }

        override public bool CanMoveDown()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if (Y == field.Height - 2)
                        return false;
                    for (int col = X, row = Y + 1; col < X + 2; col++)
                        if (field.GameField[row, col] != 0 ||
                            field.GameField[Y + 2, X + 2] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    if (Y == field.Height - 2)
                        return false;
                    for (int col = X, row = Y + 2; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    if (Y == field.Height - 3)
                        return false;
                    for (int col = X, row = Y + 3; col < X + 2; col++, row -= 2)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalLeft:
                    if (Y == field.Height - 3)
                        return false;
                    for (int col = X, row = Y + 3; col < X + 2; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
            }
            return true;
        }

        override protected bool CanMoveRight()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if (X == field.Width - 3)
                        return false;
                    for (int col = X + 3, row = Y; row < Y + 2; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    if (X == field.Width - 3)
                        return false;
                    for (int col = X + 1, row = Y; row < Y + 2; row++, col += 2)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    if (X == field.Width - 2)
                        return false;
                    for (int col = X + 1, row = Y + 1; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y, X + 2] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    if (X == field.Width - 2)
                        return false;
                    for (int col = X + 2, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
            }
            return true;
        }

        override protected bool CanMoveLeft()
        {
            if (X == 0)
                return false;

            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X - 1, row = Y; row < Y + 2; row++, col += 2)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    for (int col = X - 1, row = Y; row < Y + 2; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    for (int col = X - 1, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalLeft:
                    for (int col = X, row = Y; row < Y + 2; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 2, X - 1] != 0)
                        return false;
                    break;
            }
            return true;
        }

        override public void CreateBlock()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        field.GameField[row, col] = 6;
                    field.GameField[Y + 1, X + 2] = 6;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        field.GameField[row, col] = 6;
                    field.GameField[Y, X] = 6;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 6;
                    field.GameField[Y, X + 1] = 6;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 6;
                    field.GameField[Y + 2, X] = 6;
                    break;
            }
        }

        override protected void DeleteBlock()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y + 1, X + 2] = 0;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y, X] = 0;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y, X + 1] = 0;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y + 2, X] = 0;
                    break;
            }
        }
    }

    [Serializable]
    class BlockT : Block
    {
        public BlockT() :
            base()
        {
            State = State.GorizontalDown;
        }
        public BlockT(State state, Field field) :
            this()
        {
            this.field = field;

            State = state;
        }

        override public bool Create()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    X = 3;
                    Y = 0;
                    break;
                case State.GorizontalUp:
                    X = 3;
                    Y = 0;
                    break;
                case State.VerticalRight:
                    X = 4;
                    Y = 0;
                    break;
                case State.VerticalLeft:
                    X = 4;
                    Y = 0;
                    break;
            }

            return base.Create();
        }

        override public bool Rotate()
        {
            if (CanRotate() == false)
                return false;

            DeleteBlock();
            switch (State)
            {
                case State.GorizontalDown:
                    Y--;
                    State = State.VerticalLeft;
                    break;
                case State.GorizontalUp:
                    X++;
                    State = State.VerticalRight;
                    break;
                case State.VerticalRight:
                    X--;
                    Y++;
                    State = State.GorizontalDown;
                    break;
                case State.VerticalLeft:
                    State = State.GorizontalUp;
                    break;
            }
            CreateBlock();
            return true;
        }

        override protected bool CanCreate()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 1, X + 1] != 0)
                        return false;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y, X + 1] != 0)
                        return false;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 1, X + 1] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 1, X] != 0)
                        return false;
                    break;
            }

            return true;
        }

        private bool CanRotate()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if ((Y - 1) < 0 ||
                    field.GameField[Y - 1, X] != 0 ||
                    field.GameField[Y - 1, X + 1] != 0 ||
                    field.GameField[Y + 1, X] != 0 ||
                    field.GameField[Y + 1, X + 2] != 0)
                        return false;
                    break;
                case State.GorizontalUp:
                    if (Y == field.Height - 2 ||
                        field.GameField[Y, X] != 0 ||
                        field.GameField[Y, X + 2] != 0 ||
                        field.GameField[Y + 2, X + 1] != 0 ||
                        field.GameField[Y + 2, X + 2] != 0)
                        return false;
                    break;
                case State.VerticalRight:
                    if (X == 0 ||
                        field.GameField[Y, X + 1] != 0 ||
                        field.GameField[Y + 1, X - 1] != 0 ||
                        field.GameField[Y + 2, X - 1] != 0 ||
                        field.GameField[Y + 2, X + 1] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    if (X == field.Width - 2 ||
                        field.GameField[Y, X] != 0 ||
                        field.GameField[Y, X + 2] != 0 ||
                        field.GameField[Y + 1, X + 2] != 0 ||
                        field.GameField[Y + 2, X] != 0)
                        return false;
                    break;
            }

            return true;
        }

        override public bool CanMoveDown()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if (Y == field.Height - 2)
                        return false;
                    for (int col = X, row = Y + 1; col < X + 2; col++, row++)
                        if (field.GameField[row, col] != 0 ||
                            field.GameField[Y + 1, X + 2] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    if (Y == field.Height - 2)
                        return false;
                    for (int col = X, row = Y + 2; col < X + 3; col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    if (Y == field.Height - 3)
                        return false;
                    for (int col = X, row = Y + 3; col < X + 2; col++, row--)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalLeft:
                    if (Y == field.Height - 3)
                        return false;
                    for (int col = X, row = Y + 2; col < X + 2; col++, row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
            }
            return true;
        }

        override protected bool CanMoveRight()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    if (X == field.Width - 3)
                        return false;
                    for (int col = X + 3, row = Y; row < Y + 2; row++, col--)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    if (X == field.Width - 3)
                        return false;
                    for (int col = X + 2, row = Y; row < Y + 2; row++, col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    if (X == field.Width - 2)
                        return false;
                    for (int col = X + 1, row = Y; row < Y + 2; row++, col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 2, X + 1] != 0)
                        return false;
                    break;
                case State.VerticalLeft:
                    if (X == field.Width - 2)
                        return false;
                    for (int col = X + 2, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
            }
            return true;
        }

        override protected bool CanMoveLeft()
        {
            if (X == 0)
                return false;

            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X - 1, row = Y; row < Y + 2; row++, col++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y; row < Y + 2; row++, col--)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalRight:
                    for (int col = X - 1, row = Y; row < Y + 3; row++)
                        if (field.GameField[row, col] != 0)
                            return false;
                    break;
                case State.VerticalLeft:
                    for (int col = X, row = Y; row < Y + 2; row++, col--)
                        if (field.GameField[row, col] != 0)
                            return false;
                    if (field.GameField[Y + 2, X] != 0)
                        return false;
                    break;
            }
            return true;
        }

        override public void CreateBlock()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        field.GameField[row, col] = 7;
                    field.GameField[Y + 1, X + 1] = 7;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        field.GameField[row, col] = 7;
                    field.GameField[Y, X + 1] = 7;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 7;
                    field.GameField[Y + 1, X + 1] = 7;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 7;
                    field.GameField[Y + 1, X] = 7;
                    break;
            }
        }

        override protected void DeleteBlock()
        {
            switch (State)
            {
                case State.GorizontalDown:
                    for (int col = X, row = Y; col < X + 3; col++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y + 1, X + 1] = 0;
                    break;
                case State.GorizontalUp:
                    for (int col = X, row = Y + 1; col < X + 3; col++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y, X + 1] = 0;
                    break;
                case State.VerticalRight:
                    for (int col = X, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y + 1, X + 1] = 0;
                    break;
                case State.VerticalLeft:
                    for (int col = X + 1, row = Y; row < Y + 3; row++)
                        field.GameField[row, col] = 0;
                    field.GameField[Y + 1, X] = 0;
                    break;
            }
        }
    }

    [Serializable]
    public enum State
    {
        None,
        Vertical,
        Gorizontal,
        VerticalRight,
        VerticalLeft,
        GorizontalUp,
        GorizontalDown
    }    
}
