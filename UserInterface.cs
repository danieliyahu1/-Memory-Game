using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace B20_Ex02_1
{
    public class UserInterface
    {
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private bool m_IsFirstGame = true;
        private bool m_IsComputerPlayer = false;
        private bool m_IsAnotherRound = true;

        // Handle User-Interaction
        public void Init()
        {
            if (m_IsFirstGame)
            {
                m_FirstPlayer = setFirstPlayer();
                m_SecondPlayer = setSecondPlayer();
                m_IsFirstGame = false;
            }

            while (m_IsAnotherRound)
            {
                int boardWidth = getBoardWidth();
                int boardLength = getBoardLength();
                int levelOfGame = 0;
                
                if ( m_IsComputerPlayer )
                {
                    levelOfGame = getLevelOfGame();
                }

                while (((boardLength * boardWidth) % 2 != 0) || ((boardLength * boardWidth) == 0))
                {
                    Console.WriteLine("Board Size is invalid (there should be even number of tiles), please try again");
                    boardWidth = getBoardWidth();
                    boardLength = getBoardLength();
                }

                Game game = new Game(m_FirstPlayer, m_SecondPlayer, m_IsComputerPlayer, boardWidth, boardLength, levelOfGame);
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(showBoard(game));
                
                while (!game.GameIsOver() && (!game.IsQuite))
                {
                    if (game.Turn == 0)
                    {
                        firstPlayerTurn(game);
                    }
                    else
                    {
                        secondPlayerTurn(game);
                    }
                }

                if (!game.IsQuite)
                {
                    Console.WriteLine(whoWon());
                    m_IsAnotherRound = isAnotherRound();
                }

                else
                {
                    m_IsAnotherRound = false;
                }

                Ex02.ConsoleUtils.Screen.Clear();
            }

            Console.WriteLine("Good Bye!");
            Thread.Sleep(2000);
        }

        private static Player setFirstPlayer()
        {
            Console.WriteLine("Player 1, please enter your name (1-15 characters):");
            string firstPlayerName = Console.ReadLine();

            bool isValidName = ((firstPlayerName.Length > 0) && (firstPlayerName.Length < 16));

            while (!isValidName)
            {
                Console.WriteLine("Invalid name, please try again.");
                firstPlayerName = Console.ReadLine();
                isValidName = ((firstPlayerName.Length > 0) && (firstPlayerName.Length < 16));
            }

            Player firstPlayer = new Player(firstPlayerName, 0);

            return firstPlayer;
        }

        private Player setSecondPlayer()
        {
            Console.WriteLine("Do you want play against computer or human? (press: 1 for computer or 2 for human)");
            string userInput = Console.ReadLine();

            // Checks that the entered preference is valid 
            while (!(userInput.Equals("1") || userInput.Equals("2")))
            {
                Console.WriteLine("Your input is invalid, please try again");
                userInput = Console.ReadLine();
            }

            this.m_IsComputerPlayer = userInput.Equals("1") ? true : false;

            string secondPlayerName;

            if (this.m_IsComputerPlayer)
            {
                secondPlayerName = "Computer";
            }

            else
            {
                Console.WriteLine("Second Player, please enter your name:");
                secondPlayerName = Console.ReadLine();

                bool isValidName = ((secondPlayerName.Length > 0) && (secondPlayerName.Length < 16));

                while (!isValidName)
                {
                    Console.WriteLine("Invalid name, please try again.");
                    secondPlayerName = Console.ReadLine();
                    isValidName = ((secondPlayerName.Length > 0) && (secondPlayerName.Length < 16));
                }
            }

            Player secondPlayer = new Player(secondPlayerName, 0);

            return secondPlayer;
        }

        private int getLevelOfGame()
        {
            int maxLevelOfGame = 5;
            int minLevelOfGame = 1;
            Console.WriteLine(string.Format("Please enter the level you want to play(1-5){0}{1} - human level{0}{2} - god of war level", Environment.NewLine,minLevelOfGame,maxLevelOfGame));
            string levelOfGame = Console.ReadLine();

            while (levelOfGame.Length != 1 || levelOfGame[0]-48 < minLevelOfGame || levelOfGame[0] - 48 > maxLevelOfGame)
            {
                Console.WriteLine(string.Format("Please enter the level you want to play{0}{1} - human level{0}{2} - god of war level", Environment.NewLine, minLevelOfGame, maxLevelOfGame));
                levelOfGame = Console.ReadLine();
            }
            return levelOfGame[0] - 48;
        }

        private static int getBoardWidth()
        {
            Console.WriteLine("Please enter the board width (4-6)");
            string userInput = Console.ReadLine();

            while(!(userInput.Equals("4") || userInput.Equals("5") || userInput.Equals("6")))
            {
                Console.WriteLine("Your input is invalid, please try again");
                userInput = Console.ReadLine();
            }

            int boardWidth;
            int.TryParse(userInput, out boardWidth);

            return boardWidth;
        }

        private static int getBoardLength()
        {
            Console.WriteLine("Please enter the board length (4-6)");
            string userInput = Console.ReadLine();

            while (!(userInput.Equals("4") || userInput.Equals("5") || userInput.Equals("6")))
            {
                Console.WriteLine("Your input is invalid, please try again");
                userInput = Console.ReadLine();
            }

            int boardLegth;
            int.TryParse(userInput, out boardLegth);

            return boardLegth;
        }

        private bool isAnotherRound()
        {
            Console.WriteLine("Do You want another round? (enter yes or no)");
            string userInput = Console.ReadLine().ToLower();

            // Checks that the entered preference is valid 
            while (!(userInput.Equals("yes") || userInput.Equals("no")))
            {
                Console.WriteLine("Your input is invalid, please try again.");
                userInput = Console.ReadLine().ToLower();
            }

            return (userInput.Equals("yes"));
        }

        private void firstPlayerTurn(Game game)
        {
            Console.Write(m_FirstPlayer.PlayerName + "'s turn: ");
            Move firstMove = getPlayerMove(game);
            if (game.IsQuite) { return; }
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(showMove(game, firstMove));

            Thread.Sleep(1000);
            Console.Write(m_FirstPlayer.PlayerName + "'s turn: ");
            Move secondMove = getPlayerMove(game);
            if (game.IsQuite) { return; }
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(showMove(game, secondMove));
            if (m_IsComputerPlayer)
            {
                game.RememberMoveForComputer(firstMove);
                game.RememberMoveForComputer(secondMove);
            }

            if (!(game.ExecuteMove(firstMove, secondMove)))
            {
                Thread.Sleep(2000);
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(showBoard(game));
            }

            else
            {
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(showBoard(game));
            }
        }

        private void secondPlayerTurn(Game game)
        {
            if (m_IsComputerPlayer == false)
            {
                Console.Write(m_SecondPlayer.PlayerName + "'s turn: ");
                Move firstMove = getPlayerMove(game);
                if (game.IsQuite) { return; }
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(showMove(game, firstMove));

                Console.Write(m_SecondPlayer.PlayerName + "'s turn: ");
                Move secondMove = getPlayerMove(game);
                if (game.IsQuite) { return; }
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(showMove(game, secondMove));

                if (!(game.ExecuteMove(firstMove, secondMove)))
                {
                    Thread.Sleep(2000);
                    Ex02.ConsoleUtils.Screen.Clear();
                    Console.WriteLine(showBoard(game));
                }

                else
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    Console.WriteLine(showBoard(game));
                }
            }

            else
            {
                Thread.Sleep(1000);
                Console.Write(m_SecondPlayer.PlayerName + "'s turn: ");
                Move firstMove = game.GetComputerAIMove();
                if (game.IsQuite) { return; }
                Thread.Sleep(1000);
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(showMove(game, firstMove));
                Thread.Sleep(1000);

                Console.Write(m_SecondPlayer.PlayerName + "'s turn: ");
                Move secondMove = game.GetComputerAIMove(firstMove);
                if (game.IsQuite) { return; }
                Thread.Sleep(1000);
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(showMove(game, secondMove));

                if (!(game.ExecuteMove(firstMove, secondMove)))
                {
                    Thread.Sleep(2000);
                    Ex02.ConsoleUtils.Screen.Clear();
                    Console.WriteLine(showBoard(game));
                }

                else
                {
                    Thread.Sleep(1000);
                    Ex02.ConsoleUtils.Screen.Clear();
                    Console.WriteLine(showBoard(game));
                    Thread.Sleep(1000);
                }
            }
        }

        private static Move getPlayerMove(Game i_Game)
        {
            Console.WriteLine("Please enter your move (For Example: A1)");
            string userInput = Console.ReadLine();

            bool isValidMove = i_Game.CheckValidMove(userInput);
            bool isOccupancyTile = false;

            if (isValidMove)
            {
                isOccupancyTile = i_Game.CheckIfIsOccupancyTile(userInput);
            }

            while ((!isValidMove || isOccupancyTile) && !i_Game.IsQuite)
            {
                if (!isValidMove)
                {
                    Console.WriteLine("Invalid move, please try again.");
                }
                else
                {
                    Console.WriteLine("Tile is occupied, please try again.");
                }
                userInput = Console.ReadLine();

                isValidMove = i_Game.CheckValidMove(userInput);
                if (isValidMove)
                {
                    isOccupancyTile = i_Game.CheckIfIsOccupancyTile(userInput);
                }
            }

            if (i_Game.IsQuite)
            {
                return new Move("Q0");
            }
            //Build move
            return new Move(userInput);
        }

        private StringBuilder showBoard(Game i_Game)
        {
            StringBuilder board = i_Game.GameBoard.BoardView();
            board.Append(Environment.NewLine);
            board.Append(showScores());
            board.Append(Environment.NewLine);

            return board;
        }

        private StringBuilder showScores()
        {
            StringBuilder scores = new StringBuilder();
            string firstPlayerScore = string.Format("{0} has: {1} points", m_FirstPlayer.PlayerName, m_FirstPlayer.Score);
            string seconfPlayerScore = string.Format("{0} has: {1} points", m_SecondPlayer.PlayerName, m_SecondPlayer.Score);
            scores.Append((firstPlayerScore));
            scores.Append(Environment.NewLine);
            scores.Append((seconfPlayerScore));

            return scores;
        }

        private StringBuilder showMove(Game i_Game, Move i_Move)
        {
            StringBuilder board = i_Game.GameBoard.ShowPlayerMove(i_Move.RowIndex, i_Move.ColumnIndex);
            board.Append(Environment.NewLine);
            board.Append(showScores());
            board.Append(Environment.NewLine);

            return board;
        }

        private string whoWon()
        {
            string message;
            string name;

            if (m_FirstPlayer.Score == m_SecondPlayer.Score)
            {
                message = "This round ended in a tie" + Environment.NewLine;
            }

            else
            {
                if (m_FirstPlayer.Score > m_SecondPlayer.Score)
                {
                    name = m_FirstPlayer.PlayerName;
                }
                else
                {
                    name = m_SecondPlayer.PlayerName;
                }

                message = string.Format("{0} has won this round!{1}", name, Environment.NewLine);
            }

            return message;
        }
    }
}
