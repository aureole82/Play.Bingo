using System.Collections.Generic;
using NUnit.Framework;
using Play.Bingo.Client.Models;
using Play.Bingo.Client.Services.Implementations;

namespace Play.Bingo.Tests
{
    [TestFixture]
    public class QrTests
    {
        private static IEnumerable<BingoCardModel> _cards = new List<BingoCardModel>
        {
            new BingoCardModel
            {
                B = new[] {1, 2, 3, 4, 5},
                I = new[] {16, 17, 18, 19, 20},
                N = new[] {31, 32, 33, 34},
                G = new[] {46, 47, 48, 49, 50},
                O = new[] {61, 62, 63, 64, 65}
            },
            new BingoCardModel
            {
                B = new[] {5, 6, 4, 15, 2},
                I = new[] {18, 25, 24, 20, 17},
                N = new[] {32, 39, 45, 37},
                G = new[] {52, 55, 50, 48, 59},
                O = new[] {69, 65, 71, 64, 73}
            },
            new BingoCardModel
            {
                B = new[] {8, 6, 11, 3, 1},
                I = new[] {23, 27, 25, 29, 28},
                N = new[] {34, 44, 35, 45},
                G = new[] {60, 53, 55, 59, 47},
                O = new[] {67, 68, 69, 75, 70}
            },
            new BingoCardModel
            {
                B = new[] {9, 2, 12, 10, 3},
                I = new[] {24, 17, 19, 27, 22},
                N = new[] {33, 44, 34, 35},
                G = new[] {47, 46, 50, 49, 48},
                O = new[] {68, 63, 61, 64, 73}
            }
        };

        [Test]
        [TestCaseSource(nameof(_cards))]
        public void When_I_generate_QR_I_can_read_it(BingoCardModel card)
        {
            var qrService = new QrService();
            var qrCode = qrService.Generate(card.ToBinary());
            var data = qrService.Decode(qrCode);
            var card2 = new BingoCardModel(data);

            Assert.AreEqual(card, card2);
        }
    }
}