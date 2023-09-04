namespace SOS3
{
    public class SaveState
    {
        public char _CurrentPlayer { get; set; }
        public int _PlayerSPoints { get; set; }
        public int _PlayerOPoints { get; set; }
        public char[,] _Board { get; set; }

        private int _Boardsize;
        public SaveState(char CurrentPlayer, int PlayerSPoints, int PlayerOPoints, char[,] Board, int boardsize)
        {
            _CurrentPlayer = CurrentPlayer;
            _PlayerSPoints = PlayerSPoints;
            _PlayerOPoints = PlayerOPoints;
            _Board = Board;
            _Boardsize = boardsize;
        }

        public void SaveGameState(int winner = 1)
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
                    writer.WriteLine(_CurrentPlayer);
                    writer.WriteLine(_PlayerSPoints);
                    writer.WriteLine(_PlayerOPoints);
                    for (int i = 0; i < _Boardsize; i++)
                    {
                        for (int j = 0; j < _Boardsize; j++)
                        {
                            writer.Write(_Board[i, j]);
                        }
                        writer.WriteLine();
                    }

                }
            }
        }

        public void LoadGameState()
        {
            if (File.Exists("gamestate.txt"))
            {
                using (StreamReader reader = new StreamReader("gamestate.txt"))
                {
                    _CurrentPlayer = reader.ReadLine()[0];
                    _PlayerSPoints = int.Parse(reader.ReadLine());
                    _PlayerOPoints = int.Parse(reader.ReadLine());
                    for (int i = 0; i < _Boardsize; i++)
                    {
                        string line = reader.ReadLine();
                        for (int j = 0; j < _Boardsize; j++)
                        {
                            _Board[i, j] = line[j];
                        }
                    }
                }
            }
        }
    }
}
