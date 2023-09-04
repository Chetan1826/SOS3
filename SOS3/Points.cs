namespace SOS3
{
    public class Points
    {
        private char _currentPlayer { get; set; }
        private int _playerSPoints { get; set; }
        private int _playerOPoints { get; set; }
        private char[,] _board { get; set; }
        private readonly int _Boardsize;
        private SaveState _saveState;
        public Points(char currePlayer, int playerSPoints, int PlayerOPoints, char[,] Board, int boardsize)
        {
            _currentPlayer = currePlayer;
            _playerSPoints = playerSPoints;
            _playerOPoints = PlayerOPoints;
            _board = Board;
            _Boardsize = boardsize;
            _saveState = new SaveState(_currentPlayer, playerSPoints, _playerOPoints, _board, _Boardsize);
        }
        public int CountPoints(char player)
        {
            if (player == 'S')
            {
                _playerSPoints++;
            }
            else
            {
                _playerOPoints++;
            }
            return 0;
        }

        public int CountSOS(int row, int col)
        {
            int points = 0;

            if (col >= 2 && _board[row, col - 1] == 'O' && _board[row, col - 2] == 'S')
                CountPoints(_currentPlayer);
            if (col >= 1 && col <= 1 && _board[row, col - 1] == 'S' && _board[row, col + 1] == 'S')
                CountPoints(_currentPlayer);
            if (col <= 1 && _board[row, col + 1] == 'O' && _board[row, col + 2] == 'S')
                CountPoints(_currentPlayer);

            if (row >= 2 && _board[row - 1, col] == 'O' && _board[row - 2, col] == 'S')
                CountPoints(_currentPlayer);
            if (row >= 1 && row <= 1 && _board[row - 1, col] == 'S' && _board[row + 1, col] == 'S')
                CountPoints(_currentPlayer);
            if (row <= 1 && _board[row + 1, col] == 'O' && _board[row + 2, col] == 'S')
                CountPoints(_currentPlayer);

            if (row >= 2 && col >= 2 && _board[row - 1, col - 1] == 'O' && _board[row - 2, col - 2] == 'S')
                CountPoints(_currentPlayer);
            if (row >= 1 && row <= 1 && col >= 1 && col <= 1 && _board[row - 1, col - 1] == 'S' && _board[row + 1, col + 1] == 'S')
                CountPoints(_currentPlayer);
            if (row <= 1 && col <= 1 && _board[row + 1, col + 1] == 'O' && _board[row + 2, col + 2] == 'S')
                CountPoints(_currentPlayer);

            if (row >= 2 && col <= 1 && _board[row - 1, col + 1] == 'O' && _board[row - 2, col + 2] == 'S')
                CountPoints(_currentPlayer);
            if (row >= 1 && row <= 1 && col >= 1 && col <= 1 && _board[row - 1, col + 1] == 'S' && _board[row + 1, col - 1] == 'S')
                CountPoints(_currentPlayer);
            if (row <= 1 && col >= 2 && _board[row + 1, col - 1] == 'O' && _board[row + 2, col - 2] == 'S')
                CountPoints(_currentPlayer);

            return points;
        }

        public void DetermineWinner()
        {
            PrintScores();

            if (_playerSPoints > _playerOPoints)
            {
                Console.WriteLine($"Player S wins with {_playerSPoints} points!");
                _saveState.SaveGameState(0);
            }
            else if
                (_playerSPoints > _playerOPoints)
            {
                Console.WriteLine($"Player O wins with {_playerOPoints} points!");
                _saveState.SaveGameState(0);
            }
            else
            {
                Console.WriteLine("It's a tie!");
                _saveState.SaveGameState(0);
            }
        }

        public void PrintScores()
        {
            Console.WriteLine($"Player S points: {_playerSPoints}");
            Console.WriteLine($"Player O points: {_playerOPoints}");
        }
    }
}
