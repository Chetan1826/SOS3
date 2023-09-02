namespace SOSGame
{
    class Program
    {
        static char[,] board;
        static char currentPlayer = 'S';
        static int playerSPoints = 0;
        static int playerOPoints = 0;
        static int Boardsize;
        static Stack<char[,]> undoStack = new Stack<char[,]>();
        static Stack<char[,]> redoStack = new Stack<char[,]>();
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to SOS Game!");

            while (true)
            {
                Console.Write("Choose an option: (P)lay, (H)elp, or (Q)uit: ");
                string option = Console.ReadLine().ToLower();

                if (option == "p")
                {

                    InitializeBoard();
                    PlayGame();

                }
                else if (option == "h")
                {
                    ShowHelp();
                }
                else if (option == "q")
                {
                    SaveGameState(0);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please select 'P' to play, 'H' for help, or 'Q' to quit.");
                }
            }

        }

        static void InitializeBoard()
        {
            int boardSize = GetBoardSize();
            board = new char[boardSize, boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        static void PlayGame()
        {

            Console.WriteLine("Player 1: S");

            bool vsComputer = false;
            Console.Write("Do you want to play against the computer? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                vsComputer = true;
            }
            else if (Console.ReadLine().ToLower() == "q")
            {
                SaveGameState(0);
                return;
            }
            else
            {
                Console.WriteLine("Player 2: O");
            }
            LoadGameState();
            int totalMoves = Boardsize * Boardsize;
            int moves = CountMoves();

            while (moves < totalMoves)
            {
                PrintBoard();
                Console.WriteLine("Options: (M)ove, (U)ndo, (R)edo");
                string moveOption = Console.ReadLine().ToLower();
                switch (moveOption)
                {
                    case "m":
                        int row, col;
                        if (vsComputer && currentPlayer == 'O')
                        {
                            (row, col) = GetComputerMove();
                        }
                        else
                        {
                            Console.WriteLine("Player S points : " + playerSPoints + " | Player O points : " + playerOPoints);
                            Console.Write($"Player {currentPlayer}, enter row (0-{board.GetLength(0) - 1}): ");
                            row = int.Parse(Console.ReadLine());
                            Console.Write($"Player {currentPlayer}, enter column (0-{board.GetLength(1) - 1}): ");
                            col = int.Parse(Console.ReadLine());
                        }

                        if (IsValidMove(row, col))
                        {
                            SaveGameState();
                            SaveGameHistory(row, col);

                            board[row, col] = currentPlayer;

                            int pointsScored = CountSOS(row, col);
                            if (pointsScored > 0)
                            {
                                if (currentPlayer == 'S')
                                    playerSPoints += pointsScored;
                                else
                                    playerOPoints += pointsScored;
                            }

                            moves++;

                            currentPlayer = (currentPlayer == 'S') ? 'O' : 'S';
                        }
                        else
                        {
                            Console.WriteLine("Invalid move. Try again.");
                        }
                        break;

                    case "u":
                        Undo();
                        break;

                    case "r":
                        Redo();
                        break;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }


            }

            PrintBoard();
            DetermineWinner();
        }

        static int GetBoardSize()
        {
            int boardSize;
            do
            {
                Console.Write("Enter the board size (between 3 and 10): ");
            } while (!int.TryParse(Console.ReadLine(), out boardSize) || boardSize < 3 || boardSize > 10);
            Boardsize = boardSize;
            return boardSize;
        }

        static void PrintBoard()
        {
            string boardnum = "";
            for (int i = 0; i < Boardsize; i++)
            {

                boardnum += "   " + i;
            }
            Console.WriteLine(boardnum);
            for (int i = 0; i < Boardsize; i++)
            {
                Console.Write($"{i} ");
                for (int j = 0; j < Boardsize; j++)
                {
                    Console.Write($" {board[i, j]} ");
                    if (j < Boardsize - 1) Console.Write("|");
                }
                Console.WriteLine();
                string divivder = "  ";
                for (int k = 0; k < Boardsize; k++)
                {
                    divivder += "---|";
                }
                if (i < Boardsize - 1) Console.WriteLine(divivder);
            }
        }

        static bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < Boardsize && col >= 0 && col < Boardsize && board[row, col] == ' ';
        }

        static int CountPoints(char player)
        {
            if (player == 'S')
            {
                playerSPoints++;
            }
            else
            {
                playerOPoints++;
            }
            return 0;
        }

        static int CountSOS(int row, int col)
        {
            int points = 0;

            if (col >= 2 && board[row, col - 1] == 'O' && board[row, col - 2] == 'S')
                CountPoints(currentPlayer);
            if (col >= 1 && col <= 1 && board[row, col - 1] == 'S' && board[row, col + 1] == 'S')
                CountPoints(currentPlayer);
            if (col <= 1 && board[row, col + 1] == 'O' && board[row, col + 2] == 'S')
                CountPoints(currentPlayer);

            if (row >= 2 && board[row - 1, col] == 'O' && board[row - 2, col] == 'S')
                CountPoints(currentPlayer);
            if (row >= 1 && row <= 1 && board[row - 1, col] == 'S' && board[row + 1, col] == 'S')
                CountPoints(currentPlayer);
            if (row <= 1 && board[row + 1, col] == 'O' && board[row + 2, col] == 'S')
                CountPoints(currentPlayer);

            if (row >= 2 && col >= 2 && board[row - 1, col - 1] == 'O' && board[row - 2, col - 2] == 'S')
                CountPoints(currentPlayer);
            if (row >= 1 && row <= 1 && col >= 1 && col <= 1 && board[row - 1, col - 1] == 'S' && board[row + 1, col + 1] == 'S')
                CountPoints(currentPlayer);
            if (row <= 1 && col <= 1 && board[row + 1, col + 1] == 'O' && board[row + 2, col + 2] == 'S')
                CountPoints(currentPlayer);

            if (row >= 2 && col <= 1 && board[row - 1, col + 1] == 'O' && board[row - 2, col + 2] == 'S')
                CountPoints(currentPlayer);
            if (row >= 1 && row <= 1 && col >= 1 && col <= 1 && board[row - 1, col + 1] == 'S' && board[row + 1, col - 1] == 'S')
                CountPoints(currentPlayer);
            if (row <= 1 && col >= 2 && board[row + 1, col - 1] == 'O' && board[row + 2, col - 2] == 'S')
                CountPoints(currentPlayer);

            return points;
        }

        static void DetermineWinner()
        {
            PrintScores();

            if (playerSPoints > playerOPoints)
            {
                Console.WriteLine($"Player S wins with {playerSPoints} points!");
                SaveGameState(0);
            }
            else if
                (playerOPoints > playerSPoints)
            {
                Console.WriteLine($"Player O wins with {playerOPoints} points!");
                SaveGameState(0);
            }
            else
            {
                Console.WriteLine("It's a tie!");
                SaveGameState(0);
            }
        }

        static void PrintScores()
        {
            Console.WriteLine($"Player S points: {playerSPoints}");
            Console.WriteLine($"Player O points: {playerOPoints}");
        }

        static int CountMoves()
        {
            int count = 0;
            foreach (char cell in board)
            {
                if (cell != ' ')
                {
                    count++;
                }
            }
            return count;
        }

        static void SaveGameState(int winner = 1)
        {

            if (winner == 0)
            {
                string authorsFile = "gamestate.txt";
                if (File.Exists(authorsFile))
                {
                    File.Delete(authorsFile);
                }
            }
            else
            {

                using (StreamWriter writer = new StreamWriter("gamestate.txt"))
                {
                    writer.WriteLine(currentPlayer);
                    writer.WriteLine(playerSPoints);
                    writer.WriteLine(playerOPoints);
                    for (int i = 0; i < Boardsize; i++)
                    {
                        for (int j = 0; j < Boardsize; j++)
                        {
                            writer.Write(board[i, j]);
                        }
                        writer.WriteLine();
                    }

                }
            }
        }

        static void SaveGameHistory(int row, int col)
        {
            char[,] copyBoard = new char[board.GetLength(0), board.GetLength(1)];
            Array.Copy(board, copyBoard, board.Length);
            undoStack.Push(copyBoard);
            redoStack.Clear();
        }

        static void LoadGameState()
        {
            if (File.Exists("gamestate.txt"))
            {
                using (StreamReader reader = new StreamReader("gamestate.txt"))
                {
                    currentPlayer = reader.ReadLine()[0];
                    playerSPoints = int.Parse(reader.ReadLine());
                    playerOPoints = int.Parse(reader.ReadLine());
                    for (int i = 0; i < Boardsize; i++)
                    {
                        string line = reader.ReadLine();
                        for (int j = 0; j < Boardsize; j++)
                        {
                            board[i, j] = line[j];
                        }
                    }
                }
            }
        }

        static (int, int) GetComputerMove()
        {
            Random rand = new Random();
            int row, col;
            do
            {
                row = rand.Next(Boardsize);
                col = rand.Next(Boardsize);
            } while (!IsValidMove(row, col));
            return (row, col);
        }



        static void Undo()
        {
            if (undoStack.Count > 1)
            {
                redoStack.Push(undoStack.Pop());
                char[,] prevState = undoStack.Peek();
                Array.Copy(prevState, board, prevState.Length);
                currentPlayer = (currentPlayer == 'S') ? 'O' : 'S';

                Console.WriteLine("Last move undone.");
            }
            else
            {
                Console.WriteLine("Cannot undo further.");
            }
        }

        static void Redo()
        {
            if (redoStack.Count > 0)
            {
                char[,] nextState = redoStack.Pop();
                undoStack.Push(nextState);
                Array.Copy(nextState, board, nextState.Length);
                currentPlayer = (currentPlayer == 'S') ? 'O' : 'S';

                Console.WriteLine("Last undone move redone.");
            }
            else
            {
                Console.WriteLine("Cannot redo further.");
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("SOS Game Help:");
            Console.WriteLine("In this game, players take turns adding 'S' or 'O' to the board.");
            Console.WriteLine("If a player creates the sequence 'SOS' vertically, horizontally, or diagonally,");
            Console.WriteLine("they earn a point and take another turn.");
            Console.WriteLine("After the grid is filled, the winner is determined by the points scored.");
            Console.WriteLine("To play, select 'P'. To view this help, select 'H'. To quit, select 'Q'.");
        }
    }
}
