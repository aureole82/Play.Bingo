using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Play.Bingo.Client.Helper
{
    public class TrimmedListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var objects = (values?.FirstOrDefault() as IEnumerable)?.OfType<object>().Reverse().ToList();
            if (objects == null) return null;

            int minimumLength;
            if (int.TryParse(parameter as string, out minimumLength) && (objects.Count <= minimumLength))
                return objects;

            var max = values.Skip(1)
                .OfType<int>()
                .Concat(new[] {minimumLength})
                .Max();

            return objects.Count > max
                ? objects.Take(max)
                : objects;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}