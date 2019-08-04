using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.View
{

    class ticTacToeMenuButton : Button
    {
        public ticTacToeMenuButton()
        {
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Size = new Size(40, 30);
            this.Font = new Font("Times New Roman", 15, FontStyle.Regular);
            this.ForeColor = Color.White;
            this.TabStop = false;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }
    }

    class ticTacToeButton : Button
    {
        public ticTacToeButton()
        {
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Montserrat Thin", 60, FontStyle.Regular);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.ForeColor = Color.White;
            this.TabStop = false;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }
    }

}
