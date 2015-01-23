using System;
using System.Globalization;
using System.Windows.Data;
using Play.Bingo.Client.ViewModels;

namespace Play.Bingo.Client.Helper
{
    public class CardToQrConverter : IValueConverter
    {
        /// <summary> Converts a BingoCardViewModel to a QrCodeViewModel. </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var card = value as BingoCardViewModel;
            return card == null ? null : new QrCodeViewModel {Data = card.Card.ToBinary()};
        }

        /// <summary> Not necessary! </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}