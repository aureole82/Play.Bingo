using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoCardViewModel : ViewModelBase
    {
        public BingoCardViewModel(IQrService qrService) : this(CardGenerator.Generate(), "0001.card", qrService)
        {
        }

        public BingoCardViewModel(BingoCardModel bingoCard, string filename, IQrService qrService)
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
            Code = qrService?.Encode(bingoCard.ToBinary());
        }

        public void Mark(List<int> numbers)
        {
            foreach (var column in Columns)
            {
                column.Mark(numbers);
            }
        }

        #region Bindable properties and commands.

        private BingoCardModel _card;
        private BitmapSource _code;

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

        public BitmapSource Code
        {
            get { return _code; }
            set
            {
                if (Equals(_code, value)) return;
                _code = value;
                RaisePropertyChanged();
            }
        }

        public BingoColumnViewModel[] Columns { get; }

        public string Filename { get; private set; }

        public string Id => Convert.ToBase64String(Card.ToBinary());

        #endregion

        #region Private helper methods.

        #endregion
    }
}