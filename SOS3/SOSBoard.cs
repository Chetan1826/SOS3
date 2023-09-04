namespace SOS3
{
    public class SOSBoard
    {
        private readonly char[,] _board;
        private readonly int _Boardsize;
        public SOSBoard(int boardsize, char[,] board)
        {
            _Boardsize = boardsize;
            _board = board;
        }
        public char[,] InitializeBoard()
        {
            char[,] board = new char[_Boardsize, _Boardsize];
            for (int i = 0; i < _Boardsize; i++)
            {
                for (int j = 0; j < _Boardsize; j++)
                {
                    board[i, j] = ' ';
                }
            }
            return board;
        }

        public void PrintBoard()
        {
            string boardnum = "";
            for (int i = 0; i < _Boardsize; i++)
            {

                boardnum += "   " + i;
            }
            Console.WriteLine(boardnum);
            for (int i = 0; i < _Boardsize; i++)
            {
                Console.Write($"{i} ");
                for (int j = 0; j < _Boardsize; j++)
                {
                    Console.Write($" {_board[i, j]} ");
                    if (j < _Boardsize - 1) Console.Write("|");
                }
                Console.WriteLine();
                string divivder = "  ";
                for (int k = 0; k < _Boardsize; k++)
                {
                    divivder += "---|";
                }
                if (i < _Boardsize - 1) Console.WriteLine(divivder);
            }
        }

        public int CountMoves()
        {
            int count = 0;
            foreach (char cell in _board)
            {
                if (cell != ' ')
                {
                    count++;
                }
            }
            return count;
        }
    }
}
