using Microsoft.VisualStudio.TestTools.UnitTesting;
using Math.Lib;
using System; // Required for System.Math.Max

namespace Math.Tests
{
    [TestClass]
    public class RooterTests
    {
        [TestMethod]
        public void BasicRooterTest()
        {
            Rooter rooter = new Rooter();
            double expectedResult = 2.0;
            double input = expectedResult * expectedResult;
            double actualResult = rooter.SquareRoot(input);
            Assert.AreEqual(expectedResult, actualResult, delta: expectedResult / 100);
        }

        [TestMethod]
        public void RooterValueRange()
        {
            Rooter rooter = new Rooter();
            // Test a wide range of positive values, from very small to very large
            for (double expected = 1e-8; expected < 1e+8; expected *= 3.2)
            {
                RooterOneValue(rooter, expected);
            }
        }

        private void RooterOneValue(Rooter rooter, double expectedResult)
        {
            double input = expectedResult * expectedResult;
            double actualResult = rooter.SquareRoot(input);

            // Calculate a tolerance (delta) for floating-point comparison.
            // For very small numbers, a relative tolerance (expectedResult / 1000) might be too strict.
            // We use a combination: a relative tolerance OR a small absolute tolerance,
            // whichever is larger. This makes the test more robust for numbers close to zero.
            double relativeTolerance = expectedResult / 1000.0;
            double absoluteTolerance = 1e-10; // A small constant for numbers near zero

            // Use System.Math.Max to ensure delta is never too small, especially for expectedResult approaching 0.
            Assert.AreEqual(expectedResult, actualResult, delta: System.Math.Max(relativeTolerance, absoluteTolerance),
                            $"{nameof(RooterOneValue)} failed for expectedResult: {expectedResult}"); // Added message for clarity
        }

        [TestMethod]
        public void RooterTestNegativeInputWithMessage()
        {
            Rooter rooter = new Rooter();

            // Use Assert.ThrowsException to verify that the ArgumentOutOfRangeException is thrown
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => rooter.SquareRoot(-10));

            // Verify that the exception message contains the expected text.
            // Also, good practice to check the parameter name if your exception specifies it.
            Assert.IsTrue(exception.Message.Contains("El valor ingresado es invalido, solo se puede ingresar números positivos"),
                          "Exception message does not contain the expected text.");
            // If Rooter.SquareRoot uses the constructor ArgumentOutOfRangeException(paramName, message),
            // you might also want to assert: Assert.AreEqual("input", exception.ParamName);
        }

        [TestMethod]
        public void RooterTestNegativeInputx()
        {
            Rooter rooter = new Rooter();
            try
            {
                rooter.SquareRoot(-10);
                // If we reach here, no exception was thrown, so the test should fail.
                Assert.Fail("No exception was thrown for negative input.");
            }
            catch (System.ArgumentOutOfRangeException)
            {
                // If ArgumentOutOfRangeException is caught, the test passes.
                return;
            }
            // No Assert.Fail() needed here, as it's either returned or failed in the try block.
        }

        // --- NEW TEST ADDED TO REACH 5 TOTAL TESTS ---
        [TestMethod]
        public void RooterTestZeroInput()
        {
            Rooter rooter = new Rooter();
            double expectedResult = 0.0;
            double actualResult = rooter.SquareRoot(0.0);
            // For zero, the relative tolerance doesn't make sense, so use a small absolute delta.
            Assert.AreEqual(expectedResult, actualResult, delta: 1e-15, "Square root of zero should be zero.");
        }
    }
}