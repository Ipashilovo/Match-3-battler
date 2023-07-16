using Match3Test.Core;

namespace Match3Test.Tests
{
    public class RefStructTest
    {
        [Test]
        public void TestRefStruct()
        {
            Span<int> ints = stackalloc int[5];
            GameData gameData = new GameData(ints);
            Assert.IsTrue(gameData.ints.Length == 5);
            ints = stackalloc int[6];
            gameData.ints = ints;
            Change3Int(gameData);
            Assert.IsTrue(gameData.ints.Length == 6);
            Assert.IsTrue(gameData.ints[3] == 7);
        }

        [Test]
        public void CheckSpan()
        {
            Span<int> ints = stackalloc int[3];
            SetSpan(ints);
            Assert.IsTrue(ints[0] == 17);
        }

        private void SetSpan(Span<int> ints)
        {
            ints[0] = 17;
        }
        private void Change3Int(GameData gameData)
        {
            gameData.ints[3] = 7;
            Span<int> span = stackalloc int[gameData.ints.Length];
            gameData.ints.CopyTo(span);
            span[3] = 9;
        }
        
        private ref struct GameData
        {
            public Span<int> ints;

            public GameData(Span<int> ints)
            {
                this.ints = ints;
            }
        }
    }
}