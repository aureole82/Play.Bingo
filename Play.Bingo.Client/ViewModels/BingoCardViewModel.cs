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
            Card = bingoCard;
            Columns = new[]
            {
                new BingoColumnViewModel('B', Card.B),
                new BingoColumnViewModel('I', Card.I),
                new BingoColumnViewModel('N', Card.N),
                new BingoColumnViewModel('G', Card.G),
                new BingoColumnViewModel('O', Card.O)
            };
        }

        #region Bindable properties and commands.

        private BingoCardModel _card;
        public BingoColumnViewModel[] Columns { get; private set; }

        public BingoCardModel Card
        {
            get { return _card; }
            set
            {
                if (_card == value) return;
                _card = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Private helper methods.

        private static BingoCardModel GenerateCard(Random random)
        {
            return new BingoCardModel
            {
                B = GenerateNumbers(0, 15, random),
                I = GenerateNumbers(15, 30, random),
                N = GenerateNumbers(30, 45, random, true),
                G = GenerateNumbers(45, 50, random),
                O = GenerateNumbers(60, 75, random)
            };
        }

        private static int[] GenerateNumbers(int @from, int to, Random random, bool hasFreeChance = false)
        {
            var all = new List<int>();
            for (var i = from + 1; i < to + 1; i++)
            {
                all.Add(i);
            }
            var numbers = all
                .Shuffle(random)
                .Take(5)
                .ToArray();

            if (hasFreeChance) numbers[2] = 0;

            return numbers;
        }

        #endregion
    }
}