namespace SOS3
{
    public class Help
    {
        public void ShowHelp()
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
