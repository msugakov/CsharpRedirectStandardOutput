using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedirectStandardOutputLibrary
{
    public class FixedSimplePattern
    {
        public static void ExecuteProcess(
            string fileName,
            string arguments,
            out string standardOutput,
            out string standardError)
        {
            using (var process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;

                process.Start();

                //Thread.Sleep(100);

                using (Task processWaiter = Task.Factory.StartNew(() => process.WaitForExit()))
                using (Task<string> outputReader = Task.Factory.StartNew(() => process.StandardOutput.ReadToEnd()))
                using (Task<string> errorReader = Task.Factory.StartNew(() => process.StandardError.ReadToEnd()))
                {
                    Task.WaitAll(processWaiter, outputReader, errorReader);

                    standardOutput = outputReader.Result;
                    standardError = errorReader.Result;
                }
            }
        }
    }
}
