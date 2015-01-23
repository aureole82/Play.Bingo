using System.Linq;
using System.Threading;
using System.Windows;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMessageService _messenger = App.Messenger;
        private readonly IStorageService _storage = App.Storage;

        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(Generate);
            SaveCommand = new RelayCommand(Save, CanSave);
            OpenCommand = new RelayCommand(Open);
            PrintPreviewCommand = new RelayCommand(PrintPreview);

            Generate();
            _messenger.Subscribe<BingoCardViewModel>(ShowCard);
        }

        #region Bindable properties and commands.

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel == value) return;
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GenerateCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand OpenCommand { get; private set; }
        public RelayCommand PrintPreviewCommand { get; private set; }

        #endregion

        #region Private helper methods.

        private bool _saved;

        private void Generate()
        {
            CurrentViewModel = new BingoCardViewModel();
            _saved = false;
        }

        private void ShowCard(BingoCardViewModel card)
        {
            CurrentViewModel = card;
            _saved = true;
        }

        private void Save()
        {
            var bingoCardViewModel = CurrentViewModel as BingoCardViewModel;
            if (bingoCardViewModel == null) return;

            _storage.Save(bingoCardViewModel.Card);
            _saved = true;
        }

        private bool CanSave()
        {
            return CurrentViewModel is BingoCardViewModel && !_saved;
        }

        private void Open()
        {
            var bingoCardSelector = new SelectBingoCardViewModel();
            CurrentViewModel = bingoCardSelector;

            new Thread(() =>
            {
                var bingoCards = _storage.Load();
                foreach (var card in bingoCards.Select(c => new BingoCardViewModel(c.Value, c.Key)))
                {
                    var local = card;
                    Application.Current.Dispatcher.Invoke(() => bingoCardSelector.Cards.Add(local));
                    // Just to get a context switch.
                    Thread.Sleep(10);
                }
            }).Start();
        }

        private void PrintPreview()
        {
            CurrentViewModel = new PrintBingoCardViewModel();
        }

        #endregion
    }
}