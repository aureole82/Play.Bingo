using System.Collections.Generic;
using System.IO;
using System.Linq;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.Services
{
    /// <summary> Saves and loads Bingo cards into the subfolder "Storage". </summary>
    public class StorageService : IStorageService
    {
        private const string StorageFolder = "Storage";

        /// <summary> <see cref="IStorageService.Save" />. </summary>
        public void Save(BingoCardModel card)
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

        /// <summary> <see cref="IStorageService.Load" />. </summary>
        public IDictionary<string, BingoCardModel> Load()
        {
            var folder = GetFolder();
            var existingCards = folder.GetFiles("*.card");

            return existingCards
                .ToDictionary(
                    existingCard => existingCard.Name,
                    existingCard => new BingoCardModel(File.ReadAllBytes(existingCard.FullName)));
        }

        private static DirectoryInfo GetFolder()
        {
            var storageFolder = new DirectoryInfo(StorageFolder);
            if (!storageFolder.Exists) storageFolder.Create();

            return storageFolder;
        }
    }
}