using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    class Generator
    {
        public static void Generate()
        {
            var startField = new Field();
            GameField.Root = new GameField(startField);
            GameField.Fields = new Dictionary<Field, GameField>
            {
                [startField] = GameField.Root
            };

            var moves = new Queue<Field>();
            moves.Enqueue(startField);

            while (moves.Count != 0)
            {
                var currentField = moves.Dequeue();
                var currentGameField = GameField.Fields[currentField];

                if (!currentGameField.IsEnd)
                {
                    foreach (var possibleMove in PossibleMoves(currentField))
                    {
                        GameField move;
                        if (GameField.Fields.ContainsKey(possibleMove))
                            move = GameField.Fields[possibleMove];
                        else
                        {
                            move = new GameField(possibleMove);
                            GameField.Fields[possibleMove] = move;
                            moves.Enqueue(possibleMove);
                        }
                        currentGameField.PosibleMoves.Add(move);
                    }
                }
            }
            ChanceWin(GameField.Root);
        }
        static IEnumerable<Field> PossibleMoves(Field currentField)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (currentField[i, j] == FieldStates.empty)
                    {
                        var newField = (Field)currentField.Clone();
                        newField[i, j] =
                            currentField.Where(state => state != FieldStates.empty)
                                        .Count() % 2 == 0
                            ? FieldStates.cross
                            : FieldStates.circle;
                        yield return newField;
                    }
        }
        static void ChanceWin(GameField gameField)
        {
            gameField.PosibleMoves.Where(move => !move.IsEnd).ToList().ForEach(move => ChanceWin(move));
            gameField.winStrategy = gameField.PosibleMoves.All(opponentMove => !opponentMove.IsEnd && opponentMove.PosibleMoves.Any(secondMove => secondMove.IsWin || secondMove.winStrategy));
            gameField.loseStrategy = gameField.PosibleMoves.Any(opponentMove => opponentMove.IsWin || opponentMove.winStrategy);
        }
    }
}