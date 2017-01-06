using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Play.Bingo.Client.Helper
{
    /// <summary>
    ///     Shuffle algorithm from:
    ///     http://stackoverflow.com/questions/1651619/optimal-linq-query-to-get-a-random-sub-collection-shuffle
    /// </summary>
    public static class EnumerableExtensions
    {
        public static void BubbleSort(this IList o)
        {
            for (var i = o.Count - 1; i >= 0; i--)
            {
                for (var j = 1; j <= i; j++)
                {
                    var o1 = o[j - 1];
                    var o2 = o[j];
                    if (((IComparable) o1).CompareTo(o2) > 0)
                    {
                        o.Remove(o1);
                        o.Insert(j, o1);
                    }
                }
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random(DateTime.Now.GetHashCode()));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (random == null) throw new ArgumentNullException(nameof(random));

            return source.ShuffleIterator(random);
        }

        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random random)
        {
            var buffer = source.ToList();
            for (var i = 0; i < buffer.Count; i++)
            {
                var j = random.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}