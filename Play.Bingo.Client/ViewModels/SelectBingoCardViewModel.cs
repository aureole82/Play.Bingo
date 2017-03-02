using System.Collections.ObjectModel;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Services;
using Play.Bingo.Client.Services.Implementations;

namespace Play.Bingo.Client.ViewModels
{
    public class SelectBingoCardViewModel : ViewModelBase
    {
        private readonly IMessageService _messenger;

        public SelectBingoCardViewModel() : this(null)
        {
            // For design view only.
        }

        public SelectBingoCardViewModel(IMessageService messenger)
        {
            _messenger = messenger;
            SelectCardCommand = new RelayCommand<BingoCardViewModel>(SelectCard);
            Cards = new ObservableCollection<BingoCardViewModel>();

            if (!IsInDesignMode) return;

            for (var i = 0; i < 33; i++)
            {
                // The QR generation could be used for design data as well.
                Cards.Add(new BingoCardViewModel(new QrService()));
            }
        }

        #region Private helper methods.

        private void SelectCard(BingoCardViewModel card)
        {
            _messenger.Publish(card);
        }

        #endregion

        #region Bindable properties and commands.

        public ObservableCollection<BingoCardViewModel> Cards { get; }

        public RelayCommand<BingoCardViewModel> SelectCardCommand { get; private set; }

        #endregion
    }
}