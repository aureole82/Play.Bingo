namespace Play.Bingo.Client.ViewModels
{
    public class BingoColumnViewModel : ViewModelBase
    {
        private char _caption;

        public BingoColumnViewModel() : this('N', new[] {33, 44, 0, 42, 37})
        {
        }

        public BingoColumnViewModel(char caption, int[] numbers)
        {
            Caption = caption;
            Numbers = numbers;
        }

        public int[] Numbers { get; private set; }

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