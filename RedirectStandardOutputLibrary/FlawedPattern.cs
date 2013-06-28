using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace RedirectStandardOutputLibrary
{
    public class FlawedPattern
    {
        public static int ExecuteProcess(
            string fileName,
            string arguments,
            int timeout,
            out string standardOutput,
            out string standardError)
        {
            int exitCode;

            var standardOutputBuilder = new StringBuilder();
            var standardErrorBuilder = new StringBuilder();

            using (var process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;

                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        standardOutputBuilder.AppendLine(args.Data);
                    }
                };
                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        standardErrorBuilder.AppendLine(args.Data);
                    }
                };

                process.Start();

                // Thread.Sleep(100); uncomment this line

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (process.WaitForExit(timeout))
                {
                    exitCode = process.ExitCode;
                }
                else
                {
                    process.Kill();

                    throw new TimeoutException("Process wait timeout expired");
                }
            }

            standardOutput = standardOutputBuilder.ToString();
            standardError = standardErrorBuilder.ToString();

            return exitCode;
        }
    }
}
