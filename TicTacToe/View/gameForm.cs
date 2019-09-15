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
    class GameForm : Form
    {
        TableLayoutPanel table = new TableLayoutPanel();
        ColumnTable column = new ColumnTable();
        RownTable row = new RownTable();
        GameModel gameModel;

        public GameForm(GameModel gameModel)
        {
            InitialForm();
            InitialTable();
            this.gameModel = gameModel;
            InitialGameModelVisial();
            UpdateGameForm();
        }
        void InitialGameModelVisial()
        {
            gameModel.MarkerSeted += (gameField) =>
            {
                UpdateGameForm();
            };
            gameModel.GameWin += (tupleWin) =>
            {
                table.GetControlFromPosition(tupleWin.Item1.Y, tupleWin.Item1.X).ForeColor = Colors.Win;
                table.GetControlFromPosition(tupleWin.Item2.Y, tupleWin.Item2.X).ForeColor = Colors.Win;
                table.GetControlFromPosition(tupleWin.Item3.Y, tupleWin.Item3.X).ForeColor = Colors.Win;
            };
        }
        void UpdateGameForm()
        {
            var gameField = gameModel.currentField;
            for (int row = 0; row < 3; row++)
                for (int column = 0; column < 3; column++)
                {
                    var button = table.GetControlFromPosition(column, row);
                    if (gameField[row, column] == FieldStates.empty)
                        button.Text = " ";
                    else
                        button.Text = gameField[row, column] == FieldStates.cross
                                    ? "Ⅹ"
                                    : "◯";
                }


            var newColumn = new ColumnTable();
            var newRow = new RownTable();

            //column.RowStyles.Clear();
            //column.Controls.Clear();
            //row.ColumnStyles.Clear();
            //row.Controls.Clear();

            var columnNumber = 0;
            foreach (var move in gameField.PosibleMoves)
            {
                var table = CreateChoiceLabel(move);

                if (newColumn.Controls.Count < 4)
                {
                    newColumn.RowStyles.Add(new RowStyle(SizeType.Percent, newColumn.Height / ((gameField.PosibleMoves.Count < 4) ? gameField.PosibleMoves.Count : 4) - 25));
                    newColumn.Controls.Add(table);
                }
                else
                {
                    newRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, newRow.Width / (gameField.PosibleMoves.Count - 4) - 25));
                    newRow.Controls.Add(table, columnNumber, 0);
                    newRow.Controls.SetChildIndex(table, 2);
                    columnNumber++;
                }
            }

            Controls.Remove(row);
            row = newRow;
            Controls.Add(row);
            Controls.Remove(column);
            column = newColumn;
            Controls.Add(column);
        }

        Label CreateChoiceLabel(GameField gameField)
        {
            var result = new Label();
            var text = new Label();
            var field = new FieldTable(gameField);
            field.ClickOnMove += () =>
            {
                int i;
                int j;
                for (i = 0; i < 3; i++)
                    for (j = 0; j < 3; j++)
                        if (gameField[i, j] != gameModel.currentField[i, j])
                        {
                            gameModel.SetMarker(false, i, j);
                            return;
                        }
            };

            result.Size = new Size(70, 70);
            result.Anchor = AnchorStyles.None;

            text.Font = new Font("Times New Roman", 15, FontStyle.Regular);
            text.Size = new Size(70, 20);
            text.TextAlign = ContentAlignment.BottomCenter;
            if (gameField.IsWin || gameField.winStrategy)
            {
                text.Text = "win";
                text.ForeColor = Colors.Win;
            }
            else if (gameField.loseStrategy)
            {
                text.Text = "lose";
                text.ForeColor = Colors.Lose;
            }
            else
            {
                text.Text = "draw";
                text.ForeColor = Colors.Main;
            }
            text.Dock = DockStyle.Bottom;

            field.Location = new Point((result.Width - field.Width) / 2, 0);

            result.Controls.Add(field);
            result.Controls.Add(text);

            return result;
        }

        void InitialForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(410, 450);
            this.BackColor = Colors.BackgroundMain;
            this.Opacity = 0.9;

            InitialMenuButtons();
            InitialTransportWindow();

            this.Controls.Add(table);
            this.Controls.Add(column);
            this.Controls.Add(row);
        }

        void InitialTransportWindow()
        {
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

        void InitialMenuButtons()
        {
            var heightLevel = 0;
            var sideChoiceButton = new TicTacToeMenuButton()
            {
                Text = "Ⅹ",
                Font = new Font("Times New Roman", 20, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomCenter
            };
            sideChoiceButton.Click += (seder, args) =>
            {
                gameModel.playerSide = gameModel.enemySide;
                sideChoiceButton.Text = gameModel.playerSide == FieldStates.cross
                                ? "Ⅹ"
                                : "◯";
                ResetGame();
            };
            sideChoiceButton.Location = new Point(Size.Width / 2 - 2 * sideChoiceButton.Size.Width - 3, heightLevel);
            this.Controls.Add(sideChoiceButton);

            var newGameButton = new TicTacToeMenuButton()
            {
                Text = "↺"
            };
            newGameButton.Click += (sender, args) =>
                {
                    ResetGame();
                };
            newGameButton.Location = new Point(Size.Width / 2 - newGameButton.Size.Width - 1, heightLevel);
            this.Controls.Add(newGameButton);

            var AIButton = new TicTacToeMenuButton()
            {
                Text = "AI",
                Font = new Font("Times New Roman", 19, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomCenter
            };
            AIButton.Click += (sender, args) =>
            {
                if (!gameModel.AIOpponent)
                {
                    AIButton.ForeColor = Colors.Win;
                    gameModel.AIOpponent = true;
                }
                else
                {
                    AIButton.ForeColor = Colors.Main;
                    gameModel.AIOpponent = false;
                }
                ResetGame();
            };
            AIButton.Location = new Point(Size.Width / 2 + 1, heightLevel);
            this.Controls.Add(AIButton);

            var closeButton = new TicTacToeMenuButton()
            {
                Text = "Ⓧ"
            };
            closeButton.Click += (sender, args) =>
            {
                this.Close();
            };
            closeButton.Location = new Point(Size.Width / 2 + closeButton.Size.Width + 3, heightLevel);
            this.Controls.Add(closeButton);

            Paint += (sender, args) =>
            {
                var graphics = args.Graphics;
                var pen = new Pen(Colors.Main, 2);
                var menuButton = new TicTacToeMenuButton();

                var start = new Point(Size.Width / 2 - newGameButton.Size.Width - 3, 17);
                var finish = new Point(Size.Width / 2 - newGameButton.Size.Width - 3, 43);
                graphics.DrawLine(pen, start, finish);


                start = new Point(Size.Width / 2 - 1, 17);
                finish = new Point(Size.Width / 2 - 1, 43);
                graphics.DrawLine(pen, start, finish);

                start = new Point(Size.Width / 2 + newGameButton.Size.Width + 1, 17);
                finish = new Point(Size.Width / 2 + newGameButton.Size.Width + 1, 43);
                graphics.DrawLine(pen, start, finish);
            };
        }

        void ResetGame()
        {
            gameModel.Reset();
            if (gameModel.playerSide == FieldStates.circle && gameModel.AIOpponent)
                gameModel.SetMarker(true);
            foreach (Button button in table.Controls)
                button.ForeColor = Colors.Main;
            UpdateGameForm();
        }

        void InitialTable()
        {
            table.Size = new Size(300, 300);
            table.Location = new Point(5, 50);
            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            table.BackColor = Colors.BackgroundSecondary;

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
                    var gameButton = new TicTacToeButton();
                    var currentRow = row;
                    var currentColumn = column;
                    gameButton.Click += (sender, args) =>
                    {
                        gameModel.SetMarker(false, currentRow, currentColumn);
                        var field = gameModel.currentField;
                    };
                    table.Controls.Add(gameButton, column, row);
                }
            }
        }
    }
}
