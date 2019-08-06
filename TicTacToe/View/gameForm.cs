using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;

namespace TicTacToe.View
{
    class gameForm : Form
    {
        TableLayoutPanel table = new TableLayoutPanel();
        GameModel gameModel;

        public gameForm(GameModel gameModel)
        {
            InitialForm();
            InitialTable();
            this.gameModel = gameModel;
            InitialGameModelVisial();
        }
        void InitialGameModelVisial()
        {
            gameModel.MarkerSet += (row, column, side) =>
            {
                var button = table.GetControlFromPosition(column, row);
                button.Text = side == fiealdStates.cross
                                ? "Ⅹ"
                                : "◯";
            };
            gameModel.GameWin += (tupleWin) =>
            {
                table.GetControlFromPosition(tupleWin.Item1.Y, tupleWin.Item1.X).ForeColor = Color.Gold;
                table.GetControlFromPosition(tupleWin.Item2.Y, tupleWin.Item2.X).ForeColor = Color.Gold;
                table.GetControlFromPosition(tupleWin.Item3.Y, tupleWin.Item3.X).ForeColor = Color.Gold;
            };
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

            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
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
                gp.AddRectangles(new Rectangle[] 
                {
                    new Rectangle(0, 100, 300, 300),
                    new Rectangle(28, 50, 244, 50),
                    new Rectangle(0, 75, 28, 25),
                    new Rectangle(272, 75, 28, 25)
                });
                this.Region = new Region(gp);
            }
            base.OnPaint(e);
        }

        void InitialMenuButtons()
        {
            var heightLevel = 100 - (new ticTacToeMenuButton()).Height;
            var crossChoiceButton = new ticTacToeMenuButton()
            {
                Text = "Ⅹ",
                Font = new Font("Times New Roman", 20, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomCenter
            };
            crossChoiceButton.Click += (seder, args) =>
            {
                gameModel.playerSide = gameModel.enemySide;
                crossChoiceButton.Text = gameModel.playerSide == fiealdStates.cross
                                ? "Ⅹ"
                                : "◯";
                gameModel.Reset();
                foreach (var control in table.Controls)
                {
                    ((Button)control).Text = "";
                    ((Button)control).ForeColor = Color.White;
                }
                if (gameModel.playerSide == fiealdStates.circle)
                    gameModel.MoveAI();
            };
            crossChoiceButton.Location = new Point(Size.Width / 2 - 3 * crossChoiceButton.Size.Width / 2 - 1, heightLevel);
            this.Controls.Add(crossChoiceButton);

            var newGameButton = new ticTacToeMenuButton()
            {
                Text = "↺"
            };
            newGameButton.Click += (sender, args) =>
                {
                    gameModel.Reset();
                    foreach (var control in table.Controls)
                    {
                        ((Button)control).Text = "";
                        ((Button)control).ForeColor = Color.White;

                    }
                    if (gameModel.playerSide == fiealdStates.circle)
                        gameModel.MoveAI();
                };
            newGameButton.Location = new Point(Size.Width / 2 - 1 * newGameButton.Size.Width / 2, heightLevel);
            this.Controls.Add(newGameButton);

            var circleChoiceButton = new ticTacToeMenuButton()
            {
                Text = "Ⓧ"
            };
            circleChoiceButton.Click += (sender, args) =>
            {
                this.Close();
            };
            circleChoiceButton.Location = new Point(Size.Width / 2 + 1 * circleChoiceButton.Size.Width / 2 + 1, heightLevel);
            this.Controls.Add(circleChoiceButton);

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
            for (int row = 0; row < 3; row++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
                for (int column = 0; column < 3; column++)
                {
                    var gameButton = new ticTacToeButton();
                    var currentRow = row;
                    var currentColumn = column;
                    gameButton.Click += (sender, args) =>
                    {
                        gameModel.SetMarker(currentRow, currentColumn);
                    };
                    table.Controls.Add(gameButton, column, row);
                }
            }

        }
    }
}
