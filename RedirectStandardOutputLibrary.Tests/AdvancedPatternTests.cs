using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace RedirectStandardOutputLibrary.Tests
{
    public class AdvancedPatternTests
    {
        private const int DefaultTimeout = 1000;

        private const int Runs = 1000;

        [Fact]
        public void TestItIsOk()
        {
            for (int i = 0; i < Runs; ++i)
            {
                string testString = Guid.NewGuid().ToString();

                string standardOutput, standardError;

                int returnCode = AdvancedPattern.ExecuteProcess(
                    "cmd.exe",
                    string.Format("/c echo {0}", testString),
                    DefaultTimeout,
                    out standardOutput,
                    out standardError);

                Assert.Equal(0, returnCode);

                Assert.Equal(testString, standardOutput.Trim()); // Should not fail anymore. Also try uncomment Thread.Sleep in the code

                Assert.Equal(string.Empty, standardError);
            }
        }

        [Fact]
        public void TestTimeout()
        {
            string standardOutput, standardError;

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            Assert.Throws<TimeoutException>(() =>
                AdvancedPattern.ExecuteProcess(
                    "LongWaiter.exe",
                    string.Format("{0}", DefaultTimeout * 10),
                    DefaultTimeout,
                    out standardOutput,
                    out standardError));

            stopwatch.Stop();

            Assert.True(stopwatch.Elapsed.TotalSeconds > 1e-3 * DefaultTimeout);

            Assert.True(stopwatch.Elapsed.TotalSeconds < 1.2 * 1e-3 * DefaultTimeout);
        }
    }
}
