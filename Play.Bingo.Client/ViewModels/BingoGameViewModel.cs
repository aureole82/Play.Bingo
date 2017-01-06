using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoGameViewModel : ViewModelBase, IDisposable
    {
        private readonly IDictionary<char, ObservableCollection<int>> _containers = new Dictionary
            <char, ObservableCollection<int>>
            {
                {'B', new ObservableCollection<int>()},
                {'I', new ObservableCollection<int>()},
                {'N', new ObservableCollection<int>()},
                {'G', new ObservableCollection<int>()},
                {'O', new ObservableCollection<int>()}
            };

        private readonly Stack<int> _inputs = new Stack<int>();
        private readonly IMessageService _messenger;
        private readonly IStorageService _storage;

        public BingoGameViewModel()
            : this(new BingoGameModel {Numbers = new List<int>(new[] {11, 13, 12, 16, 35, 75, 32, 14, 7})}
                , null, null)
        {
            AnnouncedNumber = O.Last();
        }

        public BingoGameViewModel(BingoGameModel game, IMessageService messenger, IStorageService storage)
        {
            _messenger = messenger;
            _storage = storage;

            Game = game;

            foreach (var number in game.Numbers.Distinct())
            {
                AnnouncedNumber = number;
                AddNumber();
            }

            if (IsInDesignMode) return;

            _messenger.Subscribe<int>(DigitEntered);
            _messenger.Subscribe<RoundMessage>(NextRound);
        }

        public void Dispose()
        {
            _messenger.Unsubscribe<int>(DigitEntered);
            _messenger.Unsubscribe<RoundMessage>(NextRound);
        }

        #region Bindable properties and commands.

        private BingoGameModel _game;
        private int _announcedNumber;
        public ObservableCollection<int> B => _containers['B'];
        public ObservableCollection<int> I => _containers['I'];
        public ObservableCollection<int> N => _containers['N'];
        public ObservableCollection<int> G => _containers['G'];
        public ObservableCollection<int> O => _containers['O'];

        public char? LetterOfAnnouncedNumber => AnnouncedNumber.GetLetter();

        public int AnnouncedNumber
        {
            get { return _announcedNumber; }
            set
            {
                if (_announcedNumber == value) return;
                _announcedNumber = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(LetterOfAnnouncedNumber));
            }
        }

        public BingoGameModel Game
        {
            get { return _game; }
            set
            {
                if (_game == value) return;
                _game = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Private helper methods.

        private void DigitEntered(int digit)
        {
            if (AnnouncedNumber > 9)
            {
                AddNumber();
                AnnouncedNumber = digit;
            }
            else
            {
                AnnouncedNumber = AnnouncedNumber*10 + digit;
                if (AnnouncedNumber > 75)
                {
                    AnnouncedNumber = 0;
                    return;
                }
                AddNumberIn5Seconds();
            }
        }

        private void NextRound(RoundMessage message)
        {
            switch (message.Action)
            {
                case GameAction.Next:
                    if (AnnouncedNumber > 0) AddNumber();
                    AnnouncedNumber = Next();
                    if (AnnouncedNumber > 0) AddNumberIn5Seconds();
                    return;
                case GameAction.Accept:
                    AddNumber();
                    return;
                case GameAction.Revert:
                    Reverse();
                    break;
            }
        }

        private readonly IReadOnlyCollection<int> _allNumbers =
            new ReadOnlyCollection<int>(Enumerable.Range(1, 75).ToList());

        private readonly Random _random = new Random(DateTime.Now.Ticks.GetHashCode());
        private Thread _thread;

        private int Next()
        {
            var next = _allNumbers
                .Except(_game.Numbers)
                .Shuffle(_random)
                .FirstOrDefault();
            return next;
        }

        private void AddNumberIn5Seconds()
        {
            _thread?.Abort();
            _thread = new Thread(() =>
            {
                Thread.Sleep(5000);
                if (AnnouncedNumber <= 0) return;

                UiInvoke(AddNumber);
                _thread = null;
            });
            _thread.Start();
        }

        private void Reverse()
        {
            if (AnnouncedNumber > 0)
            {
                AnnouncedNumber = 0;
                return;
            }
            if (_inputs.Count == 0) return;

            var lastNumber = _inputs.Pop();
            var letter = lastNumber.GetLetter();
            if (!letter.HasValue) return;

            var container = _containers[letter.Value];
            if (container.Contains(lastNumber))
            {
                container.Remove(lastNumber);
                Save();
            }
        }

        private void AddNumber()
        {
            var letter = AnnouncedNumber.GetLetter();
            if (!letter.HasValue) return;

            var container = _containers[letter.Value];
            if (container != null)
                AddNumber(container, AnnouncedNumber);
            AnnouncedNumber = 0;
        }

        private void AddNumber(Collection<int> container, int number)
        {
            if (container.Contains(number)) return;

            Console.WriteLine($@"{nameof(AddNumber)}({number})");
            container.Add(number);
            container.BubbleSort();
            _inputs.Push(number);
            Save();
        }

        public void Save()
        {
            Game.Numbers = B.Union(I).Union(N).Union(G).Union(O).ToList();
            _storage?.SaveGame(Game);
        }

        #endregion
    }

    internal class RoundMessage
    {
        public RoundMessage(GameAction action)
        {
            Action = action;
        }

        public GameAction Action { get; }
    }

    internal enum GameAction
    {
        Accept,
        Next,
        Revert
    }
}