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

        [TestMethod]
        public void TestScoreResultCheck()
        {

            Program pro = new Program();
            string result = pro.ScoreCheck(0);
            string ExpectedLoss = "Game Over, You Lost. :(";
            string ExpectedWin = "Congratulations!!! You Won!!!.";

            Assert.AreEqual(result, ExpectedLoss);

            string altResult = pro.ScoreCheck(2);

            Assert.AreEqual(altResult, ExpectedWin);


        }
    }
}

