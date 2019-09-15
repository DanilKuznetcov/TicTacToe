using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;
using System.Linq;

namespace TicTacToe.Model
{
    class GameField : IEnumerable<string> // field with statistic
    {
        public static Dictionary<Field, GameField> Fields;
        public static GameField Root;
        public static GameField ParseField(string input)
        {
            var data = input.Split('\r', '\n');
            data = data.Where(row => row.Length > 0).Select(row => row.Trim()).ToArray();
            var field = new Field();
            var resultField = new GameField(field);
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (data[i][j] == 'x')
                        resultField[i, j] = FieldStates.cross;
                    else if (data[i][j] == '0')
                        resultField[i, j] = FieldStates.circle;
                }
            return resultField;
        }

        public readonly Field gameField;
        public List<GameField> PosibleMoves;
        public bool loseStrategy;
        public bool winStrategy;
        public bool IsWin;
        public bool IsEnd;
        public Tuple<Point, Point, Point> winTuple;


        public FieldStates this[int row, int column]
        {
            get => gameField[row, column];
            set
            {
                gameField[row, column] = value;
                IsWin = isWin();
                IsEnd = isEnd();
            }
        }

        public GameField(Field fiealdStates)
        {
            gameField = new Field();
            PosibleMoves = new List<GameField>();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    gameField[i, j] = fiealdStates[i, j];

            IsWin = isWin();
            IsEnd = isEnd();
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
                    if (gameField[i, j] == FieldStates.cross)
                        mark = 'x';
                    else if (gameField[i, j] == FieldStates.circle)
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

        bool isEnd()
        {
            if (IsWin)
                return true;
            else
                foreach (var state in gameField)
                    if (state == FieldStates.empty)
                        return false;
            return true;
        }

        bool isWin()
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
            if (gameField[0, startIndex] != FieldStates.empty
                && gameField[0, startIndex] == gameField[1, startIndex]
                && gameField[0, startIndex] == gameField[2, startIndex])
            {
                winTuple = Tuple.Create(new Point(0, startIndex), new Point(1, startIndex), new Point(2, startIndex));
                return true;
            }
            else if (gameField[startIndex, 0] != FieldStates.empty
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
            if (gameField[0, 0] != FieldStates.empty
                && gameField[0, 0] == gameField[1, 1]
                && gameField[0, 0] == gameField[2, 2])
            {
                winTuple = Tuple.Create(new Point(0, 0), new Point(1, 1), new Point(2, 2));
                return true;
            }
            else if (gameField[0, 2] != FieldStates.empty
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
            var actual = new GameField(new Field());
            actual[1, 1] = FieldStates.cross;
            Assert.AreEqual(FieldStates.cross, actual[1, 1]);
        }
        [Test]
        public void VisualForDebugger()
        {
            var actual = new GameField(new Field());
            actual[1, 1] = FieldStates.cross;
            actual[2, 2] = FieldStates.circle;
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
            var field = GameField.ParseField(data);
            Assert.IsTrue(field.IsWin);
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
            var field = GameField.ParseField(data);
            Assert.IsTrue(field.IsEnd);
        }
        [TestCase(@"
                    ...
                    ...
                    ...",
                    0,
                    TestName = "HashCodeEmptyField")]
        [TestCase(@"
                    .x.
                    ...
                    ...",
                    55,
                    TestName = "HashCodeCrossField")]
        [TestCase(@"
                    0..
                    ...
                    ...",
                    24,
                    TestName = "HashCodeCircleField")]
        public void GetHashCodeTests(string data, int expectedCode)
        {
            var field = GameField.ParseField(data);
            var actualCode = field.gameField.GetHashCode();
            Assert.AreEqual(expectedCode, actualCode);
        }
        [TestCase(@"
                    0..
                    ...
                    ...",
                  @"0..
                    ...
                    ...",
                    true,
                    TestName = "DictionaryWithSameField")]
        [TestCase(@"
                    0..
                    ...
                    ...",
                  @"0x.
                    ...
                    ...",
                    false,
                    TestName = "DictionaryWithDifferentField")]
        public void ListFieldsTests(string data1, string data2, bool same)
        {
            var field1 = GameField.ParseField(data1);
            var field2 = GameField.ParseField(data2);
            var startField = new Field();
            GameField.Root = new GameField(startField);
            GameField.Fields = new Dictionary<Field, GameField>();
            var hash1 = field1.gameField.GetHashCode();
            var hash2 = field2.gameField.GetHashCode();
            GameField.Fields[field1.gameField] = field1;
            if (same)
            {
                Assert.AreEqual(1, GameField.Fields.Count);
                Assert.IsTrue(GameField.Fields.ContainsKey(field2.gameField));
            }
            else
            {
                GameField.Fields[field2.gameField] = field2;
                Assert.AreEqual(2, GameField.Fields.Count);
            }
        }
    }
}
