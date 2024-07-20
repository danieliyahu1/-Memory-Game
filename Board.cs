using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryBoard
{
    internal class Board
    {
        private int m_BoardLength;
        private int m_BoardWidth;
        private char[,] m_Values;
        private bool[,] m_OccupancyValues;

        public Board(int i_Length, int i_Width)
        {
            m_BoardLength = i_Length;
            m_BoardWidth = i_Width;
            m_Values = new char[m_BoardLength, m_BoardWidth];
            setValues(m_Values);
            m_OccupancyValues = new bool[m_BoardLength, m_BoardWidth];
        }

        internal int Length
        {
            get { return this.m_BoardLength; }
        }

        internal int Width
        {
            get { return this.m_BoardWidth; }
        }

        private void setValues(char[,] io_Values)
        {
            List<char> inValidChars = new List<char>(26);
            List<string> invalidCells = new List<string>((m_BoardWidth * m_BoardLength) / 2);
            string firstCell;
            string SecondCell;
            Random rnd = new Random();
            int card;
            int firstCardColumn; ;
            int firstCardRow;
            int secondCardColumn;
            int secondCardRow;
            for (int i = 0; i < (m_BoardLength * m_BoardWidth) / 2; i++)
            {
                firstCardColumn = rnd.Next(0, m_BoardWidth);
                firstCardRow = rnd.Next(0, m_BoardLength);
                secondCardColumn = rnd.Next(0, m_BoardWidth);
                secondCardRow = rnd.Next(0, m_BoardLength);
                card = rnd.Next('A', 'Z' + 1);

                while (inValidChars.Contains((char)card))
                {
                    card = rnd.Next('A', 'Z' + 1);
                }

                inValidChars.Add((char)card);

                firstCell = firstCardRow.ToString() + firstCardColumn.ToString();
                SecondCell = secondCardRow.ToString() + secondCardColumn.ToString();

                while ((firstCardColumn == secondCardColumn && firstCardRow == secondCardRow) || invalidCells.Contains(firstCell) || invalidCells.Contains(SecondCell))
                {
                    secondCardRow = rnd.Next(0, m_BoardLength);
                    secondCardColumn = rnd.Next(0, m_BoardWidth);
                    firstCardColumn = rnd.Next(0, m_BoardWidth);
                    firstCardRow = rnd.Next(0, m_BoardLength);
                    firstCell = firstCardRow.ToString() + firstCardColumn.ToString();
                    SecondCell = secondCardRow.ToString() + secondCardColumn.ToString();
                }
                
                invalidCells.Add(firstCell);
                invalidCells.Add(SecondCell);
                io_Values[firstCardRow, firstCardColumn] = (char)card;
                io_Values[secondCardRow, secondCardColumn] = (char)card;
            }
        }

        internal bool IsOccupancyTile(string i_Move)
        {
            int rowIndexInBoard;
            int columnIndexInBoard = i_Move[0] - 'A';
            int.TryParse(i_Move[1].ToString(), out rowIndexInBoard);
            rowIndexInBoard--;

            return m_OccupancyValues[rowIndexInBoard, columnIndexInBoard];
        }

        internal bool AllTilesAreFull()
        {
            int numOfSimilarCards = 0;
            for (int i = 0; i < m_BoardLength; i++)
            {
                for (int j = 0; j < m_BoardWidth; j++)
                {
                    if (m_OccupancyValues[i, j] == true)
                    {
                        numOfSimilarCards++;
                    }
                }
            }

            return numOfSimilarCards == m_BoardWidth * m_BoardLength;
        }

        internal bool IsPair(int i_FirstMoveRow, int i_FirstMoveColumn, int i_SecondMoveRow, int i_SecondMoveColumn)
        {
            bool isPair = true;

            if (m_Values[i_FirstMoveRow, i_FirstMoveColumn] != m_Values[i_SecondMoveRow, i_SecondMoveColumn])
            {
                m_OccupancyValues[i_FirstMoveRow, i_FirstMoveColumn] = false;
                m_OccupancyValues[i_SecondMoveRow, i_SecondMoveColumn] = false;

                isPair = false;
            }

            return isPair;
        }

        internal char ValueInCell(int i_MoveRow, int i_MoveColumn)
        {
            return m_Values[i_MoveRow, i_MoveColumn];
        }

        internal StringBuilder ShowPlayerMove(int i_FirstMoveRow, int i_FirstMoveColumn)
        {
            m_OccupancyValues[i_FirstMoveRow, i_FirstMoveColumn] = true;
            return BoardView();
        }

        internal void RemoveShownCard(int i_FirstMoveColumn, int i_FirstMoveRow)
        {
            m_OccupancyValues[i_FirstMoveRow, i_FirstMoveColumn] = false;
        }

        internal StringBuilder BoardView()
        {
            int rowCounter = 1;
            StringBuilder row = new StringBuilder();
            StringBuilder rowSeperator = new StringBuilder();
            StringBuilder board = new StringBuilder();
            for (int i = -1; i < m_BoardLength; i++)
            {
                for (int j = -1; j < m_BoardWidth; j++)
                {
                    if (i == -1)
                    {
                        if (j == -1)
                        {
                            rowSeperator.Append("   ");
                            row.Append(" ");
                        }
                        else if (j > -1)
                        {
                            rowSeperator.Append("====");
                            row.Append("   ");
                            row.Append((char)('A' + j));
                            
                        }
                    }
                    else
                    {
                        if (j == -1)
                        {
                            row.Append(rowCounter++);
                            row.Append(" | ");
                        }
                        else
                        {
                            if (m_OccupancyValues[i, j] == true)
                            {
                                row.Append(m_Values[i, j]);
                                row.Append(" | ");

                            }
                            else
                            {
                                row.Append("  | ");
                            }
                        }
                    }
                }
                
                board.Append(row);
                row = row.Clear();
                board.Append(Environment.NewLine);
                board.Append(rowSeperator);
                board.Append(Environment.NewLine);
            }

            return board;
        }
    }
}
