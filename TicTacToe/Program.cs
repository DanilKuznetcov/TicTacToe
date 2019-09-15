using System;
using TicTacToe.View;
using TicTacToe.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Generator.Generate();
            var gameModel = new GameModel();
            Application.Run(new GameForm(gameModel));
        }
    }
}
