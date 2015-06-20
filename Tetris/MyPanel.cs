using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Tetris
{
    public class MyPanel : Panel
    {
        public MyPanel()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
