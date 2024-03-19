using BullsAndCows.Model.Helpers;

namespace BullsAndCowsTests
{
    [TestClass]
    public class MessageParserTest
    {
        List<Tuple<string, int[]>> _validCases = new List<Tuple<string, int[]>>
        {
            Tuple.Create("1234", new int[]{1, 2, 3, 4}),
            Tuple.Create("0234", new int[]{0, 2, 3, 4}),
            Tuple.Create("5678", new int[]{5, 6, 7, 8}),
            Tuple.Create("1379", new int[]{1, 3, 7, 9})
        };

        List<string> _errorCases = new List<string>
        {
            "1123",
            "0000",
            "123d",
            "",
            "123"
        };

        [TestMethod]
        public void ValidValues()
        {
            foreach (var pair in _validCases)
            {
                var result = MessageParser.ParseMessage(pair.Item1);
                Assert.IsTrue(result.SequenceEqual(pair.Item2));
            }
        }

        [TestMethod]
        public void ErrorValues()
        {
            foreach (var elem in _errorCases)
            {
                try
                {
                    var result = MessageParser.ParseMessage(elem);
                }
                catch
                {
                    continue;
                }
                Assert.Fail();
            }
        }
    }
}