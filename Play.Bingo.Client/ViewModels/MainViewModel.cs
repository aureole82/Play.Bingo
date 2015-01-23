using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IStorageService _storage = new StorageService();

        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(Generate);
            SaveCommand = new RelayCommand(Save, CanSave);
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
        public RelayCommand SaveCommand { get; private set; }

        #endregion

        #region Private helper methods.

        private bool _saved;

        private void Generate()
        {
            BingoCard = new BingoCardViewModel();
            _saved = false;
        }

        private void Save()
        {
            _storage.Save(BingoCard.Card);
            _saved = true;
        }

        private bool CanSave()
        {
            return !_saved;
        }

        #endregion
    }
}