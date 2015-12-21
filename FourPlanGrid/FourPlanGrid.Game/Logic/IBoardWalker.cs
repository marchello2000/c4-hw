using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourPlanGrid.Game.Logic
{
    interface IBoardWalker<BoardElementType> where BoardElementType : IPlayer
    {
        /// <summary>
        /// GeBoardElementType the nexBoardElementType type BoardElementType to the righBoardElementType of the currenBoardElementType T
        /// </summary>
        /// <param name="cur"></param>
        /// <returns>the BoardElementType to the right, null if none</returns>
        BoardElementType GetRight(BoardElementType cur);

        BoardElementType GetRightUp(BoardElementType cur);

        BoardElementType GetUp(BoardElementType cur);

        BoardElementType GetLeftUp(BoardElementType cur);

        BoardElementType GetLeft(BoardElementType cur);

        BoardElementType GetLeftDown(BoardElementType cur);

        BoardElementType GetDown(BoardElementType cur);

        BoardElementType GetRightDown(BoardElementType cur);
        
    }
}
