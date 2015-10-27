using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Connect
{
    public partial class Form1 : Form
    {
        private Core core;
        private Bitmap bitmap, fon, pc, pcNet, server, menu;
        private Graphics graphics;
        private BufferedGraphics bg;
        private BufferedGraphicsContext bgc;
        private Brush netBrush;
        private Brush blockBrush;
        private const int cellSize = 63;
        private const int cell5 = 315;
        private const int cell7 = 441;
        private const int cell9 = 567;

        public Form1()
        {
            InitializeComponent();
            core = new Core();
            core.onWin += Win;
            core.NewGame(Core.Mode.Easy);
            stepsLabel.Text = core.steps.ToString();
            bgc = new BufferedGraphicsContext();

            fon = new Bitmap(567, 567);
            for (int i = 0; i < 567; i++)
                for (int j = 0; j < 567; j++)
                    fon.SetPixel(i, j, Color.FromArgb(
                        0,
                        102 + (int)Math.Round(Math.Sqrt((i - 283.5) * (i - 283.5) + (283.5 - j) * (283.5 - j)) * (-46) / 283.5),
                        204 + (int)Math.Round(Math.Sqrt((i - 283.5) * (i - 283.5) + (283.5 - j) * (283.5 - j)) * (-97) / 283.5)
                        ));

            pc = new Bitmap(63, 63);
            graphics = Graphics.FromImage(pc);
            graphics.DrawImage(Properties.Resources.PC, 0, 0, 63, 63);
            pcNet = new Bitmap(200, 200);
            graphics = Graphics.FromImage(pcNet);
            graphics.DrawImage(Properties.Resources.PCNet, 0, 0, 200, 200);
            server = new Bitmap(200, 200);
            graphics = Graphics.FromImage(server);
            graphics.DrawImage(Properties.Resources.Server, 0, 0, 200, 200);
            blockBrush = new Pen(Color.FromArgb(153, 0, 0, 0)).Brush;
            cursorBrush = new Pen(Color.FromArgb(51, 255, 255, 255)).Brush;
            menu = new Bitmap(cell9, cell9);
            menuPictureBox.Image = menu;

            bitmap = new Bitmap(cell9, cell9);
            graphics = Graphics.FromImage(bitmap);

            bg = bgc.Allocate(fieldPictureBox.CreateGraphics(), fieldPictureBox.ClientRectangle);
            ClientSize = new Size(567, 597);
            stepsLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            menuButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            timerLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            ClientSize = new Size(cell5, cell5 + 30);

            Draw();
        }

        private void Draw()
        {
            Core.Mask mask;

            graphics.DrawImage(fon, fieldPictureBox.ClientRectangle);
            for (int i = 0; i < core.width; i++)
                for (int j = 0; j < core.width; j++)
                {
                    mask = core[i, j];
                    netBrush = mask.HasFlag(Core.Mask.net) ? Brushes.LightSkyBlue : Brushes.LightGray;
                    if (mask.HasFlag(Core.Mask.block))
                        graphics.FillRectangle(blockBrush, i * cellSize, j * cellSize, cellSize, cellSize);
                    if (mask.HasFlag(Core.Mask.left))
                        graphics.FillRectangle(netBrush, i * cellSize, j * cellSize + cellSize / 9F * 4F, cellSize / 9F * 5F, cellSize / 9F * 1F);
                    if (mask.HasFlag(Core.Mask.right))
                        graphics.FillRectangle(netBrush, i * cellSize + cellSize / 9F * 4F, j * cellSize + cellSize / 9F * 4F, cellSize / 9F * 5F, cellSize / 9F * 1F);
                    if (mask.HasFlag(Core.Mask.up))
                        graphics.FillRectangle(netBrush, i * cellSize + cellSize / 9F * 4F, j * cellSize, cellSize / 9F * 1F, cellSize / 9F * 5F);
                    if (mask.HasFlag(Core.Mask.down))
                        graphics.FillRectangle(netBrush, i * cellSize + cellSize / 9F * 4F, j * cellSize + cellSize / 9F * 4F, cellSize / 9F * 1F, cellSize / 9F * 5F);
                    if (mask.HasFlag(Core.Mask.pc))
                        if (mask.HasFlag(Core.Mask.net))
                            graphics.DrawImage(pcNet, i * cellSize, j * cellSize, cellSize, cellSize);
                        else
                            graphics.DrawImage(pc, i * cellSize, j * cellSize, cellSize, cellSize);
                    if (mask.HasFlag(Core.Mask.server))
                        graphics.DrawImage(server, i * cellSize, j * cellSize, cellSize, cellSize);
                }
            bg.Graphics.DrawImage(bitmap, 0, 0);
            if (cursorVisible)
            {
                bg.Graphics.FillRectangle(cursorBrush, pointCursor.X * cellSize, pointCursor.Y * cellSize, cellSize, cellSize);
            }
        }

        private Point pointCursor;
        private bool cursorVisible;
        private Brush cursorBrush;
        private void fieldPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / cellSize;
            int y = e.Y / cellSize;
            if (!cursorVisible)
            {
                cursorVisible = true;
                bg.Graphics.FillRectangle(cursorBrush, x * cellSize, y * cellSize, cellSize, cellSize);
                pointCursor = new Point(x, y);
            }
            else if (pointCursor.X != x || pointCursor.Y != y)
            {
                if (pointCursor.X != -1)
                {
                    bg.Graphics.DrawImage(
                        bitmap,
                        new Rectangle(pointCursor.X * cellSize, pointCursor.Y * cellSize, cellSize, cellSize),
                        new Rectangle(pointCursor.X * cellSize, pointCursor.Y * cellSize, cellSize, cellSize),
                        GraphicsUnit.Pixel);
                }
                bg.Graphics.FillRectangle(cursorBrush, x * cellSize, y * cellSize, cellSize, cellSize);
                pointCursor = new Point(x, y);
            }
        }

        private void fieldPictureBox_MouseLeave(object sender, EventArgs e)
        {
            bg.Graphics.DrawImage(
                        bitmap,
                        new Rectangle(pointCursor.X * cellSize, pointCursor.Y * cellSize, cellSize, cellSize),
                        new Rectangle(pointCursor.X * cellSize, pointCursor.Y * cellSize, cellSize, cellSize),
                        GraphicsUnit.Pixel);
            cursorVisible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            fieldPictureBox.Width = ClientSize.Width;
            fieldPictureBox.Height = ClientSize.Width;
            bg = bgc.Allocate(fieldPictureBox.CreateGraphics(), fieldPictureBox.ClientRectangle);
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            menuPictureBox.Size = fieldPictureBox.Size;
            menuPictureBox.Visible = true;
            Bitmap bits = (Bitmap)bitmap.Clone();
            double[] matrix = { 0.005, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.005 };
            System.Drawing.Imaging.BitmapData bd = bits.LockBits(
                new Rectangle(0, 0, cell9, cell9),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bits.PixelFormat);
            IntPtr ptr = bd.Scan0;
            byte[] temp = new byte[Math.Abs(bd.Stride) * cell9];
            Marshal.Copy(ptr, temp, 0, temp.Length);
            byte[] temp1 = (byte[])temp.Clone();
            for (int i = 63; i < 252; i++)
                for (int j = 63; j < 252; j++)
                {
                    double r = 0, g = 0, b = 0;
                    for (int k = 0; k < 11; k++)
                    {
                        r += temp[2 + ((i - 5 + k) * 4) + (j * cell9 * 4)] * matrix[k];
                        g += temp[1 + ((i - 5 + k) * 4) + (j * cell9 * 4)] * matrix[k];
                        b += temp[0 + ((i - 5 + k) * 4) + (j * cell9 * 4)] * matrix[k];
                    }
                    temp1[2 + (i * 4) + (j * cell9 * 4)] = (byte)Math.Round(r);
                    temp1[1 + (i * 4) + (j * cell9 * 4)] = (byte)Math.Round(g);
                    temp1[0 + (i * 4) + (j * cell9 * 4)] = (byte)Math.Round(b);
                }
            for (int i = 63; i < 252; i++)
                for (int j = 63; j < 252; j++)
                {
                    double r = 0, g = 0, b = 0;
                    for (int k = 0; k < 11; k++)
                    {
                        r += temp1[2 + (i * 4) + ((j - 5 + k) * cell9 * 4)] * matrix[k];
                        g += temp1[1 + (i * 4) + ((j - 5 + k) * cell9 * 4)] * matrix[k];
                        b += temp1[0 + (i * 4) + ((j - 5 + k) * cell9 * 4)] * matrix[k];
                    }
                    temp[2 + (i * 4) + (j * cell9 * 4)] = (byte)Math.Round(r);
                    temp[1 + (i * 4) + (j * cell9 * 4)] = (byte)Math.Round(g);
                    temp[0 + (i * 4) + (j * cell9 * 4)] = (byte)Math.Round(b);
                }
            Marshal.Copy(temp, 0, ptr, temp.Length);
            bits.UnlockBits(bd);
            menuPictureBox.Image = (Bitmap)bits.Clone();

            menuPictureBox.Refresh();
        }

        private void menuPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            menuPictureBox.Visible = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    core.NewGame(Core.Mode.Easy);
                    stepsLabel.Text = core.steps.ToString();
                    Draw();
                    break;
                case Keys.F3:
                    core.RepeatGame();
                    stepsLabel.Text = core.steps.ToString();
                    Draw();
                    break;
                case Keys.Z:
                    if (e.Control)
                    {
                        core.Undo();
                        stepsLabel.Text = core.steps.ToString();
                        Draw();
                    }
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bg.Render();
            timerLabel.Text = core.timer.ToString();
        }

        private void Win()
        {
            Draw();
            stepsLabel.Text = core.steps.ToString();
            MessageBox.Show("Победа!");
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    core.NewTurn(e.X / cellSize, e.Y / cellSize, Core.TypeTurn.left);
                    stepsLabel.Text = core.steps.ToString();
                    break;
                case MouseButtons.Right:
                    core.NewTurn(e.X / cellSize, e.Y / cellSize, Core.TypeTurn.right);
                    stepsLabel.Text = core.steps.ToString();
                    break;
                case MouseButtons.Middle:
                    core.NewTurn(e.X / cellSize, e.Y / cellSize, Core.TypeTurn.block);
                    break;
            }
            Draw();
        }
    }
}
