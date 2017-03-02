using System.Linq;
using System.Threading;
using System.Windows.Input;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;
using Play.Bingo.Client.Services.Implementations;

namespace Play.Bingo.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly CaptureQrCodeViewModel _captureQrCodeViewModel;
        private readonly IMessageService _messenger = new MessageService();
        private readonly IQrService _qrService = new QrService();
        private readonly ISolver _solver = new Solver();

        private readonly IStorageService _storage = IsInDesignMode
            ? (IStorageService) new DesignStorageService()
            : new StorageService();

        public MainViewModel()
        {
            _solver.AddRule(new ColumnRule());
            _solver.AddRule(new RowRule());
            _solver.AddRule(new DiagonalRule());
            //_solver.AddRule(new AllRule());

            GenerateCommand = new RelayCommand(Generate);
            OpenCommand = new RelayCommand(Open);
            ScanCommand = new RelayCommand(Scan);
            PrintPreviewCommand = new RelayCommand(PrintPreview);
            PlayCommand = new RelayCommand(Play);
            NewCommand = new RelayCommand(New);
            KeyEnteredCommand = new RelayCommand<Key>(KeyEntered);

            _messenger.Subscribe<BingoCardViewModel>(ShowCard);
            _captureQrCodeViewModel = new CaptureQrCodeViewModel(_messenger, _qrService);
            Play();
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
        public ICommand OpenCommand { get; private set; }
        public ICommand ScanCommand { get; private set; }
        public ICommand PrintPreviewCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand KeyEnteredCommand { get; private set; }

        #endregion

        #region Private helper methods.

        private BingoGameViewModel _currentGameViewModel;

        private void Generate()
        {
            var viewModel = new BingoCardViewModel(_qrService);

            _storage.SaveCard(viewModel.Card);
            CurrentViewModel = viewModel;
        }

        private void ShowCard(BingoCardViewModel card)
        {
            CurrentViewModel = card;
            if (_currentGameViewModel == null) return;

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

        private void Scan()
        {
            CurrentViewModel = _captureQrCodeViewModel;
        }

        private void Open()
        {
            var bingoCardSelector = new SelectBingoCardViewModel(_messenger);
            CurrentViewModel = bingoCardSelector;

            new Thread(() =>
            {
                var bingoCards = _storage.LoadCards();
                foreach (var card in bingoCards.Select(c => new BingoCardViewModel(c.Value, c.Key, _qrService)))
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
            CurrentViewModel = new PrintBingoCardViewModel(_storage, _qrService);
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

            CurrentViewModel =
                _currentGameViewModel =
                    new BingoGameViewModel(bingoGameModel ?? new BingoGameModel(), _messenger, _storage);
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
            if (key >= Key.D0 && key <= Key.D9)
            {
                number = key - Key.D0;
                return true;
            }
            if (key >= Key.NumPad0 && key <= Key.NumPad9)
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