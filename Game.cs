using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MemoryBoard;

namespace B20_Ex02_1
{
    internal class Game
    {
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private bool m_IsComputerPlayer;
        private Board m_GameBoard;
        private int m_Turn;
        private bool m_IsQuite;
        private Dictionary<Move, char> m_LastMovesInTheGame;
        private int m_LevelOfGame;

        internal Game(Player i_FirstPlayer, Player i_SecondPlayer, bool i_isComputerPlayer, int i_BoardWidth, int i_BoardLength, int levelOfGame)
        {
            this.m_FirstPlayer = i_FirstPlayer;
            this.m_SecondPlayer = i_SecondPlayer;
            this.m_IsComputerPlayer = i_isComputerPlayer;
            this.m_GameBoard = new Board(i_BoardWidth, i_BoardLength);
            this.m_Turn = 0;
            this.m_IsQuite = false;
            this.m_LevelOfGame = levelOfGame*2;
            this.m_FirstPlayer.Score = 0;
            this.m_SecondPlayer.Score = 0;

        }

        internal int Turn
        {
            get { return this.m_Turn; }
        }

        internal bool IsQuite
        {
            get { return this.m_IsQuite; }

            set { this.m_IsQuite = value; }
        }

        internal Board GameBoard
        {
            get { return this.m_GameBoard; }
        }


        internal bool ExecuteMove(Move i_FirstMove, Move i_SecondMove)
        {
            bool isChangeBoard = true;

            // Checks if this move is a win move
            if (this.m_GameBoard.IsPair(i_FirstMove.RowIndex, i_FirstMove.ColumnIndex, i_SecondMove.RowIndex, i_SecondMove.ColumnIndex))
            {
                if (m_Turn == 0)
                {
                    m_FirstPlayer.Score++;
                }
                else
                {
                    m_SecondPlayer.Score++;
                }
            }

            else
            {
                switchTurn();
                isChangeBoard = false;
            }

            return isChangeBoard;
        }

        private void switchTurn()
        {
            m_Turn = (m_Turn * (-1) + 1);
        }

        internal Move GetComputerAIMove()
        {
            Move newMoveOfComputer = null;
            if (m_LastMovesInTheGame == null)
            {
                newMoveOfComputer = getComputerMove();
            }
            else
            {
                Dictionary<char, Move> uniqe = new Dictionary<char, Move>();
                char charValue;
                foreach (Move previousMove in m_LastMovesInTheGame.Keys)
                {
                    charValue = m_GameBoard.ValueInCell(previousMove.RowIndex, previousMove.ColumnIndex);
                    if (uniqe.ContainsKey(charValue) && !m_GameBoard.IsOccupancyTile(indexToPosition(previousMove.RowIndex,previousMove.ColumnIndex)) && (previousMove.RowIndex != uniqe[charValue].RowIndex || previousMove.ColumnIndex != uniqe[charValue].ColumnIndex))
                    {
                        newMoveOfComputer = uniqe[charValue];
                        break;
                    }
                    uniqe[charValue] = previousMove;
                }
                if (newMoveOfComputer == null)
                {
                    newMoveOfComputer = getComputerMove();
                }
            }
            return newMoveOfComputer;
        }

        internal Move GetComputerAIMove(Move i_FirstMove)
        {
            Move computerMove = checkIfRememberPair(i_FirstMove);
            if (computerMove == null)
            {
                computerMove = this.getComputerMove();
            }
            RememberMoveForComputer(i_FirstMove);
            RememberMoveForComputer(computerMove);
            return computerMove;
        }

        private string indexToPosition(int i_RowIndex, int i_ColumnIndex)
        {
           return ((Convert.ToChar(i_ColumnIndex + 'A')).ToString() + (i_RowIndex + 1).ToString());
        }

        private Move checkIfRememberPair(Move i_FirstMove)
        {
            char ValueOfFirstCell = m_GameBoard.ValueInCell(i_FirstMove.RowIndex, i_FirstMove.ColumnIndex);
            Move newMoveOfComputer = null;
            if (m_LastMovesInTheGame.ContainsValue(ValueOfFirstCell))
            {
                foreach (Move previousMove in m_LastMovesInTheGame.Keys)
                {
                    if (!m_GameBoard.IsOccupancyTile(indexToPosition(previousMove.RowIndex,previousMove.ColumnIndex)) && m_LastMovesInTheGame[previousMove] == ValueOfFirstCell && (previousMove.RowIndex!= i_FirstMove.RowIndex || previousMove.ColumnIndex != i_FirstMove.ColumnIndex))
                    {
                        newMoveOfComputer = previousMove;
                        break;
                    }
                }
            }
            return newMoveOfComputer;
        }

        internal void RememberMoveForComputer(Move i_LastMoveOnGame)
        {
            if (m_LastMovesInTheGame == null)
            {
                m_LastMovesInTheGame = new Dictionary<Move, char>();
            }
            else  
            {
                if (m_LastMovesInTheGame.Count == m_LevelOfGame)
                {
                    m_LastMovesInTheGame.Remove(m_LastMovesInTheGame.Keys.First());
                }
                m_LastMovesInTheGame[i_LastMoveOnGame] = m_GameBoard.ValueInCell(i_LastMoveOnGame.RowIndex, i_LastMoveOnGame.ColumnIndex);
            }
            return;
        }

        private Move getComputerMove()
        {
            Random rnd = new Random();

            int MoveRow = rnd.Next(0, m_GameBoard.Length);
            int MoveColumn = rnd.Next(0, m_GameBoard.Width);

            bool isNotValidMove = this.m_GameBoard.IsOccupancyTile(indexToPosition(MoveRow, MoveColumn));

            while (isNotValidMove)
            { 
                MoveRow = rnd.Next(0, m_GameBoard.Length);
                MoveColumn = rnd.Next(0, m_GameBoard.Width);

                isNotValidMove = this.m_GameBoard.IsOccupancyTile((Convert.ToChar(MoveColumn + 'A')).ToString() + (MoveRow + 1).ToString());
            }

            return new Move(MoveRow, MoveColumn);
        }

        internal bool CheckValidMove(string i_Move)
        {
            bool isValidInput = true;

            // Check move size
            if (i_Move.Length != 2)
            {
                if (i_Move.Equals("Q"))
                {
                    this.m_IsQuite = true;
                }

                isValidInput = false;
            }

            else if (i_Move[0] < 'A' || i_Move[0] > this.m_GameBoard.Width + 'A' - 1)
            {
                isValidInput = false;
            }

            else if (i_Move[1] - 48 < 1 || i_Move[1] - 48 > this.m_GameBoard.Length)
            {
                isValidInput = false;
            }

            return isValidInput;
        }

        internal bool CheckIfIsOccupancyTile(string i_Move)
        {
            return (m_GameBoard.IsOccupancyTile(i_Move));
        }


        internal bool GameIsOver()
        {
            return (m_GameBoard.AllTilesAreFull());
        }
    }
}
