using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    class GameModel
    {
        public event Action<GameField> MarkerSeted;
        public event Action<Tuple<Point, Point, Point>> GameWin;

        public GameField currentField;
        public bool AIOpponent;
        public FieldStates playerSide = FieldStates.cross;
        public FieldStates enemySide
        {
            get => (playerSide == FieldStates.cross)
                                ? FieldStates.circle
                                : FieldStates.cross;
        }

        public GameModel()
        {
            currentField = GameField.Root;
            MarkerSeted += (gameField) =>
            {
                if (gameField.IsWin)
                    GameWin(gameField.winTuple);
            };
        }

        public void SetMarker(bool AIMove, int row = 0, int column = 0)
        {
            if (((AIOpponent && AIMove) || currentField[row, column] == FieldStates.empty) && !currentField.IsEnd)
            {
                if (AIOpponent && AIMove)
                {
                    if (currentField.PosibleMoves.Where(field => field.winStrategy || field.IsWin).Count() > 0)
                        currentField = currentField.PosibleMoves.Where(field => field.winStrategy || field.IsWin).RandomElement();
                    else if (currentField.PosibleMoves.Where(field => !field.loseStrategy).Count() > 0)
                        currentField = currentField.PosibleMoves.Where(field => !field.loseStrategy).RandomElement();
                    else
                        currentField = currentField.PosibleMoves.RandomElement();
                    MarkerSeted(currentField);
                }
                else
                {
                    currentField = currentField.PosibleMoves.Where(field => field[row, column] != FieldStates.empty).First();
                    if (!AIOpponent || currentField.IsEnd)
                        MarkerSeted(currentField);
                    else
                        SetMarker(true);
                }
            }
        }

        public void Reset()
        {
            currentField = GameField.Root;
        }
    }
}
