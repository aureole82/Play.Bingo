using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoGameViewModel : ViewModelBase
    {
        private readonly Stack<int> _inputs = new Stack<int>();
        private readonly IMessageService _messenger = App.Messenger;
        private readonly IStorageService _storage = App.Storage;

        public BingoGameViewModel()
            : this(new BingoGameModel {Numbers = new List<int>(new[] {11, 13, 12, 16, 35, 75, 32, 14, 7})})
        {
            LastNumber = O.Last();
        }

        public BingoGameViewModel(BingoGameModel game)
        {
            _messenger.Subscribe<Key>(KeyEntered);
            B = new ObservableCollection<int>();
            I = new ObservableCollection<int>();
            N = new ObservableCollection<int>();
            G = new ObservableCollection<int>();
            O = new ObservableCollection<int>();

            Game = game;

            foreach (var number in game.Numbers.Distinct())
            {
                AddNumber(number);
            }
        }

        #region Bindable properties and commands.

        private BingoGameModel _game;
        private int _lastNumber;
        public ObservableCollection<int> B { get; set; }
        public ObservableCollection<int> I { get; set; }
        public ObservableCollection<int> N { get; set; }
        public ObservableCollection<int> G { get; set; }
        public ObservableCollection<int> O { get; set; }

        public int LastNumber
        {
            get { return _lastNumber; }
            set
            {
                if (_lastNumber == value) return;
                _lastNumber = value;
                RaisePropertyChanged();
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

        private void KeyEntered(Key key)
        {
            int number;
            if (TryParse(key, out number))
            {
                if (LastNumber > 9)
                {
                    AddNumber(LastNumber);
                    LastNumber = number;
                }
                else
                {
                    var lastNumber = LastNumber*10 + number;
                    LastNumber = lastNumber;
                    new Thread(() =>
                    {
                        Thread.Sleep(5000);
                        if (LastNumber > 0 && lastNumber == LastNumber)
                        {
                            UiInvoke(() => AddNumber(lastNumber));
                            LastNumber = 0;
                        }
                    }).Start();
                }
            }

            if (key == Key.Tab)
            {
                AddNumber(LastNumber);
                LastNumber = 0;
                return;
            }
            if (key == Key.Back)
            {
                Reverse();
            }
        }

        private void Reverse()
        {
            if (LastNumber > 0)
            {
                LastNumber = 0;
                return;
            }
            var lastNumber = _inputs.Pop();
            var container = DetermineContainer(lastNumber);
            if (container.Contains(lastNumber))
            {
                container.Remove(lastNumber);
                Save();
            }
        }

        private static bool TryParse(Key key, out int number)
        {
            var value = (int) key;
            var isNumeric = value >= 34 && value <= 43;
            number = isNumeric ? value - 34 : default(int);
            return isNumeric;
        }


        private void AddNumber(int number)
        {
            var container = DetermineContainer(number);
            if (container != null)
                AddNumber(container, number);
        }

        private ObservableCollection<int> DetermineContainer(int number)
        {
            if (number <= 0) return null;
            if (number <= 15)
            {
                return B;
            }
            if (number <= 30)
            {
                return I;
            }
            if (number <= 45)
            {
                return N;
            }
            if (number <= 60)
            {
                return G;
            }
            if (number <= 75)
            {
                return O;
            }
            return null;
        }

        private void AddNumber(Collection<int> container, int number)
        {
            if (container.Contains(number)) return;
            container.Add(number);
            container.BubbleSort();
            _inputs.Push(number);
            Save();
        }

        private void Save()
        {
            Game.Numbers = B.Union(I).Union(N).Union(G).Union(O).ToList();
            _storage.SaveGame(Game);
        }

        #endregion
    }
}