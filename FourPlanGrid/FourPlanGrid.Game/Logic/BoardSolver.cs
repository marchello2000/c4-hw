using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourPlanGrid.Game.Logic
{
    class BoardSolver<BoardElementType> where BoardElementType : IPlayer
    {
        /// <summary>
        /// The algorithm for solving a board. Pass in a BoardElementType and
        /// Solve will return an ICollection of  
        /// </summary>
        /// <param name="cur"></param>
        /// <returns></returns>
        public static ICollection<BoardElementType> Solve(BoardElementType cur, IBoardWalker<BoardElementType> itr)
        {
            HashSet<BoardElementType> results = new HashSet<BoardElementType>();

            results.UnionWith(SolveLine(cur, itr.GetRight, itr.GetLeft));
            results.UnionWith(SolveLine(cur, itr.GetRightUp, itr.GetLeftDown));
            results.UnionWith(SolveLine(cur, itr.GetUp, itr.GetDown));
            results.UnionWith(SolveLine(cur, itr.GetLeftUp, itr.GetRightDown));

            if (results.Count > 0) // i.e. there is some winning set, so add current
                results.Add(cur);

            return results;
        }

        private delegate BoardElementType itrDirectionDelegate(BoardElementType cur);
        private static ICollection<BoardElementType> SolveLine(BoardElementType cur, itrDirectionDelegate forward, itrDirectionDelegate backward)
        {
            List<BoardElementType> results = new List<BoardElementType>();

            BoardElementType temp;

            temp = forward(cur);
            while (temp != null && temp.Player == cur.Player)
            {
                results.Add(temp);
                temp = forward(temp);
            }
            temp = backward(cur);
            while (temp != null && temp.Player == cur.Player)
            {
                results.Add(temp);
                temp = backward(temp);
            }

            if (results.Count < 3)
                results.Clear();

            return results;
        }
    }
}
