using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDotNet.DynamicInterop
{
    public static class PlatformUtility
    {

        public static PlatformID GetPlatform()
        {
            return Environment.OSVersion.Platform;
        }

        public static string ExecCommand(string processName, string arguments)
        {
            using (var proc = new Process())
            {
                proc.StartInfo.FileName = processName;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                var kernelName = proc.StandardOutput.ReadLine();
                proc.WaitForExit();
                return kernelName;
            }
        }
    }
}
