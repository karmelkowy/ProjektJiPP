using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt
{
    public partial class Food : UserControl
    {
        public static int size = 10;
        int x = 0;
        int y = 0;
        public Food(Panel parent, int x, int y)
        {
            InitializeComponent();
            Width = size;
            Height = size;
            Parent = parent;

            this.x = x;
            this.y = y;
            Left = x - size / 2;
            Top = y - size / 2;


        }

        public int getAngle(int ox, int oy)
        {

            return (int)(Math.Atan2(oy-this.y, ox-this.x) * 180 / Math.PI);
        }

        public float GetDist(int ox, int oy)
        {
            return (float)Math.Sqrt(Math.Pow(this.x - ox, 2) + Math.Pow(this.y - oy, 2));
        }

        public bool CheckColide(int ox, int oy, int distance=0)
        {
            return (distance > GetDist(ox, oy));
                    
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);

            SolidBrush brush = new SolidBrush(Color.Red);
            e.Graphics.FillEllipse(brush, 0, 0, (float)size - 1, (float)size - 1);
        }

    }
}
