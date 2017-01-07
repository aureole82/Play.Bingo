using System.Collections.Generic;
using System.Linq;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.Services.Implementations
{
    internal class DesignStorageService : IStorageService
    {
        public void SaveCard(BingoCardModel card)
        {
        }

        public IDictionary<string, BingoCardModel> LoadCards()
        {
            return Enumerable.Range(1, 10)
                .Select(index => $"{index:0000}.card")
                .ToDictionary(key => key, _ => new BingoCardModel());
        }

        public BingoGameModel[] LoadGames()
        {
            return new[] {new BingoGameModel {Numbers = new List<int>(new[] {11, 13, 12, 16, 35, 75, 32, 14, 7})}};
        }

        public void SaveGame(BingoGameModel game)
        {
        }
    }
}