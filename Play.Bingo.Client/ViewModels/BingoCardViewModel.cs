using System;
using System.Collections.Generic;
using System.Linq;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoCardViewModel : ViewModelBase
    {
        public BingoCardViewModel() : this(GenerateCard(new Random(DateTime.Now.Millisecond)))
        {
        }

        public BingoCardViewModel(BingoCardModel bingoCard)
        {
            Columns = new[]
            {
                new BingoColumnViewModel('B', bingoCard.B),
                new BingoColumnViewModel('I', bingoCard.I),
                new BingoColumnViewModel('N', bingoCard.N),
                new BingoColumnViewModel('G', bingoCard.G),
                new BingoColumnViewModel('O', bingoCard.O)
            };
        }

        public BingoColumnViewModel[] Columns { get; private set; }

        #region Private helper methods.

        private static BingoCardModel GenerateCard(Random random)
        {
            return new BingoCardModel
            {
                B = GenerateNumbers(0, 15, random),
                I = GenerateNumbers(15, 30, random),
                N = GenerateNumbers(30, 45, random),
                G = GenerateNumbers(45, 50, random),
                O = GenerateNumbers(60, 75, random)
            };
        }

        private static int[] GenerateNumbers(int from, int to, Random random)
        {
            var all = new List<int>();
            for (var i = from + 1; i < to + 1; i++)
            {
                all.Add(i);
            }
            return all
                .Shuffle(random)
                .Take(5)
                .ToArray();
        }

        #endregion
    }
}