using System.Collections.Generic;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.Services
{
    /// <summary> Allows to persist Bingo cards. </summary>
    public interface IStorageService
    {
        /// <summary> Store the Bingo card somewhere. </summary>
        void SaveCard(BingoCardModel card);

        /// <summary> Give me all the Bingo cards you've stored. </summary>
        IDictionary<string, BingoCardModel> LoadCards();

        /// <summary> Loads the last game. </summary>
        BingoGameModel[] LoadGames();

        /// <summary> Save game. </summary>
        void SaveGame(BingoGameModel game);
    }
}