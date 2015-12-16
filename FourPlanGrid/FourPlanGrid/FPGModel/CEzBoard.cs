using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourPlanGrid.FPGModel
{
    public class CEzBoard : IFPGBoard
    {

        #region interface methods
        public bool IsWinner(int r, int c, out IEnumerable<Tuple<int, int>> cells)
        {
            bool ret = false;
            List<Tuple<int, int>>[] cellSegments = new List<Tuple<int, int>>[4] { new List<Tuple<int,int>>(),
                                                                           new List<Tuple<int,int>>(),
                                                                           new List<Tuple<int,int>>(),
                                                                           new List<Tuple<int,int>>(), };
            cellSegments.Initialize();
            ret |= IsWinnerHelper(r, c, cellSegments[0], 1, 0); // right left
            ret |= IsWinnerHelper(r, c, cellSegments[1], 0, 1); // up down
            ret |= IsWinnerHelper(r, c, cellSegments[2], 1, 1); // 45
            ret |= IsWinnerHelper(r, c, cellSegments[3], 1, -1); // -45

            cellSegments[0].Add(new Tuple<int, int>(r, c));
            cells = cellSegments[0].Concat(cellSegments[1]);
            cells = cells.Concat(cellSegments[2].Concat(cellSegments[3]));

            return ret;
        }

        private bool IsWinnerHelper(int r, int c, List<Tuple<int, int>> cells, int x, int y)
        {
            int total = 0;
            int i = 1;
            while (IsCellValid(r + i * y, c + i * x) && GetCell(r + i * y, c + i * x).Equals(GetCell(r, c)))
            {
                total++;
                cells.Add(new Tuple<int, int>(r + i * y, c + i * x));
                i++;
            }
            i = 1;
            while (IsCellValid(r - i * y, c - i * x) && GetCell(r - i * y, c - i * x).Equals(GetCell(r, c)))
            {
                total++;
                cells.Add(new Tuple<int, int>(r - i * y, c - i * x));
                i++;
            }
            if (total < 3)
            {
                cells.Clear();
            }
            return cells.Count > 0;
        }

        public bool Drop(int col, int value)
        {
            if (col >= _cols || col < 0) throw new ArgumentException(string.Format("Invalid Parameter: {0}", "col"));
            if (_tops[col] >= _rows) // this column is full
            {
                return false;
            }

            SetCell(Top(col), col, value);
            _tops[col]++;

            return true;
        }

        public int Top(int col)
        {
            if (col >= _cols || col < 0) throw new ArgumentException("Invalid Parameter: int col");
            return getTop(col);
        }

        public bool CanDrop(int col)
        {
            if (col >= _cols || col < 0) throw new ArgumentException("Invalid Parameter: int col");
            return _tops[col] < _rows;
        }

        public void Reset()
        {
            InitBoard();
        }

        public CEzBoard(int rows, int cols)
        {
            this._rows = rows;
            this._cols = cols;
            InitBoard();
        }
        #endregion

        #region private methods
        private void SetCell(int row, int col, int value)
        {
            _board[c2n(row, col)] = value;
        }
        private int GetCell(int row, int col)
        {
            return _board[c2n(row, col)];
        }
        private bool IsCellValid(int row, int col)
        {
            if (row < 0 || row >= _rows) return false;
            if (col < 0 || col >= _cols) return false;
            return true;
        }

        private int getTop(int col)
        {
            return _tops[col];
        }

        private void InitBoard()
        {
            _board = new int[_rows * _cols];
            _tops = new int[_cols];
        }

        private int c2n(int row, int col)
        {
            return _cols * row + col;
        }
        #endregion

        #region private members
        private int _rows, _cols;
        private int[] _board;
        private int[] _tops;
        #endregion
    }
}
