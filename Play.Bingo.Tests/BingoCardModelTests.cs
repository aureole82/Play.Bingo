using NUnit.Framework;
using Play.Bingo.Client.Models;

namespace Play.Bingo.Tests
{
    [TestFixture]
    public class BingoCardModelTests
    {
        [Test]
        public void When_I_have_a_regular_binary_array_I_can_generate_a_Bingo_card()
        {
            // Arrange.
            // Numbers: 1,2,3,4,5 - 16,17,18,19,20 - 31,32,33,34 - 46,47,48,49,50 - 61,62,63,64,65
            var binary = new byte[] {0x12, 0x34, 0x51, 0x23, 0x45, 0x12, 0x34, 0x12, 0x34, 0x51, 0x23, 0x45};

            // Act.
            var card = new BingoCardModel(binary);

            // Assert.
            CollectionAssert.AreEquivalent(new[] {1, 2, 3, 4, 5}, card.B);
            CollectionAssert.AreEquivalent(new[] {16, 17, 18, 19, 20}, card.I);
            CollectionAssert.AreEquivalent(new[] {31, 32, 0, 33, 34}, card.N);
            CollectionAssert.AreEquivalent(new[] {46, 47, 48, 49, 50}, card.G);
            CollectionAssert.AreEquivalent(new[] {61, 62, 63, 64, 65}, card.O);
        }

        [Test]
        public void When_I_have_a_regular_Bingo_card_I_can_serialize_it()
        {
            // Arrange.
            // Numbers: 1,2,3,4,5 - 16,17,18,19,20 - 31,32,33,34 - 46,47,48,49,50 - 61,62,63,64,65
            var card = new BingoCardModel
            {
                B = new[] {1, 2, 3, 4, 5},
                I = new[] {16, 17, 18, 19, 20},
                N = new[] {31, 32, 0, 33, 34},
                G = new[] {46, 47, 48, 49, 50},
                O = new[] {61, 62, 63, 64, 65}
            };

            // Act.
            var binary = card.ToBinary();

            // Assert.
            CollectionAssert.AreEquivalent(
                new byte[] {0x12, 0x34, 0x51, 0x23, 0x45, 0x12, 0x34, 0x12, 0x34, 0x51, 0x23, 0x45},
                binary);
        }
    }
}