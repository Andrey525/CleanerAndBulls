namespace BullsAndCowsTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("TestMethod1");
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine("TestMethod2");
            Assert.AreEqual(2, 2);
        }
    }
}