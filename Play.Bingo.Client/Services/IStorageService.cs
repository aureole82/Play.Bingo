using System.Collections.Generic;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.Services
{
    /// <summary> Allows to persist Bingo cards. </summary>
    public interface IStorageService
    {
        /// <summary> Store the Bingo card somewhere. </summary>
        void Save(BingoCardModel card);

        /// <summary> Give me all the Bingo cards you've stored. </summary>
        IDictionary<string, BingoCardModel> Load();
    }
}