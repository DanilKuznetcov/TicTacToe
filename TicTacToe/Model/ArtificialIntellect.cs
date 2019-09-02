using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    class ArtificialIntellect
    {
        public int difficultLevel = 3;


        GameField gameField;
        FiealdStates playerSide;
        FiealdStates EnemySide
        {
            get => (playerSide == FiealdStates.cross)
                                ? FiealdStates.circle
                                : FiealdStates.cross;
        }
        FiealdStates LocalEnemySide(FiealdStates side)
        {
            return (playerSide == FiealdStates.cross)
                                ? FiealdStates.circle
                                : FiealdStates.cross;
        }

        Tuple<int, int> moveTuple;

        public Tuple<int, int> NextMove(GameField gameField, FiealdStates playerSide)
        {
            this.gameField = gameField.Clone();
            this.playerSide = playerSide;

            if (CheckPreWin(playerSide))
                return moveTuple;
            else if (CheckPreWin(EnemySide))
                return moveTuple;
            else if (OppotrunityCreateWinningSituation(playerSide))
                return moveTuple;
            else if (OppotrunityCreateWinningSituation(EnemySide))
                return moveTuple;
            else if (WinningPoints().Any(winPoint => EmptyPoints().Contains(winPoint)))
                return (playerSide == FiealdStates.circle && gameField[1,1] == FiealdStates.empty) ? Tuple.Create(1,1)
                    : WinningPoints().Where(winPoint => EmptyPoints().Contains(winPoint)).RandomElement();
            else
                return EmptyPoints().RandomElement(); 
        }

        bool CheckPreWin(FiealdStates side)
        {
            if (CheckPreWinRow(side) != null)
            {
                moveTuple = CheckPreWinRow(side);
                return true;
            }
            else if (CheckPreWinColumn(side) != null)
            {
                moveTuple = CheckPreWinColumn(side);
                return true;
            }
            else if (CheckPreWinDiagonals(side) != null)
            {
                moveTuple = CheckPreWinDiagonals(side);
                return true;
            }
            return false;
        }
        Tuple<int, int> CheckPreWinRow(FiealdStates side)
        {
            for (int rowNumber = 0; rowNumber < 3; rowNumber++)
            {
                if (gameField[rowNumber, 0] == FiealdStates.empty
                    && gameField[rowNumber, 1] == gameField[rowNumber, 2]
                    && gameField[rowNumber, 1] == side)
                    return Tuple.Create(rowNumber, 0);
                else if (gameField[rowNumber, 1] == FiealdStates.empty
                        && gameField[rowNumber, 0] == gameField[rowNumber, 2]
                        && gameField[rowNumber, 0] == side)
                    return Tuple.Create(rowNumber, 1);
                else if (gameField[rowNumber, 2] == FiealdStates.empty
                        && gameField[rowNumber, 0] == gameField[rowNumber, 1]
                        && gameField[rowNumber, 0] == side)
                    return Tuple.Create(rowNumber, 2);
            }
            return null;
        }
        Tuple<int, int> CheckPreWinColumn(FiealdStates side)
        {
            for (int columnNumber = 0; columnNumber < 3; columnNumber++)
            {
                if (gameField[0, columnNumber] == FiealdStates.empty
                && gameField[1, columnNumber] == gameField[2, columnNumber]
                && gameField[1, columnNumber] == side)
                    return Tuple.Create(0, columnNumber);
                else if (gameField[1, columnNumber] == FiealdStates.empty
                        && gameField[0, columnNumber] == gameField[2, columnNumber]
                        && gameField[0, columnNumber] == side)
                    return Tuple.Create(1, columnNumber);
                else if (gameField[2, columnNumber] == FiealdStates.empty
                        && gameField[0, columnNumber] == gameField[1, columnNumber]
                        && gameField[0, columnNumber] == side)
                    return Tuple.Create(2, columnNumber);
            }
            return null;
        }
        Tuple<int, int> CheckPreWinDiagonals(FiealdStates side)
        {
            if (gameField[0, 0] == FiealdStates.empty
                && gameField[1, 1] == gameField[2, 2]
                && gameField[1, 1] == side)
                return Tuple.Create(0, 0);
            else if (gameField[1, 1] == FiealdStates.empty
                    && gameField[0, 0] == gameField[2, 2]
                    && gameField[0, 0] == side)
                return Tuple.Create(1, 1);
            else if (gameField[2, 2] == FiealdStates.empty
                    && gameField[0, 0] == gameField[1, 1]
                    && gameField[0, 0] == side)
                return Tuple.Create(2, 2);
            else if (gameField[0, 2] == FiealdStates.empty
                    && gameField[1, 1] == gameField[2, 0]
                    && gameField[1, 1] == side)
                return Tuple.Create(0, 2);
            else if (gameField[1, 1] == FiealdStates.empty
                    && gameField[0, 2] == gameField[2, 0]
                    && gameField[0, 2] == side)
                return Tuple.Create(1, 1);
            else if (gameField[2, 0] == FiealdStates.empty
                    && gameField[0, 2] == gameField[1, 1]
                    && gameField[0, 2] == side)
                return Tuple.Create(2, 0);
            else
                return null;
        }

        bool OppotrunityCreateWinningSituation(FiealdStates side)
        {
            if (CheckDiagonalTactic(side))
                return true;
            foreach (var point in WinningPoints())
            {
                var rawNumber = point.Item1;
                var columnNumber = point.Item2;
                var checkingField = gameField.Clone();
                if (checkingField[rawNumber, columnNumber] == FiealdStates.empty)
                {
                    checkingField[rawNumber, columnNumber] = side;
                    if (CheckPotential(checkingField, side))
                    {
                        checkingField[rawNumber, columnNumber] = FiealdStates.empty;
                        moveTuple = point;
                        return true;
                    }
                    else
                        checkingField[rawNumber, columnNumber] = FiealdStates.empty;
                }
            }
            return false;
        }

        IEnumerable<Tuple<int, int>> WinningPoints()
        {
            yield return Tuple.Create(1, 1);
            yield return Tuple.Create(0, 0);
            yield return Tuple.Create(0, 2);
            yield return Tuple.Create(2, 0);
            yield return Tuple.Create(2, 2);
        }
        IEnumerable<Tuple<int, int>> EmptyPoints()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (gameField[i, j] == FiealdStates.empty)
                        yield return Tuple.Create(i, j);
                }
        }

        bool CheckPotential(GameField field, FiealdStates side)
        {
            var checkingField = field.Clone();
            for (int i = 0; i < 4; i++)
            {
                bool leftRightCorner = (checkingField[2, 0] == side && checkingField[0, 0] == side && checkingField[0, 2] == side
                    && checkingField[1, 0] == FiealdStates.empty && checkingField[0, 1] == FiealdStates.empty);
                bool leftCenterCorner = (checkingField[0, 0] == side && checkingField[1, 1] == side && checkingField[2, 0] == side
                    && checkingField[1, 0] == FiealdStates.empty &&
                    (checkingField[0, 2] == FiealdStates.empty || checkingField[2, 2] == FiealdStates.empty));
                if (leftRightCorner || leftCenterCorner)
                    return true;
                RotationField(checkingField);
            }
            return false;
        }
        bool CheckDiagonalTactic(FiealdStates side)
        {
            if (gameField[1, 1] == side &&
                    (gameField[0, 0] == LocalEnemySide(side) && gameField[2, 2] == LocalEnemySide(side)
                    || gameField[0, 2] == LocalEnemySide(side) && gameField[2, 0] == LocalEnemySide(side)))
            {
                moveTuple = Tuple.Create(0, 1);
                return true;
            }
            return false;
        }
        void RotationField(GameField field)
        {
            Transpose(field);
            ChangeRow(field);
        }
        void Transpose(GameField field)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (j <= i) continue;
                    var vessel = field[i, j];
                    field[i, j] = field[j, i];
                    field[j, i] = vessel;
                }
        }
        void ChangeRow(GameField field)
        {
            for (int i = 0; i < 3; i++)
            {
                var vessel = field[i, 0];
                field[i, 0] = field[i, 2];
                field[i, 2] = vessel;
            }
        }
    }
}
