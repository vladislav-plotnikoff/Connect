using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
        private int dXFormWidth, dYFormHeight, dSize, cellSize;

        public Form1()
        {
            InitializeComponent();
            core = new Core();
            core.NewGame();
            bgc = new BufferedGraphicsContext();
            dXFormWidth = Width - ClientSize.Width;
            dYFormHeight = Height - ClientSize.Height;
            dSize = dYFormHeight - dXFormWidth;
            MinimumSize = new Size(300 + dXFormWidth, 300 + dYFormHeight);

			bg = bgc.Allocate(pictureBox1.CreateGraphics(), pictureBox1.ClientRectangle);
			ClientSize = new Size(ClientSize.Height, ClientSize.Height);
		}

        private void Draw()
        {
            Core.Mask mask;

                bg.Graphics.Clear(Color.Black);
                for (int i = 0; i < core.width; i++)
                    for (int j = 0; j < core.width; j++)
                    {
                        mask = core[i, j];

                        if (mask.HasFlag(Core.Mask.left))
                            bg.Graphics.FillRectangle(Brushes.White, i * cellSize, j * cellSize + 25, 35, 10);
                        if (mask.HasFlag(Core.Mask.right))
                            bg.Graphics.FillRectangle(Brushes.White, i * cellSize + 25, j * cellSize + 25, 35, 10);
                        if (mask.HasFlag(Core.Mask.up))
                            bg.Graphics.FillRectangle(Brushes.White, i * cellSize + 25, j * cellSize, 10, 35);
                        if (mask.HasFlag(Core.Mask.down))
                            bg.Graphics.FillRectangle(Brushes.White, i * cellSize + 25, j * cellSize + 25, 10, 35);
                        if (mask.HasFlag(Core.Mask.pc))
                            bg.Graphics.FillRectangle(Brushes.Red, i * cellSize + 10, j * cellSize + 10, 40, 40);
                        if (mask.HasFlag(Core.Mask.server))
                            bg.Graphics.FillRectangle(Brushes.Blue, i * cellSize + 10, j * cellSize + 10, 40, 40);
                    }
                bg.Render();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                core.NewGame();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Width = ClientRectangle.Width;
            pictureBox1.Height = ClientRectangle.Width;
			cellSize = ClientRectangle.Width / core.width;
            bg.Dispose();
            bg = bgc.Allocate(pictureBox1.CreateGraphics(), pictureBox1.ClientRectangle);
        }

        const int WM_SIZING = 0x214;
        const int WMSZ_LEFT = 1;
        const int WMSZ_RIGHT = 2;
        const int WMSZ_TOP = 3;

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (ClientSize.Width != ClientSize.Height)
            {
                ClientSize = new Size(ClientSize.Height, ClientSize.Height);
            }
        }

		private void pictureBox1_MouseClick(object sender, MouseEventArgs e) {
			switch(e.Button) {
				case MouseButtons.Left:
					core.NewTurn(0, 1, Core.TypeTurn.left);
					break;
				case MouseButtons.Right:
					core.NewTurn(0, 1, Core.TypeTurn.right);
					break;
				case MouseButtons.Middle:
					core.NewTurn(0, 1, Core.TypeTurn.block);
					break;
			}
		}

		const int WMSZ_BOTTOM = 6;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SIZING)
            {
                Rectangle rc = (Rectangle)Marshal.PtrToStructure(m.LParam, typeof(Rectangle));
                int res = m.WParam.ToInt32();
                if (res == WMSZ_LEFT + WMSZ_TOP)
                {
                    rc.X = rc.Width - Height + dSize;
                }
                else if (res == WMSZ_TOP || res == WMSZ_BOTTOM || res == WMSZ_RIGHT + WMSZ_TOP)
                {
                    rc.Width = rc.Left + Height - dSize;
                }
                else if (res == WMSZ_LEFT || res == WMSZ_RIGHT || res == WMSZ_RIGHT + WMSZ_BOTTOM || res == WMSZ_LEFT + WMSZ_BOTTOM)
                {
                    rc.Height = rc.Top + Width + dSize;
                }
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            base.WndProc(ref m);
        }
    }
}
