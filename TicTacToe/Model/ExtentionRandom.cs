using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Model
{
    public static class IEnumerableExtensions
    {
        public static T RandomElement<T>(this IEnumerable<T> items)
        {
            var listItem = items.ToList();
            var rnd = new Random();
            var randomIndex = rnd.Next(listItem.Count);
            return listItem[randomIndex];
        }
    }
}
