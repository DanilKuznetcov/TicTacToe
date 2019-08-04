using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.View
{
    class gameForm : Form
    {
        TableLayoutPanel table = new TableLayoutPanel();
        public gameForm()
        {
            InitialForm();
            InitialTable();

            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
        }

        void InitialForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(300, 400);
            this.BackColor = Color.DarkSlateGray;
            this.Opacity = 0.9;

            InitialMenuButtons();

            this.Controls.Add(table);
        }
        Point moveStart;
        void Form_MouseDown(object sender, MouseEventArgs e)
        {
            // если нажата левая кнопка мыши
            if (e.Button == MouseButtons.Left)
            {
                moveStart = new Point(e.X, e.Y);
            }
        }
        void Form_MouseMove(object sender, MouseEventArgs e)
        {
            // если нажата левая кнопка мыши
            if ((e.Button & MouseButtons.Left) != 0)
            {
                // получаем новую точку положения формы
                Point deltaPos = new Point(e.X - moveStart.X, e.Y - moveStart.Y);
                // устанавливаем положение формы
                this.Location = new Point(this.Location.X + deltaPos.X,
                  this.Location.Y + deltaPos.Y);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.Aqua, Color.Blue, 90);

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(Location.X - 915, Location.Y - 150, 900, 500);
                this.Region = new Region(gp);
            }


            base.OnPaint(e);
        }

        void InitialMenuButtons()
        {
            var crossChoiceButton = new ticTacToeMenuButton()
            {
                Text = "Ⅹ",
            };
            crossChoiceButton.Location = new Point(Size.Width / 2 - 3 * crossChoiceButton.Size.Width / 2 - 1, 100 - crossChoiceButton.Size.Height);
            this.Controls.Add(crossChoiceButton);

            var newGameButton = new ticTacToeMenuButton()
            {
                Text = "↺"
            };
            newGameButton.Location = new Point(Size.Width / 2 - 1 * newGameButton.Size.Width / 2, 100 - newGameButton.Size.Height);
            this.Controls.Add(newGameButton);


            var noughtChoiceButton = new ticTacToeMenuButton()
            {
                Text = "◯"
            };
            noughtChoiceButton.Location = new Point(Size.Width / 2 + 1 * noughtChoiceButton.Size.Width / 2 + 1, 100 - noughtChoiceButton.Size.Height);
            this.Controls.Add(noughtChoiceButton);

            Paint += (sender, args) =>
            {
                var graphics = args.Graphics;
                var pen = new Pen(Color.White, 1);
                var menuButton = new ticTacToeMenuButton();

                var start = new Point(Size.Width / 2 - menuButton.Size.Width / 2, 100 - menuButton.Height + 7);
                var finish = new Point(Size.Width / 2 - menuButton.Size.Width / 2, 93);
                graphics.DrawLine(pen, start, finish);

                start = new Point(Size.Width / 2 + menuButton.Size.Width / 2, 100 - menuButton.Height + 7);
                finish = new Point(Size.Width / 2 + menuButton.Size.Width / 2, 93);
                graphics.DrawLine(pen, start, finish);
            };
        }

        void InitialTable()
        {
            table.Size = new Size(300, 300);
            table.Location = new Point(0, 100);
            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            table.BackColor = Color.LightSeaGreen;

            SetButtons();
        }

        void SetButtons()
        {
            for (int columns = 0; columns < 3; columns++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
                for (int rows = 0; rows < 3; rows++)
                {
                    var gameButton = new ticTacToeButton();
                    table.Controls.Add(gameButton, columns, rows);
                }
            }

        }
    }

    class ticTacToeMenuButton : Button
    {
        public ticTacToeMenuButton()
        {
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Size = new Size(40, 30);
            this.Font = new Font("Times New Roman", 15, FontStyle.Regular);
            this.ForeColor = Color.White;
            this.TabStop = false;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }
    }

    class ticTacToeButton : Button
    {
        public ticTacToeButton()
        {
            this.Dock = DockStyle.Fill;
        }
    }
}
