using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourPlanGrid.FPGModel
{
    interface IFPGBoard
    {
        bool Drop(int col, int value);
        bool CanDrop(int col);
        int Top(int col);
        bool IsWinner(int r, int c, out IEnumerable<Tuple<int, int>> cells);
        void Reset();

    }
}
