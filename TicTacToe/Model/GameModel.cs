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
        public event Action<int, int, fiealdStates> MarkerSet;
        public event Action<Tuple<Point, Point, Point>> GameWin;
        ArtificialIntellect AI;
        int gameProgress;

        GameField gameField;
        public fiealdStates playerSide = fiealdStates.cross;
        public fiealdStates enemySide
        {
            get => (playerSide == fiealdStates.cross)
                                ? fiealdStates.circle
                                : fiealdStates.cross;
        }

        bool AIMove { get => (gameProgress % 2 == (int)playerSide % 2); }

        public GameModel()
        {
            gameField = new GameField();
            AI = new ArtificialIntellect();
        }

        public void SetMarker(int row, int column)
        {
            if (gameField[row, column] == fiealdStates.empty && !gameField.IsEnd())
            {
                gameField[row, column] = playerSide;
                MarkerSet(row, column, playerSide);
                if (gameField.IsWin())
                    GameWin(gameField.winTuple);
                gameProgress++;
                MoveAI();
            }
        }

        public void MoveAI()
        {
            if (!gameField.IsEnd())
            {
                var nextMove = AI.NextMove(gameField, enemySide);
                var row = nextMove.Item1;
                var column = nextMove.Item2;
                gameField[row, column] = enemySide;
                SetMarker(row, column);
                MarkerSet(row, column, enemySide);
                if (gameField.IsWin())
                    GameWin(gameField.winTuple);
                gameProgress++;
            }
        }

        public void Reset()
        {
            gameProgress = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    gameField[i, j] = fiealdStates.empty;
                }
        }
    }
}
