using BullsAndCows.Model;
using BullsAndCows.Model.Helpers;

namespace BullsAndCowsTests
{
    [TestClass]
    public class HornedsCounterTest
    {
        List<Tuple<Horneds, int[], int[]>> _validCases = new List<Tuple<Horneds, int[], int[]>>
        {
            Tuple.Create(new Horneds { BullsCount = 4, CowsCount = 0 }, new int[]{1, 2, 3, 4}, new int[]{1, 2, 3, 4}),
            Tuple.Create(new Horneds { BullsCount = 0, CowsCount = 4 }, new int[]{1, 2, 3, 4}, new int[]{2, 1, 4, 3}),
            Tuple.Create(new Horneds { BullsCount = 0, CowsCount = 0 }, new int[]{1, 2, 3, 4}, new int[]{0, 5, 6, 7}),
            Tuple.Create(new Horneds { BullsCount = 2, CowsCount = 2 }, new int[]{1, 2, 3, 4}, new int[]{1, 2, 4, 3})
        };

        [TestMethod]
        public void ValidValues()
        {
            foreach (var tuple in _validCases)
            {
                var result = HornedsCounter.CountHorneds(tuple.Item2, tuple.Item3);
                Assert.IsTrue(result.CowsCount == tuple.Item1.CowsCount &&
                              result.BullsCount == tuple.Item1.BullsCount);
            }
        }
    }
}