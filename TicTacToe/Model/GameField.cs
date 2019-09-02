using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;

namespace TicTacToe.Model
{
    class GameField : IEnumerable<string>
    {
        readonly FiealdStates[,] gameField;

        public FiealdStates this[int row, int column]
        {
            get => gameField[row, column];
            set => gameField[row, column] = value;
        }

        public GameField()
        {
            gameField = new FiealdStates[3, 3];
        }

        public GameField(FiealdStates[,] fiealdStates)
        {
            gameField = new FiealdStates[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    gameField[i, j] = fiealdStates[i, j];
        }

        public IEnumerator<string> GetEnumerator()
        {
            var result = new StringBuilder[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] = new StringBuilder();
                for (int j = 0; j < 3; j++)
                {
                    var mark = '-';
                    if (gameField[i, j] == FiealdStates.cross)
                        mark = 'x';
                    else if (gameField[i, j] == FiealdStates.circle)
                        mark = 'o';
                    result[i].Append(mark);
                }
            }
            yield return result[0].ToString();
            yield return result[1].ToString();
            yield return result[2].ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public GameField Clone()
        {
            var newGameField = new GameField(gameField);
            return newGameField;
        }

        public bool IsEnd()
        {
            if (IsWin())
                return true;
            else
                foreach (var state in gameField)
                    if (state == FiealdStates.empty)
                        return false;
            return true;
        }

        public Tuple<Point, Point, Point> winTuple;
        public bool IsWin()
        {
            for (int i = 0; i < 3; i++)
                if (CheckWinRowAndColumn(i))
                {
                    return true;
                }
            return CheckWinDiagonals();
        }
        bool CheckWinRowAndColumn(int startIndex)
        {
            if (gameField[0, startIndex] != FiealdStates.empty
                && gameField[0, startIndex] == gameField[1, startIndex]
                && gameField[0, startIndex] == gameField[2, startIndex])
            {
                winTuple = Tuple.Create(new Point(0, startIndex), new Point(1, startIndex), new Point(2, startIndex));
                return true;
            }
            else if (gameField[startIndex, 0] != FiealdStates.empty
                    && gameField[startIndex, 0] == gameField[startIndex, 1]
                    && gameField[startIndex, 0] == gameField[startIndex, 2])
            {
                winTuple = Tuple.Create(new Point(startIndex, 0), new Point(startIndex, 1), new Point(startIndex, 2));
                return true;
            }
            return false;
        }
        bool CheckWinDiagonals()
        {
            if (gameField[0, 0] != FiealdStates.empty
                && gameField[0, 0] == gameField[1, 1]
                && gameField[0, 0] == gameField[2, 2])
            {
                winTuple = Tuple.Create(new Point(0, 0), new Point(1, 1), new Point(2, 2));
                return true;
            }
            else if (gameField[0, 2] != FiealdStates.empty
                    && gameField[0, 2] == gameField[1, 1]
                    && gameField[0, 2] == gameField[2, 0])
            {
                winTuple = Tuple.Create(new Point(2, 0), new Point(1, 1), new Point(0, 2));
                return true;
            }
            return false;
        }
    }

    [TestFixture]
    class GameFieldTests
    {
        [Test]
        public void SetAndGet()
        {
            var actual = new GameField();
            actual[1, 1] = FiealdStates.cross;
            Assert.AreEqual(FiealdStates.cross, actual[1, 1]);
        }
        [Test]
        public void VisualForDebugger()
        {
            var actual = new GameField();
            actual[1, 1] = FiealdStates.cross;
            actual[2, 2] = FiealdStates.circle;
            var expected = new List<string> { "---", "-x-", "--o" };
            Assert.AreEqual(expected, actual);
        }
        [TestCase(@"
                    ...
                    .x0
                    xxx",
                    "20,21,22",
                    TestName = "RowWinning")]
        [TestCase(@"
                    .x.
                    .x0
                    .xx",
                    "01,11,21",
                    TestName = "ColumnWinning")]
        [TestCase(@"
                    ..0
                    .00
                    0xx",
                    "20,11,02",
                    TestName = "DiagonalWinning")]
        public void WinTests(string data, string winPoints)
        {
            var field = ArtificialIntellectTests.ParseField(data);
            Assert.IsTrue(field.IsWin());
            var dataTuple = winPoints.Split(',');
            var winTuple = Tuple.Create(new Point(int.Parse(dataTuple[0][0].ToString()), int.Parse(dataTuple[0][1].ToString())),
                                        new Point(int.Parse(dataTuple[1][0].ToString()), int.Parse(dataTuple[1][1].ToString())),
                                        new Point(int.Parse(dataTuple[2][0].ToString()), int.Parse(dataTuple[2][1].ToString())));
            Assert.AreEqual(winTuple, field.winTuple);
        }
        [Test]
        public void EndTest()
        {
            var data = @"
                        0x0
                        xx0
                        x0x";
            var field = ArtificialIntellectTests.ParseField(data);
            Assert.IsTrue(field.IsEnd());
        }
    }
}
