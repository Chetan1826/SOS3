namespace SOS3
{
    public class Move
    {
        private int _boardSize;
        private char[,] _board;
        public Move(int boardSize, char[,] board)
        {
            _boardSize = boardSize;
            _board = board;
        }

        public (int, int) GetComputerMove()
        {
            Random rand = new Random();
            int row, col;
            do
            {
                row = rand.Next(_boardSize);
                col = rand.Next(_boardSize);
            } while (!IsValidMove(row, col));
            return (row, col);
        }

        public bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < _boardSize && col >= 0 && col < _boardSize && _board[row, col] == ' ';
        }
    }
}
