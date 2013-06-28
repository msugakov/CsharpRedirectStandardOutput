using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RedirectStandardOutputLibrary.Tests
{
    public class FlawedPatternTests
    {
        private const int DefaultTimeout = 1000;

        private const int Runs = 1000;

        [Fact]
        public void TestItBreaks_ShouldFail()
        {
            for (int i = 0; i < Runs; ++i)
            {
                string testString = Guid.NewGuid().ToString();

                string standardOutput, standardError;

                int returnCode = FlawedPattern.ExecuteProcess(
                    "cmd.exe",
                    string.Format("/c echo {0}", testString),
                    DefaultTimeout,
                    out standardOutput,
                    out standardError);

                Assert.Equal(0, returnCode);

                Assert.Equal(testString, standardOutput.Trim()); // should fail here

                Assert.Equal(string.Empty, standardError);
            }
        }

        [Fact]
        public void TestNoEolWriter()
        {
            string testString = Guid.NewGuid().ToString();

            string standardOutput, standardError;

            int returnCode = FlawedPattern.ExecuteProcess(
                "NoEolWriter.exe",
                testString,
                DefaultTimeout,
                out standardOutput,
                out standardError);

            Assert.Equal(0, returnCode);

            Assert.Equal(testString, standardOutput.Trim());

            Assert.Equal(string.Empty, standardError);
        }
    }
}
