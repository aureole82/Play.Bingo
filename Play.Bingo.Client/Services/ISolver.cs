using System.Collections.Generic;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Client.Services
{
    public interface ISolver
    {
        void AddRule(ISolverRule rule);
        void RemoveRule(ISolverRule rule);
        bool IsSolved(ICollection<int> numbers, BingoCardModel card);
    }

    public interface ISolverRule
    {
        bool Complies(ICollection<int> numbers, BingoCardModel card);
    }
}