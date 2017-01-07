using System;
using System.Collections.Generic;
using System.Linq;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.Helper
{
    /// <summary> Shuffle and provides the Bingo cards. </summary>
    public static class CardGenerator
    {
        /// <summary> Creates a new random Bingo card. </summary>
        public static BingoCardModel Generate(Random random = null)
        {
            random = random ?? new Random(DateTime.Now.Millisecond);
            return new BingoCardModel
            {
                B = GenerateNumbers(0, 15, random),
                I = GenerateNumbers(15, 30, random),
                N = GenerateNumbers(30, 45, random, true),
                G = GenerateNumbers(45, 60, random),
                O = GenerateNumbers(60, 75, random)
            };
        }

        /// <summary> Creates a couple of random Bingo cards at once. </summary>
        public static IEnumerable<BingoCardModel> Generate(int count, Random random = null)
        {
            if (count <= 0) yield break;

            random = random ?? new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < count; i++)
            {
                yield return Generate(random);
            }
        }

        #region Private helper methods.

        private static int[] GenerateNumbers(int @from, int to, Random random, bool hasFreeChance = false)
        {
            var all = new List<int>();
            for (var i = from + 1; i < to + 1; i++)
            {
                all.Add(i);
            }
            var numbers = all
                .Shuffle(random)
                .Take(5)
                .ToArray();

            if (hasFreeChance) numbers[2] = 0;

            return numbers;
        }

        #endregion
    }
}