using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect
{
    public partial class Form1 : Form
    {
        private Core core;
        private Bitmap bitmap;
        private Bitmap fon;
        Graphics graphics;
        BufferedGraphics bg;
        BufferedGraphicsContext bgc;

        public Form1()
        {
            InitializeComponent();
            core = new Core();
            core.NewGame();
            bgc = new BufferedGraphicsContext();
            oldHeight = Height;
            oldWidth = Width;
        }

        private void Draw()
        {
            Core.Mask mask;
            try
            {
                bg.Graphics.Clear(Color.Black);
                for (int i = 0; i < core.width; i++)
                    for (int j = 0; j < core.width; j++)
                    {
                        mask = core[i, j];
                        if (mask.HasFlag(Core.Mask.left))
                            bg.Graphics.FillRectangle(Brushes.White, i * 60, j * 60 + 25, 35, 10);
                        if (mask.HasFlag(Core.Mask.right))
                            bg.Graphics.FillRectangle(Brushes.White, i * 60 + 25, j * 60 + 25, 35, 10);
                        if (mask.HasFlag(Core.Mask.up))
                            bg.Graphics.FillRectangle(Brushes.White, i * 60 + 25, j * 60, 10, 35);
                        if (mask.HasFlag(Core.Mask.down))
                            bg.Graphics.FillRectangle(Brushes.White, i * 60 + 25, j * 60 + 25, 10, 35);
                        if (mask.HasFlag(Core.Mask.pc))
                            bg.Graphics.FillRectangle(Brushes.Red, i * 60 + 10, j * 60 + 10, 40, 40);
                        if (mask.HasFlag(Core.Mask.server))
                            bg.Graphics.FillRectangle(Brushes.Blue, i * 60 + 10, j * 60 + 10, 40, 40);
                    }
                bg.Render();
            }
            catch (NullReferenceException e)
            {
                bg = bgc.Allocate(pictureBox1.CreateGraphics(), pictureBox1.ClientRectangle);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                core.NewGame();
                Draw();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
        }
        int oldWidth, oldHeight;
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (oldWidth != Width)
            {
                Height = Width;
            }
            else if (oldHeight != Height)
            {
                Width = Height;
            }
            oldHeight = Height;
            oldWidth = Width;
            pictureBox1.Width = ClientRectangle.Height;
            pictureBox1.Height = ClientRectangle.Height;
            pictureBox1.Left = ClientRectangle.Width / 2 - pictureBox1.Width / 2;
            pictureBox1.Top = 0;
            bg.Dispose();
            bg = bgc.Allocate(pictureBox1.CreateGraphics(), pictureBox1.ClientRectangle);
        }
    }
}
