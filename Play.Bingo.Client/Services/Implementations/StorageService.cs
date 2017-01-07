using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.Services.Implementations
{
    /// <summary> Saves and loads Bingo cards into the subfolder "Storage". </summary>
    public class StorageService : IStorageService
    {
        private const string StorageFolder = "Storage";

        /// <summary> <see cref="IStorageService.SaveCard" />. </summary>
        public void SaveCard(BingoCardModel card)
        {
            var index = 1;
            var folder = GetFolder();
            var existingCards = folder.GetFiles("*.card");
            if (existingCards.Length > 0)
            {
                var last = existingCards
                    .Select(file => file.Name.Substring(0, 5))
                    .Last();
                index = int.Parse(last) + 1;
            }

            var pathToNewFile = Path.Combine(folder.FullName, string.Format("{0:D5}.card", index));
            using (var stream = File.OpenWrite(pathToNewFile))
            {
                var binary = card.ToBinary();
                stream.Write(binary, 0, binary.Length);
            }
        }

        /// <summary> <see cref="IStorageService.LoadCards" />. </summary>
        public IDictionary<string, BingoCardModel> LoadCards()
        {
            var folder = GetFolder();
            var existingCards = folder.GetFiles("*.card");

            return existingCards
                .ToDictionary(
                    existingCard => existingCard.Name,
                    existingCard => new BingoCardModel(File.ReadAllBytes(existingCard.FullName)));
        }

        public BingoGameModel[] LoadGames()
        {
            var games = new List<BingoGameModel>();
            var folder = GetFolder();
            var existingGames = folder.GetFiles("*.game");
            var timestampParser = new Regex(@"(\d{14}).game");
            foreach (var existingGame in existingGames)
            {
                var match = timestampParser.Match(existingGame.Name);
                if (match.Success)
                {
                    var binary = File.ReadAllBytes(existingGame.FullName);
                    if (binary.Length <= 1) continue;

                    games.Add(new BingoGameModel
                    {
                        OpenedAt =
                            DateTime.ParseExact(match.Groups[1].Value, "yyyyMMddhhmmss", CultureInfo.InvariantCulture),
                        IsFinished = binary[0] == 1,
                        Numbers = new List<int>(binary
                            .Skip(1)
                            .Select(b => (int) b)
                            .Where(n => n > 0 && n <= 75)
                            .Distinct()
                            .OrderBy(n => n))
                    });
                }
            }
            return games
                .OrderByDescending(g => g.OpenedAt)
                .ToArray();
        }

        public void SaveGame(BingoGameModel game)
        {
            var folder = GetFolder();
            var pathToFile = Path.Combine(folder.FullName, $"{game.OpenedAt:yyyyMMddhhmmss}.game");
            using (var stream = File.OpenWrite(pathToFile))
            {
                stream.WriteByte((byte) (game.IsFinished ? 1 : 0));
                var binary = game.Numbers.Distinct().OrderBy(n => n).Select(n => (byte) n).ToArray();
                stream.Write(binary, 0, binary.Length);
            }
        }

        #region Private helper methods.

        private static DirectoryInfo GetFolder()
        {
            var storageFolder = new DirectoryInfo(StorageFolder);
            if (!storageFolder.Exists) storageFolder.Create();

            return storageFolder;
        }

        #endregion
    }
}