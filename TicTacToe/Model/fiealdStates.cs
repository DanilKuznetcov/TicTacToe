using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public enum FieldStates
    {
        empty,
        cross,
        circle
    }

    public class Field : IEnumerable<FieldStates>, ICloneable
    {
        readonly FieldStates[,] field;

        public Field()
        {
            field = new FieldStates[3, 3];
        }

        public FieldStates this[int row, int column]
        {
            get => field[row, column];
            set => field[row, column] = value;
        }

        public override int GetHashCode()
        {
            int code = 0;
            for (int i = 1; i <= 3; i++)
                for (int j = 1; j <= 3; j++)
                {
                    if (this[i - 1, j - 1] == FieldStates.circle)
                        code = code * 7 + i * 11 + j * 13;
                    else if (this[i - 1, j - 1] == FieldStates.cross)
                        code = code * 7 + i * 17 + j * 19;
                }
            return code;
        }

        public override bool Equals(object obj)
        {
            var field = (Field)obj;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (this[i, j] != field[i, j])
                        return false;
                }
            return true;
        }

        public IEnumerator<FieldStates> GetEnumerator()
        {
            foreach (var state in field)
                yield return state;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Clone()
        {
            var field = new Field();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    field[i, j] = this[i, j];
            return field;
        }
    }
}
