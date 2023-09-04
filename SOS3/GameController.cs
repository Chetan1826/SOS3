namespace SOS3
{
    public class GameController
    {
        public char _CurrentPlayer { get; set; }
        public int _PlayerSPoints { get; set; }
        public int _PlayerOPoints { get; set; }
        public char[,] _Board { get; set; }

        private int _Boardsize;


        private Stack<char[,]> _undoStack = new Stack<char[,]>();
        private Stack<char[,]> _redoStack = new Stack<char[,]>();

        private SaveState _saveState;

        private SOSBoard _sosBoard;
        private Move _move;
        private Points _points;
        public GameController(char CurrentPlayer, int PlayerSPoints, int PlayerOPoints, char[,] Board, int boardsize,
            Stack<char[,]> undoStack, Stack<char[,]> redoStack)
        {
            _CurrentPlayer = CurrentPlayer;
            _PlayerSPoints = PlayerSPoints;
            _PlayerOPoints = PlayerOPoints;
            _Board = Board;
            _Boardsize = boardsize;
            _undoStack = undoStack;
            _redoStack = redoStack;
            _saveState = new SaveState(_CurrentPlayer, _PlayerSPoints, _PlayerOPoints, _Board, _Boardsize);
            _sosBoard = new SOSBoard(_Boardsize, _Board);
            _move = new Move(_Boardsize, _Board);
            _points = new Points(_CurrentPlayer, _PlayerSPoints, _PlayerOPoints, _Board, _Boardsize);
        }

        public void PlayGame()
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
                _saveState.SaveGameState(0);
                return;
            }
            else
            {
                Console.WriteLine("Player 2: O");
            }
            _saveState.LoadGameState();
            int totalMoves = _Boardsize * _Boardsize;
            int moves = _sosBoard.CountMoves();

            while (moves < totalMoves)
            {
                _sosBoard.PrintBoard();
                Console.WriteLine("Options: (M)ove, (U)ndo, (R)edo");
                string moveOption = Console.ReadLine().ToLower();
                switch (moveOption)
                {
                    case "m":
                        int row, col;
                        if (vsComputer && _CurrentPlayer == 'O')
                        {
                            (row, col) = _move.GetComputerMove();
                        }
                        else
                        {
                            Console.WriteLine("Player S points : " + _PlayerSPoints + " | Player O points : " + _PlayerOPoints);
                            Console.Write($"Player {_CurrentPlayer}, enter row (0-{_Board.GetLength(0) - 1}): ");
                            row = int.Parse(Console.ReadLine());
                            Console.Write($"Player {_CurrentPlayer}, enter column (0-{_Board.GetLength(1) - 1}): ");
                            col = int.Parse(Console.ReadLine());
                        }

                        if (_move.IsValidMove(row, col))
                        {
                            _saveState.SaveGameState();
                            SaveGameHistory(row, col);

                            _Board[row, col] = _CurrentPlayer;

                            int pointsScored = _points.CountSOS(row, col);
                            if (pointsScored > 0)
                            {
                                if (_CurrentPlayer == 'S')
                                    _PlayerSPoints += pointsScored;
                                else
                                    _PlayerOPoints += pointsScored;
                            }

                            moves++;

                            _CurrentPlayer = (_CurrentPlayer == 'S') ? 'O' : 'S';
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

            _sosBoard.PrintBoard();
            _points.DetermineWinner();
        }

        public void SaveGameHistory(int row, int col)
        {
            char[,] copyBoard = new char[_Board.GetLength(0), _Board.GetLength(1)];
            Array.Copy(_Board, copyBoard, _Board.Length);
            _undoStack.Push(copyBoard);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (_undoStack.Count > 1)
            {
                _redoStack.Push(_undoStack.Pop());
                char[,] prevState = _undoStack.Peek();
                Array.Copy(prevState, _Board, prevState.Length);
                _CurrentPlayer = (_CurrentPlayer == 'S') ? 'O' : 'S';

                Console.WriteLine("Last move undone.");
            }
            else
            {
                Console.WriteLine("Cannot undo further.");
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                char[,] nextState = _redoStack.Pop();
                _undoStack.Push(nextState);
                Array.Copy(nextState, _Board, nextState.Length);
                _CurrentPlayer = (_CurrentPlayer == 'S') ? 'O' : 'S';

                Console.WriteLine("Last undone move redone.");
            }
            else
            {
                Console.WriteLine("Cannot redo further.");
            }
        }
    }
}
