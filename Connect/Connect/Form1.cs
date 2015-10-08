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
        private Core core = new Core();
        Bitmap bitmap;
        Graphics graphics;

        public Form1()
        {
            InitializeComponent();
            core.NewGame();
            bitmap = new Bitmap(540, 540);
            graphics = Graphics.FromImage(bitmap);
            pictureBox1.Image = bitmap;
            Draw();
        }

        private void Draw()
        {
            graphics.Clear(Color.Black);
            for (int i = 0; i < (int)core.mode; i++)
                for (int j = 0; j < (int)core.mode; j++)
                {
                    if (core[i, j].left)
                        graphics.FillRectangle(Brushes.White, i * 60, j * 60 + 25, 35, 10);
                    if (core[i, j].right)
                        graphics.FillRectangle(Brushes.White, i * 60 + 25, j * 60 + 25, 35, 10);
                    if (core[i, j].up)
                        graphics.FillRectangle(Brushes.White, i * 60 + 25, j * 60, 10, 35);
                    if (core[i, j].down)
                        graphics.FillRectangle(Brushes.White, i * 60 + 25, j * 60 + 25, 10, 35);
                    if (core[i, j].pc)
                        graphics.FillRectangle(Brushes.Red, i * 60 + 10, j * 60 + 10, 40, 40);
                    if (core[i, j].server)
                        graphics.FillRectangle(Brushes.Blue, i * 60 + 10, j * 60 + 10, 40, 40);
                }
            pictureBox1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                core.NewGame();
                Draw();
            }
        }
    }
}
