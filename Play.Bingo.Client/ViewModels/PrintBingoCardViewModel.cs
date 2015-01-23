using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Play.Bingo.Client.Helper;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services;

namespace Play.Bingo.Client.ViewModels
{
    public class PrintBingoCardViewModel : ViewModelBase
    {
        private const int PageSize = 2;
        private readonly IDictionary<string, BingoCardModel> _allCards;
        private readonly IMessageService _messenger = App.Messenger;
        private readonly IStorageService _storage = App.Storage;

        public PrintBingoCardViewModel()
        {
            PrintCommand = new RelayCommand(Print);
            PreviousCommand = new RelayCommand(Previous, CanPrevious);
            NextCommand = new RelayCommand(Next, CanNext);
            Cards = new ObservableCollection<BingoCardViewModel>();

            _allCards = IsInDesignMode
                ? CardGenerator.Generate(PageSize*2)
                    .Select((card, i) =>
                        new KeyValuePair<string, BingoCardModel>(string.Format("{0:D5}.card", i + 1), card))
                    .ToDictionary(pair => pair.Key, pair => pair.Value)
                : _storage.Load();

            TotalPages = Convert.ToInt32(Math.Ceiling((double) _allCards.Count/PageSize));
            FillPage();
        }

        #region Attached control to print.

        public static readonly DependencyProperty PrintAreaProperty = DependencyProperty.RegisterAttached(
            "PrintArea", typeof (Visual), typeof (PrintBingoCardViewModel), new PropertyMetadata(OnPrintAreaChanged));

        private static Visual _printArea;

        public static void OnPrintAreaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _printArea = obj as Visual;
        }

        public static void SetPrintArea(DependencyObject element, Visual value)
        {
            element.SetValue(PrintAreaProperty, value);
        }

        public static Visual GetPrintArea(DependencyObject element)
        {
            return (Visual) element.GetValue(PrintAreaProperty);
        }

        #endregion

        #region Bindable properties and commands.

        private int _page = 1;
        private int _totalPages = 1;
        public ObservableCollection<BingoCardViewModel> Cards { get; private set; }

        public RelayCommand PrintCommand { get; private set; }
        public RelayCommand PreviousCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }

        public int Page
        {
            get { return _page; }
            set
            {
                if (_page == value) return;
                _page = value;
                RaisePropertyChanged();
            }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            set
            {
                if (_totalPages == value) return;
                _totalPages = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Private helper methods.

        private void Print()
        {
            var printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(_printArea, "Bingo card");
            }

            if (CanNext()) Next();
        }


        private bool CanPrevious()
        {
            return Page > 1;
        }

        private void Previous()
        {
            Page--;
            FillPage();
        }

        private bool CanNext()
        {
            return Page < TotalPages;
        }

        private void Next()
        {
            Page++;
            FillPage();
        }

        private void FillPage()
        {
            Cards.Clear();
            foreach (var card in _allCards
                .Skip((Page - 1)*PageSize)
                .Take(PageSize)
                .Select(pair => new BingoCardViewModel(pair.Value, pair.Key)))
            {
                Cards.Add(card);
            }
        }

        #endregion
    }
}