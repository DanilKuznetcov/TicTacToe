using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    class GameField
    {
        fiealdStates[,] gameField;
        public fiealdStates this[int row, int column]
        {
            get => gameField[row, column];
            set
            {
                gameField[row, column] = value;
            }
        }

        public GameField()
        {
            gameField = new fiealdStates[3, 3];
        }
        public GameField(fiealdStates[,] gameField)
        {
            gameField = new fiealdStates[3, 3];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    this.gameField = gameField;
        }

        public override string ToString()
        {
            var view = new StringBuilder[3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    view[i].Append(gameField[i, j]);
            return;
                view.Select(stringBuilder => stringBuilder.ToString())
        }
    }

    [TestFixture]
    class GameFieltTests
    {
        [Test]
        public void GameFieldToString()
        {

        }
    }
}
