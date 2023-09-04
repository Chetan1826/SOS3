using SOS3;

namespace SOSGame
{
    public class Program
    {
        private readonly char[,] _board;
        private readonly char _currentPlayer = 'S';
        private readonly int _playerSPoints = 0;
        private readonly int _playerOPoints = 0;
        private readonly int _boardsize;

        private readonly Stack<char[,]> _undoStack = new();
        private readonly Stack<char[,]> _redoStack = new();

        private readonly SOSBoard _sosBoard;
        private readonly SaveState _saveState;
        private readonly Help _help;
        private readonly GameController _controller;

        public Program()
        {
            _sosBoard = new SOSBoard(_boardsize, _board);
            _saveState = new SaveState(_currentPlayer, _playerSPoints, _playerOPoints, _board, _boardsize);
            _help = new Help();
            _controller = new GameController(_currentPlayer, _playerSPoints, _playerOPoints, _board, _boardsize,
            _undoStack, _redoStack);
        }

        public void Main(string[] args)
        {
            Console.WriteLine("Welcome to SOS Game!");

            while (true)
            {
                Console.Write("Choose an option: (P)lay, (H)elp, or (Q)uit: ");
                string option = Console.ReadLine().ToLower();

                if (option == "p")
                {

                    _sosBoard.InitializeBoard();
                    _controller.PlayGame();

                }
                else if (option == "h")
                {
                    _help.ShowHelp();
                }
                else if (option == "q")
                {
                    _saveState.SaveGameState(0);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please select 'P' to play, 'H' for help, or 'Q' to quit.");
                }
            }

        }
    }
}
