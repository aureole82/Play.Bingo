using System.Collections.Generic;
using System.Linq;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoColumnViewModel : ViewModelBase
    {
        public BingoColumnViewModel() : this('N', new[] {33, 44, 0, 42, 37})
        {
        }

        public BingoColumnViewModel(char caption, IEnumerable<int> numbers)
        {
            Caption = caption;
            Numbers = numbers.Select(n => new BingoNumber {Number = n}).ToArray();

            if (!IsInDesignMode)return;
            Numbers.Skip(1).First().IsMarked = true;
        }

        #region Bindable properties and commands.

        private char _caption;

        public BingoNumber[] Numbers { get; private set; }

        public char Caption
        {
            get { return _caption; }
            set
            {
                if (_caption == value) return;
                _caption = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public void Mark(IEnumerable<int> markedNumbers)
        {
            foreach (var markedNumber in markedNumbers)
            {
                var found = Numbers.FirstOrDefault(n => n.Number == markedNumber);
                if (found != null) found.IsMarked = true;
            }
        }
    }

    public class BingoNumber
    {
        public int Number { get; set; }
        public bool IsMarked { get; set; }
    }
}