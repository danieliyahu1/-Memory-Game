using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_1
{
    internal class Move
    {
        private int m_RowindexInBoard;
        private int m_ColumnIndexInBoard;

        public Move(string i_Cell)
        {
            m_ColumnIndexInBoard = i_Cell[0] - 'A';
            int.TryParse(i_Cell[1].ToString(), out m_RowindexInBoard);
            m_RowindexInBoard--;
        }

        internal int RowIndex
        {
            get { return this.m_RowindexInBoard; }
        }

        internal int ColumnIndex
        {
            get { return this.m_ColumnIndexInBoard; }
        }

        internal Move(int i_RowIndex, int i_ColumnIndex)
        {
            m_ColumnIndexInBoard = i_ColumnIndex;
            m_RowindexInBoard = i_RowIndex;
        }
    }
}
