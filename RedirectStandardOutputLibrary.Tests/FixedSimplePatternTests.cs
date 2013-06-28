using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RedirectStandardOutputLibrary.Tests
{
    public class FixedSimplePatternTests
    {
        private const int Runs = 1000;

        [Fact]
        public void TestItIsOk()
        {
            for (int i = 0; i < Runs; ++i)
            {
                string testString = Guid.NewGuid().ToString();

                string standardOutput, standardError;

                FixedSimplePattern.ExecuteProcess(
                    "cmd.exe",
                    string.Format("/c echo {0}", testString),
                    out standardOutput,
                    out standardError);

                Assert.Equal(testString, standardOutput.Trim()); // Should not fail anymore. Also try uncomment Thread.Sleep in the code

                Assert.Equal(string.Empty, standardError);
            }
        }
    }
}
