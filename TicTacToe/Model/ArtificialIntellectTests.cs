using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    [TestFixture]
    class ArtificialIntellectTests
    {
        ArtificialIntellect AI;
        [SetUp]
        public void InitialTest()
        {
            AI = new ArtificialIntellect();
        }

        [TestCase(@"
                    0x.
                    0.x
                    ...",
                    2, 0,
                    TestName = "ColumnPreWin")]
        [TestCase(@"
                    .0.
                    x.x
                    ...",
                    1, 1,
                    TestName = "RowPreWin")]
        [TestCase(@"
                    ..x
                    .0.
                    x..",
                    0, 0,
                    TestName = "FirstWinningSituation")]
        [TestCase(@"
                    ..0
                    .x.
                    x0.",
                    0, 0,
                    TestName = "SecondWinningSituation")]
        [TestCase(@"
                    x..
                    .0.
                    ..x",
                    0, 2,
                    TestName = "FirstWinningSituationWithRotation")]
        [TestCase(@"
                    0..
                    .x0
                    ..x",
                    2, 0,
                    TestName = "SecondWinningSituationWithRotation")]
        [TestCase(@"
                    ..0
                    .x.
                    0..",
                    0, 2,
                    TestName = "EnemyWinningSituation")]
        public void CheckTacticTest(string data, int expectedRow, int expectedColumn, fiealdStates side = fiealdStates.cross)
        {
            var field = ParseField(data);
            var actualPoint = AI.NextMove(field, side);
            Assert.AreEqual(Tuple.Create(expectedRow, expectedColumn), actualPoint);
        }

        [TestCase(@"
                    ...
                    ...
                    ...",
                    TestName = "RandnomWinningElementsWithoutStrategy")]
        [TestCase(@"
                    x.0
                    0xx
                    x.0",
                    TestName = "RandnomElementsWithoutStrategy")]
        public void CheckRandom(string data)
        {
            var gameField = ParseField(data);
            var actualFirst = AI.NextMove(gameField, fiealdStates.cross);
            var actualSecond = AI.NextMove(gameField, fiealdStates.cross);
            var count = 10;
            while (actualFirst.Equals(actualSecond) || count > 0)
            {
                actualFirst = AI.NextMove(gameField, fiealdStates.cross);
                actualSecond = AI.NextMove(gameField, fiealdStates.cross);
                count--;
            }
            Assert.AreNotEqual(actualFirst, actualSecond);
        }
        
        public static GameField ParseField(string input)
        {
            var data = input.Split('\r', '\n');
            data = data.Where(row => row.Length > 0).Select(row => row.Trim()).ToArray();
            var resultField = new GameField();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (data[i][j] == 'x')
                        resultField[i, j] = fiealdStates.cross;
                    else if (data[i][j] == '0')
                        resultField[i, j] = fiealdStates.circle;
                }
            return resultField;
        }
    }
}
