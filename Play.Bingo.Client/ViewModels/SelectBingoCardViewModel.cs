using System.Collections.ObjectModel;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class SelectBingoCardViewModel : ViewModelBase
    {
        private readonly IMessageService _messenger = App.Messenger;

        public SelectBingoCardViewModel()
        {
            SelectCardCommand = new RelayCommand<BingoCardModel>(SelectCard);
            Cards = new ObservableCollection<BingoCardViewModel>();

            if (!IsInDesignMode) return;

            for (var i = 0; i < 33; i++)
            {
                Cards.Add(new BingoCardViewModel());
            }
        }

        #region Bindable properties and commands.

        public ObservableCollection<BingoCardViewModel> Cards { get; private set; }

        public RelayCommand<BingoCardModel> SelectCardCommand { get; private set; }

        #endregion

        #region Private helper methods.

        private void SelectCard(BingoCardModel card)
        {
            _messenger.Publish(card);
        }

        #endregion
    }
}