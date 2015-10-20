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
        private Bitmap bitmap, fon, pc, pcNet, server;
        private Graphics graphics;
        private BufferedGraphics bg;
        private BufferedGraphicsContext bgc;
        private int dXFormWidth, dYFormHeight, dSize;
        private float cellSize;
        private Brush netBrush;
        private Brush blockBrush;

        public Form1()
        {
            InitializeComponent();
            core = new Core();
            core.NewGame();
            bgc = new BufferedGraphicsContext();
            dXFormWidth = Width - ClientSize.Width;
            dYFormHeight = Height - ClientSize.Height;
            dSize = dYFormHeight - dXFormWidth + 30;
            MinimumSize = new Size(300 + dXFormWidth, 330 + dYFormHeight);

            fon = new Bitmap(540, 540);
            for (int i = 0; i < 540; i++)
                for (int j = 0; j < 540; j++)
                    fon.SetPixel(i, j, Color.FromArgb(
                        0 + (int)Math.Round(Math.Sqrt((i - 270) * (i - 270) + (270 - j) * (270 - j)) * (7) / 270.0),
                        102 + (int)Math.Round(Math.Sqrt((i - 270) * (i - 270) + (270 - j) * (270 - j)) * (-46) / 270.0),
                        255 + (int)Math.Round(Math.Sqrt((i - 270) * (i - 270) + (270 - j) * (270 - j)) * (-97) / 270.0)
                        ));

            pc = new Bitmap(200, 200);
            graphics = Graphics.FromImage(pc);
            graphics.DrawImage(Properties.Resources.PC, 0, 0, 200, 200);
            pcNet = new Bitmap(200, 200);
            graphics = Graphics.FromImage(pcNet);
            graphics.DrawImage(Properties.Resources.PCNet, 0, 0, 200, 200);
            server = new Bitmap(200, 200);
            graphics = Graphics.FromImage(server);
            graphics.DrawImage(Properties.Resources.Server, 0, 0, 200, 200);
            blockBrush = new Pen(Color.FromArgb(128, 0, 0, 0)).Brush;

            bg = bgc.Allocate(pictureBox1.CreateGraphics(), pictureBox1.ClientRectangle);
            ClientSize = new Size(567, 597);

            menuButton.Top = 567;
            menuButton.Left = 75;
            menuButton.Width = 390;
        }

        private void Draw()
        {
            Core.Mask mask;

            bg.Graphics.DrawImage(fon, pictureBox1.ClientRectangle);
            for (int i = 0; i < core.width; i++)
                for (int j = 0; j < core.width; j++)
                {
                    mask = core[i, j];
                    netBrush = mask.HasFlag(Core.Mask.net) ? Brushes.CornflowerBlue : Brushes.Gainsboro;
                    if (mask.HasFlag(Core.Mask.block))
                        bg.Graphics.FillRectangle(blockBrush, i * cellSize, j * cellSize, cellSize, cellSize);
                    if (mask.HasFlag(Core.Mask.left))
                        bg.Graphics.FillRectangle(netBrush, i * cellSize, j * cellSize + cellSize / 9F * 4F, cellSize / 9F * 5F, cellSize / 9F * 1F);
                    if (mask.HasFlag(Core.Mask.right))
                        bg.Graphics.FillRectangle(netBrush, i * cellSize + cellSize / 9F * 4F, j * cellSize + cellSize / 9F * 4F, cellSize / 9F * 5F, cellSize / 9F * 1F);
                    if (mask.HasFlag(Core.Mask.up))
                        bg.Graphics.FillRectangle(netBrush, i * cellSize + cellSize / 9F * 4F, j * cellSize, cellSize / 9F * 1F, cellSize / 9F * 5F);
                    if (mask.HasFlag(Core.Mask.down))
                        bg.Graphics.FillRectangle(netBrush, i * cellSize + cellSize / 9F * 4F, j * cellSize + cellSize / 9F * 4F, cellSize / 9F * 1F, cellSize / 9F * 5F);
                    if (mask.HasFlag(Core.Mask.pc))
                        if (mask.HasFlag(Core.Mask.net))
                            bg.Graphics.DrawImage(pcNet, i * cellSize + cellSize / 9F * 1F, j * cellSize + cellSize / 9F * 1F, cellSize / 9F * 7F, cellSize / 9F * 7F);
                        else
                            bg.Graphics.DrawImage(pc, i * cellSize + cellSize / 9F * 1F, j * cellSize + cellSize / 9F * 1F, cellSize / 9F * 7F, cellSize / 9F * 7F);
                    if (mask.HasFlag(Core.Mask.server))
                        bg.Graphics.DrawImage(server, i * cellSize + cellSize / 9F * 1F, j * cellSize + cellSize / 9F * 1F, cellSize / 9F * 7F, cellSize / 9F * 7F);
                }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    core.NewGame();
                    Draw();
                    break;
                case Keys.F3:
                    core.RepeatGame();
                    Draw();
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bg.Render();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Width = ClientRectangle.Width;
            pictureBox1.Height = ClientRectangle.Width;
            cellSize = ClientRectangle.Width / core.width;
            Text = ClientRectangle.Width.ToString();
            bg.Dispose();
            bg = bgc.Allocate(pictureBox1.CreateGraphics(), pictureBox1.ClientRectangle);
            Draw();
        }

        const int WM_SIZING = 0x214;
        const int WMSZ_LEFT = 1;
        const int WMSZ_RIGHT = 2;
        const int WMSZ_TOP = 3;

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (ClientSize.Width != ClientSize.Height + 30 || ClientSize.Width % 81 != 0)
            {
                if (ClientSize.Width % 81 > 40)
                    ClientSize = new Size(ClientSize.Width + 81 - ClientSize.Width % 81, ClientSize.Width + 30 + 81 - ClientSize.Width % 81);
                else
                    ClientSize = new Size(ClientSize.Width - ClientSize.Width % 81, ClientSize.Width + 30 - ClientSize.Width % 81);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    core.NewTurn(e.X / (int)cellSize, e.Y / (int)cellSize, Core.TypeTurn.left);
                    break;
                case MouseButtons.Right:
                    core.NewTurn(e.X / (int)cellSize, e.Y / (int)cellSize, Core.TypeTurn.right);
                    break;
                case MouseButtons.Middle:
                    core.NewTurn(e.X / (int)cellSize, e.Y / (int)cellSize, Core.TypeTurn.block);
                    break;
            }
            Draw();
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
