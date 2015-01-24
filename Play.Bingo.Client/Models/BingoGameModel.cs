using System;
using System.Collections.Generic;

namespace Play.Bingo.Client.Models
{
    public class BingoGameModel
    {
        public BingoGameModel()
        {
            OpenedAt = DateTime.Now;
            Numbers = new List<int>();
        }

        public List<int> Numbers { get; set; }
        public DateTime OpenedAt { get; set; }
        public bool IsFinished { get; set; }
    }
}