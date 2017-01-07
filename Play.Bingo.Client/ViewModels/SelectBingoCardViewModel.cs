using System.Collections.ObjectModel;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Services;

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
                Cards.Add(new BingoCardViewModel());
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