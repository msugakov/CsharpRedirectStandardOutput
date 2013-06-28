using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedirectStandardOutputLibrary
{
    public class AdvancedPattern
    {
        public static int ExecuteProcess(
            string fileName,
            string arguments,
            int timeout,
            out string standardOutput,
            out string standardError)
        {
            int exitCode;

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

                using (Task<bool> processWaiter = Task.Factory.StartNew(() => process.WaitForExit(timeout)))
                using (Task<string> outputReader = Task.Factory.StartNew((Func<object, string>)ReadStream, process.StandardOutput))
                using (Task<string> errorReader = Task.Factory.StartNew((Func<object, string>)ReadStream, process.StandardError))
                {
                    bool waitResult = processWaiter.Result;

                    if (!waitResult)
                    {
                        process.Kill();
                    }

                    Task.WaitAll(outputReader, errorReader);
                    // if waitResult == true hope those already finished or will finish fast
                    // otherwise wait for taks to complete to be able to dispose them

                    if (!waitResult)
                    {
                        throw new TimeoutException("Process wait timeout expired");
                    }

                    exitCode = process.ExitCode;

                    standardOutput = outputReader.Result;
                    standardError = errorReader.Result;
                }
            }

            return exitCode;
        }

        private static string ReadStream(object streamReader)
        {
            string result = ((StreamReader)streamReader).ReadToEnd();

            return result;
        } // put breakpoint on this line
    }
}
