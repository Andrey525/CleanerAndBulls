using BullsAndCows.Model;
using BullsAndCows.Model.Players;

namespace BullsAndCowsTests
{
    [TestClass]
    public class GameModelTest
    {
        [TestMethod]
        public void TestDraw()
        {
            var _player1 = new PlayerMock();
            var _player2 = new PlayerMock();
            GameModel _gameModel = new GameModel(_player1, _player2);

            /* First value is our secret. Others - our answers */
            var moves = new List<string> { "1234", "5678", "9120", "0123" };
            var otherMoves = new List<string> { "0123", "9120", "5678", "1234" };
            _player1.Moves = moves;
            _player2.Moves = otherMoves;

            _gameModel.Run();

            Assert.IsTrue(_player1.Draw);
            Assert.IsTrue(_player2.Draw);

            Assert.IsFalse(_player1.WeLose);
            Assert.IsFalse(_player1.WeWon);
            Assert.IsFalse(_player2.WeLose);
            Assert.IsFalse(_player2.WeWon);
        }

        [TestMethod]
        public void TestFirstWin()
        {
            var _player1 = new PlayerMock();
            var _player2 = new PlayerMock();
            GameModel _gameModel = new GameModel(_player1, _player2);

            /* First value is our secret. Others - our answers */
            var moves = new List<string> { "1234", "5678", "9120", "0123" };
            var otherMoves = new List<string> { "0123", "9120", "5678", "6780" };
            _player1.Moves = moves;
            _player2.Moves = otherMoves;

            _gameModel.Run();

            Assert.IsTrue(_player1.WeWon);
            Assert.IsTrue(_player2.WeLose);

            Assert.IsFalse(_player1.WeLose);
            Assert.IsFalse(_player1.Draw);
            Assert.IsFalse(_player2.WeWon);
            Assert.IsFalse(_player2.Draw);
        }

        [TestMethod]
        public void TestFirstWinWithInvalidInput()
        {
            var _player1 = new PlayerMock();
            var _player2 = new PlayerMock();
            GameModel _gameModel = new GameModel(_player1, _player2);

            /* First value is our secret. Others - our answers */
            var moves = new List<string> { "1234", "5678", "awd1", "9120", "0123" };
            var otherMoves = new List<string> { "0123", "9120", "", "5678", "6780" };
            _player1.Moves = moves;
            _player2.Moves = otherMoves;

            _gameModel.Run();

            Assert.IsTrue(_player1.WeWon);
            Assert.IsTrue(_player2.WeLose);

            Assert.IsFalse(_player1.WeLose);
            Assert.IsFalse(_player1.Draw);
            Assert.IsFalse(_player2.WeWon);
            Assert.IsFalse(_player2.Draw);
        }
    }

    public class PlayerMock : Player
    {
        public bool WeStarted { get; set; }
        public bool WeWon { get; set; }
        public bool WeLose { get; set; }
        public bool Draw { get; set; }

        public List<string> Moves { get; set; }
        public override string? GetNumbers()
        {
            var str = Moves.First();
            Moves.RemoveAt(0);
            return string.Join("", str);
        }

        public override void Handle(string message, NotifyCode notifyCode)
        {
            switch (notifyCode)
            {
                case NotifyCode.YouStart:
                    WeStarted = true;
                    break;
                case NotifyCode.OpponentStart:
                    WeStarted = true;
                    break;
                case NotifyCode.YouLose:
                    WeLose = true;
                    break;
                case NotifyCode.YouWin:
                    WeWon = true;
                    break;
                case NotifyCode.Draw:
                    Draw = true;
                    break;
                default:
                    break;
            }
        }

        public override void UpdateHorneds(Horneds horneds)
        {
            return;
        }
    }
}