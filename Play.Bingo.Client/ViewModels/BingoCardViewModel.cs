using System;
using System.Collections.Generic;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoCardViewModel : ViewModelBase
    {
        public BingoCardViewModel() : this(CardGenerator.Generate(), "0001.card")
        {
        }

        public BingoCardViewModel(BingoCardModel bingoCard, string filename)
        {
            Filename = filename;
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

        public string Filename { get; private set; }

        public string Id
        {
            get { return Convert.ToBase64String(Card.ToBinary()); }
        }

        #endregion

        #region Private helper methods.

        #endregion

        public void Mark(List<int> numbers)
        {
            foreach (var column in Columns)
            {
                column.Mark(numbers);
            }
        }
    }
}