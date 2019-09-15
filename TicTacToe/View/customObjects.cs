using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Model;

namespace TicTacToe.View
{

    class TicTacToeMenuButton : Button
    {
        public TicTacToeMenuButton()
        {
            TextAlign = ContentAlignment.TopCenter;
            Size = new Size(50, 50);
            Font = new Font("Times New Roman", 30, FontStyle.Bold);
            ForeColor = Colors.Main;
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }
    }

    class FieldTable : TableLayoutPanel
    {
        public event Action ClickOnMove;
        public FieldTable(GameField gameField)
        {
            this.Size = new Size(50, 50);
            Anchor = AnchorStyles.Right;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;

            for (int i = 0; i < 3; i++)
            {
                RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            }

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    var text = new Label
                    {
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Montserrat Thin", 7, FontStyle.Bold),
                        ForeColor = Colors.Main
                    };
                    if (gameField[i, j] == FieldStates.empty)
                        text.Text = " ";
                    else
                        text.Text = gameField[i, j] == FieldStates.cross
                                    ? "Ⅹ"
                                    : "O";
                    Controls.Add(text, j, i);

                    text.MouseEnter += (sender, args) =>
                    {
                        ChangeBackColor(Colors.BackgroundMain);
                    };

                    text.Click += (sender, args) =>
                    {
                        ClickOnMove();
                    };

                    text.MouseLeave += (sender, args) =>
                    {
                        ChangeBackColor(Color.Transparent);
                    };
                }

            if (gameField.IsWin)
            {
                var tupleWin = gameField.winTuple;
                GetControlFromPosition(tupleWin.Item1.Y, tupleWin.Item1.X).ForeColor = Colors.Win;
                GetControlFromPosition(tupleWin.Item2.Y, tupleWin.Item2.X).ForeColor = Colors.Win;
                GetControlFromPosition(tupleWin.Item3.Y, tupleWin.Item3.X).ForeColor = Colors.Win;
            }

            MouseEnter += (sender, args) =>
            {
                ChangeBackColor(Colors.BackgroundMain);
            };
            Click += (sender, args) =>
            {
                ClickOnMove();
            };
            MouseLeave += (sender, args) =>
            {
                ChangeBackColor(Color.Transparent);
            };
        }
        void ChangeBackColor(Color color)
        {
            if (BackColor != color)
                BackColor = color;
        }
    }

    class ColumnTable : TableLayoutPanel
    {
        public ColumnTable()
        {
            Size = new Size(95, 300);
            Location = new Point(310, 50);
            BackColor = Colors.BackgroundSecondary;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
        }
    }

    class RownTable : TableLayoutPanel
    {
        public RownTable()
        {
            Size = new Size(400, 90);
            Location = new Point(5, 355);
            BackColor = Colors.BackgroundSecondary;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
        }
    }

    class TicTacToeButton : Button
    {
        public TicTacToeButton()
        {
            Dock = DockStyle.Fill;
            Font = new Font("Montserrat Thin", 50, FontStyle.Regular);
            TextAlign = ContentAlignment.TopCenter;
            ForeColor = Colors.Main;
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }
    }
}