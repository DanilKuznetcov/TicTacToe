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
        public event Action<int, int> MarkerSet;
        public event Action<Tuple<Point, Point, Point>> GameWin;
        public int gameProgress;

        fiealdStates[,] gameField;

        bool loseTactic;

        public GameModel()
        {
            gameField = new fiealdStates[3, 3];
        }

        public void SetMarker(int row, int column)
        {
            if (gameField[column, row] == fiealdStates.empty)
            {
                gameField[column, row] = gameProgress % 2 == 0
                                ? fiealdStates.cross
                                : fiealdStates.circle; // change model
                MarkerSet(column, row);              // call event with visual change
                CheckWin();
                gameProgress++;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    gameField[i, j] = fiealdStates.empty;
                }
        }

        bool CheckWin()
        {
            Tuple<Point, Point, Point> winTuple = Tuple.Create(new Point(), new Point(), new Point());
            var defaultTuple = Tuple.Create(new Point(), new Point(), new Point());

            for (int i = 0; i < 3; i++)
                if (!CheckWinRowAndColumn(i).Equals(defaultTuple))
                {
                    GameWin(CheckWinRowAndColumn(i));
                    return true;
                }

            winTuple = CheckWinDiagonals();

            if (!winTuple.Equals(defaultTuple))
            {
                GameWin(winTuple);
                return true;
            }
            return false;
        }
        Tuple<Point, Point, Point> CheckWinRowAndColumn(int startIndex)
        {
            Tuple<Point, Point, Point> winTuple = Tuple.Create(new Point(), new Point(), new Point());
            if (gameField[0, startIndex] != fiealdStates.empty
                && gameField[0, startIndex] == gameField[1, startIndex]
                && gameField[0, startIndex] == gameField[2, startIndex])
                winTuple = Tuple.Create(new Point(0, startIndex), new Point(1, startIndex), new Point(2, startIndex));
            else if (gameField[startIndex, 0] != fiealdStates.empty
                    && gameField[startIndex, 0] == gameField[startIndex, 1]
                    && gameField[startIndex, 0] == gameField[startIndex, 2])
                winTuple = Tuple.Create(new Point(startIndex, 0), new Point(startIndex, 1), new Point(startIndex, 2));
            return winTuple;
        }
        Tuple<Point, Point, Point> CheckWinDiagonals()
        {
            Tuple<Point, Point, Point> winTuple = Tuple.Create(new Point(), new Point(), new Point());
            if (gameField[0, 0] != fiealdStates.empty
                && gameField[0, 0] == gameField[1, 1]
                && gameField[0, 0] == gameField[2, 2])
                winTuple = Tuple.Create(new Point(0, 0), new Point(1, 1), new Point(2, 2));
            else if (gameField[0, 2] != fiealdStates.empty
                    && gameField[0, 2] == gameField[1, 1]
                    && gameField[0, 2] == gameField[2, 0])
                winTuple = Tuple.Create(new Point(2, 0), new Point(1, 1), new Point(0, 2));
            return winTuple;
        }
    }
}
