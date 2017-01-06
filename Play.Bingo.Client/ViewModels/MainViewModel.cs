using System.Linq;
using System.Threading;
using System.Windows.Input;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CaptureQrCodeViewModel _captureQrCodeViewModel= new CaptureQrCodeViewModel();
        private readonly IMessageService _messenger = App.Messenger;
        private readonly ISolver _solver = App.Solver;
        private readonly IStorageService _storage = App.Storage;

        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(Generate);
            SaveCommand = new RelayCommand(Save, CanSave);
            OpenCommand = new RelayCommand(Open);
            ScanCommand = new RelayCommand(Scan);
            PrintPreviewCommand = new RelayCommand(PrintPreview);
            PlayCommand = new RelayCommand(Play);
            NewCommand = new RelayCommand(New);
            KeyEnteredCommand = new RelayCommand<Key>(KeyEntered);

            Play();
            _messenger.Subscribe<BingoCardViewModel>(ShowCard);
        }

        #region Bindable properties and commands.

        private ViewModelBase _currentViewModel;

        private bool _isWinner;

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

        public bool IsWinner
        {
            get { return _isWinner; }
            set
            {
                if (_isWinner == value) return;
                _isWinner = value;
                RaisePropertyChanged();
            }
        }

        public ICommand GenerateCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand ScanCommand { get; private set; }
        public ICommand PrintPreviewCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand KeyEnteredCommand { get; private set; }

        #endregion

        #region Private helper methods.

        private BingoGameViewModel _currentGameViewModel;
        private bool _saved;

        private void Generate()
        {
            CurrentViewModel = new BingoCardViewModel();
            _saved = false;
        }

        private void ShowCard(BingoCardViewModel card)
        {
            CurrentViewModel = card;
            if (_currentGameViewModel != null)
            {
                card.Mark(_currentGameViewModel.Game.Numbers);
                IsWinner = _solver.IsSolved(_currentGameViewModel.Game.Numbers, card.Card);
                if (IsWinner)
                {
                    new Thread(() =>
                    {
                        Thread.Sleep(10000);
                        IsWinner = false;
                    }).Start();
                }
            }
            _saved = true;
        }

        private bool CanSave()
        {
            return CurrentViewModel is BingoCardViewModel && !_saved;
        }

        private void Save()
        {
            var bingoCardViewModel = CurrentViewModel as BingoCardViewModel;
            if (bingoCardViewModel == null) return;

            _storage.SaveCard(bingoCardViewModel.Card);
            _saved = true;
        }

        private void Scan()
        {
            CurrentViewModel = _captureQrCodeViewModel;
        }

        private void Open()
        {
            var bingoCardSelector = new SelectBingoCardViewModel();
            CurrentViewModel = bingoCardSelector;

            new Thread(() =>
            {
                var bingoCards = _storage.LoadCards();
                foreach (var card in bingoCards.Select(c => new BingoCardViewModel(c.Value, c.Key)))
                {
                    var local = card;
                    UiInvoke(() => bingoCardSelector.Cards.Add(local));
                    // Just to get a context switch.
                    Thread.Sleep(10);
                }
            }).Start();
        }

        private void PrintPreview()
        {
            CurrentViewModel = new PrintBingoCardViewModel();
        }

        private void New()
        {
            if (_currentGameViewModel != null)
            {
                _currentGameViewModel.Game.IsFinished = true;
                _currentGameViewModel.Save();
                _currentGameViewModel.Dispose();
                _currentGameViewModel = null;
            }

            var bingoGameModel = _storage.LoadGames()
                .Where(g => !g.IsFinished)
                .OrderByDescending(g => g.OpenedAt)
                .FirstOrDefault();

            CurrentViewModel = _currentGameViewModel = new BingoGameViewModel(bingoGameModel ?? new BingoGameModel());
        }

        private void Play()
        {
            if (_currentGameViewModel == null)
            {
                New();
                return;
            }

            if (!(CurrentViewModel is BingoGameViewModel))
            {
                CurrentViewModel = _currentGameViewModel;
                return;
            }

            _messenger.Publish(new RoundMessage(GameAction.Next));
        }

        private void KeyEntered(Key key)
        {
            int number;
            if (TryParse(key, out number))
            {
                _messenger.Publish(number);
                return;
            }

            switch (key)
            {
                case Key.Insert:
                    _messenger.Publish(new RoundMessage(GameAction.Next));
                    return;
                case Key.Back:
                    _messenger.Publish(new RoundMessage(GameAction.Revert));
                    return;
                case Key.Tab:
                    _messenger.Publish(new RoundMessage(GameAction.Accept));
                    return;
            }
        }

        private static bool TryParse(Key key, out int number)
        {
            if ((key >= Key.D0) && (key <= Key.D9))
            {
                number = key - Key.D0;
                return true;
            }
            if ((key >= Key.NumPad0) && (key <= Key.NumPad9))
            {
                number = key - Key.NumPad0;
                return true;
            }
            number = 0;
            return false;
        }

        #endregion
    }
}