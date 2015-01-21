using System.Collections.ObjectModel;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoColumnViewModel : ViewModelBase
    {
        private char _caption;

        public BingoColumnViewModel()
        {
            Numbers = new ObservableCollection<int>();

            if (!IsInDesignMode) return;
            Caption = 'B';
            Numbers = new ObservableCollection<int> {1, 4, 8, 12, 15};
        }

        public ObservableCollection<int> Numbers { get; private set; }

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
    }
}