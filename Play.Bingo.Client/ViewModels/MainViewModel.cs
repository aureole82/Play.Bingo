using Play.Bingo.Client.Helper;

namespace Play.Bingo.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(Generate);
            Generate();
        }

        #region Bindable properties and commands.

        private BingoCardViewModel _bingoCard;

        public BingoCardViewModel BingoCard
        {
            get { return _bingoCard; }
            set
            {
                if (_bingoCard == value) return;
                _bingoCard = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GenerateCommand { get; private set; }

        #endregion

        #region Private helper methods.

        private void Generate()
        {
            BingoCard = new BingoCardViewModel();
        }

        #endregion
    }
}