using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeGame;
namespace SnakeGameTest
{
    [TestClass]
    public class SnakeTest
    {
        [TestMethod]
        public void increaseSnakeLength()
        {
            Program prog = new Program();

            Assert.AreEqual(prog.ch.Count, 0);
            Assert.AreEqual(prog.score, 0);

            prog.IncreaseSnakeLength(prog, 0, 0);

            Assert.AreEqual(prog.ch.Count, 1);
            Assert.AreEqual(prog.score, 1);
        }
    }
}
