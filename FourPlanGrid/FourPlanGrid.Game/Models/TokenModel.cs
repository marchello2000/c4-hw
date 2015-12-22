using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourPlanGrid.Game.Models
{
    enum TokenState
    {
        Empty,
        Hover,
        Ready,
        ReadyAI,
        Placed,
        Winner,
        NotWinner,
    }

    class TokenModel
    {
        #region Fields


        #endregion

        #region Properties
        public int Row { get; set; }
        public int Column { get; set; }
        public int Player { get; set; }
        public TokenState State { get; set; }

        #endregion

        #region Helpers

        #endregion
    }
}
