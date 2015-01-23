namespace Play.Bingo.Client.ViewModels
{
    public class BingoColumnViewModel : ViewModelBase
    {
        public BingoColumnViewModel() : this('N', new[] {33, 44, 0, 42, 37})
        {
        }

        public BingoColumnViewModel(char caption, int[] numbers)
        {
            Caption = caption;
            Numbers = numbers;
        }

        #region Bindable properties and commands.

        private char _caption;

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

        #endregion
    }
}