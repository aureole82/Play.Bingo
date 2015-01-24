using System.Collections.Generic;
using System.Linq;
using Play.Bingo.Client.Models;
using WPFMediaKit.DirectShow.Interop;

namespace Play.Bingo.Client.Services
{
    public interface ISolver
    {
        void AddRule(ISolverRule rule);
        void RemoveRule(ISolverRule rule);
        bool IsSolved(ICollection<int> numbers, BingoCardModel card);
    }

    public class Solver : ISolver
    {
        private readonly ICollection<ISolverRule> _rules = new List<ISolverRule>();

        public void AddRule(ISolverRule rule)
        {
            if (!_rules.Contains(rule)) _rules.Add(rule);
        }

        public void RemoveRule(ISolverRule rule)
        {
            if (_rules.Contains(rule)) _rules.Remove(rule);
        }

        public bool IsSolved(ICollection<int> numbers, BingoCardModel card)
        {
            var isSolved = false;
            foreach (var rule in _rules)
            {
                isSolved |= rule.Complies(numbers, card);
            }
            return isSolved;
        }
    }

    public interface ISolverRule
    {
        bool Complies(ICollection<int> numbers, BingoCardModel card);
    }

    public class ColumnRule : ISolverRule
    {
        public bool Complies(ICollection<int> numbers, BingoCardModel card)
        {
            if (Complies(numbers, card.B)) return true;
            if (Complies(numbers, card.I)) return true;
            if (Complies(numbers, card.N.Except(new []{0}).ToArray())) return true;
            if (Complies(numbers, card.G)) return true;
            if (Complies(numbers, card.O)) return true;
            return false;
        }

        private bool Complies(ICollection<int> numbers, int[] column)
        {
            return column.All(numbers.Contains);
        }
    }

    public class RowRule : ISolverRule
    {
        public bool Complies(ICollection<int> numbers, BingoCardModel card)
        {
            if (Complies(numbers, GetRow(card, 0))) return true;
            if (Complies(numbers, GetRow(card, 1))) return true;
            if (Complies(numbers, GetRow(card, 2))) return true;
            if (Complies(numbers, GetRow(card, 3))) return true;
            if (Complies(numbers, GetRow(card, 4))) return true;
            return false;
        }

        private int[] GetRow(BingoCardModel card, int index)
        {
            var row = new List<int>(new[] {card.B[index], card.I[index], card.G[index], card.O[index]});
            if (index != 2) row.Add(card.N[index]);
            return row.ToArray();
        }

        private bool Complies(ICollection<int> numbers, int[] row)
        {
            return row.All(numbers.Contains);
        }
    }

    public class DiagonalRule : ISolverRule
    {
        public bool Complies(ICollection<int> numbers, BingoCardModel card)
        {
            if (Complies(numbers, new[] {card.B[0], card.I[1], card.G[3], card.O[4]})) return true;
            if (Complies(numbers, new[] {card.B[4], card.I[3], card.G[1], card.O[0]})) return true;
            return false;
        }

        private bool Complies(ICollection<int> numbers, int[] row)
        {
            return row.All(numbers.Contains);
        }
    }
}