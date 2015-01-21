using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Play.Bingo.Client.Helper;

namespace Play.Bingo.Client.ViewModels
{
    public class BingoCardViewModel : ViewModelBase
    {
        public BingoCardViewModel()
        {
            var random = new Random(DateTime.Now.Millisecond);
            Columns = new ObservableCollection<BingoColumnViewModel>
            {
                new BingoColumnViewModel {Caption = 'B'},
                new BingoColumnViewModel {Caption = 'I'},
                new BingoColumnViewModel {Caption = 'N'},
                new BingoColumnViewModel {Caption = 'G'},
                new BingoColumnViewModel {Caption = 'O'}
            };

            for (var index = 0; index < Columns.Count; index++)
            {
                var column = Columns[index];
                column.Numbers.Clear();
                foreach (var number in GenerateNumbers(index*15, (index + 1)*15, random))
                {
                    column.Numbers.Add(number);
                }
            }
        }

        public ObservableCollection<BingoColumnViewModel> Columns { get; private set; }

        private static IEnumerable<int> GenerateNumbers(int from, int to, Random random)
        {
            var all = new List<int>();
            for (var i = from + 1; i < to + 1; i++)
            {
                all.Add(i);
            }
            return all
                .Shuffle(random)
                .Take(5);
        }
    }
}