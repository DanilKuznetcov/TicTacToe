using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    class GameModel
    {
        public event Action<int, int> markerSet;
        public string playerMarker;

        fiealdStates[,] gameField;
        fiealdStates playerSide;

        int gameProgress;
        bool gameLose;

        public GameModel()
        {
            gameField = new fiealdStates[3, 3];
            playerSide = fiealdStates.cross;
            playerMarker = "Ⅹ";
        }

        public void SetMarker(int row, int column)
        {
            if (gameField[column, row] == fiealdStates.empty)
            {
                gameField[column, row] = playerSide; // change model
                markerSet(column, row);              // call event with visual change
            }
        }
    }
}
