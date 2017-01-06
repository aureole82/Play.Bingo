namespace Play.Bingo.Client.Helper
{
    internal static class BingoExtensions
    {
        public static char? GetLetter(this int number)
        {
            if (number <= 0) return null;
            if (number <= 15)
            {
                return 'B';
            }
            if (number <= 30)
            {
                return 'I';
            }
            if (number <= 45)
            {
                return 'N';
            }
            if (number <= 60)
            {
                return 'G';
            }
            if (number <= 75)
            {
                return 'O';
            }
            return null;
        }
    }
}