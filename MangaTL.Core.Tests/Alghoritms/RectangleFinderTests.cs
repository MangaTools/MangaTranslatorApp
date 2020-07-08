using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MangaTL.Core.Algorithms.Tests
{
    [TestClass]
    public class RectangleFinderTests
    {
        [TestMethod]
        public void FindRectangleTestSimple()
        {
            var input = new[,]
            {
                {true, false},
                {false, false}
            };
            var expected = new Rectangle(0, 0, 1, 1);

            GeneralChecker(input, expected);
        }

        [TestMethod]
        public void FindRectangleTestHarder()
        {
            var input = new[,]
            {
                {true, true, false},
                {true, true, false},
                {false, false, false}
            };
            var expected = new Rectangle(0, 0, 2, 2);

            GeneralChecker(input, expected);
        }

        [TestMethod]
        public void FindRectangleTestMultiple()
        {
            var input = new[,]
            {
                {true, false, true},
                {true, false, true},
                {false, false, true}
            };
            var expected = new Rectangle(2, 0, 1, 3);

            GeneralChecker(input, expected);
        }

        [TestMethod]
        public void FindRectangleTestEmpty()
        {
            var input = new[,]
            {
                {false, false, false},
                {false, false, false},
                {false, false, false}
            };
            var expected = new Rectangle(0, 0, 0, 0);

            GeneralChecker(input, expected);
        }

        [TestMethod]
        public void FindRectangleTestConnected()
        {
            var input = new[,]
            {
                {true, true, false},
                {true, true, false},
                {false, true, false},
                {false, true, false}
            };
            var expected = new Rectangle(0, 0, 2, 2);

            GeneralChecker(input, expected);
        }

        private static void GeneralChecker(bool[,] input, Rectangle expected)
        {
            var actual = RectangleFinder.FindRectangle(input);
            Assert.AreEqual(expected, actual);
        }
    }
}