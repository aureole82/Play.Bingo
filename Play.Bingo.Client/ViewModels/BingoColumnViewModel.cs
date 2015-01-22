namespace Play.Bingo.Client.ViewModels
{
    public class BingoColumnViewModel : ViewModelBase
    {
        private char _caption;

        public BingoColumnViewModel() : this('B', new[] {1, 4, 8, 12, 15})
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