using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerPlanSystray
{
    public class PowerPlan
    {
        public string Guid { get; private set; }
        public string Name { get; private set; }

        public PowerPlan(string guid, string name)
        {
            Guid = guid;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else if (obj == this)
            {
                return true;
            }

            PowerPlan other = (PowerPlan)obj;
            return Guid == other.Guid && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode() * 17 + Name.GetHashCode();
        }
    }

    public class PowerPlans
    {
        public static readonly Regex POWER_SCHEME_REGEX = new Regex(@"GUID: ([A-Fa-f0-9-]+) +\(([^)]+)\)( +\*)?");
        public static readonly string POWERCFG = GetPowercfgPath();

        private static PowerPlans instance;

        public List<PowerPlan> Plans { get; private set; }
        public PowerPlan ActivePlan { get; private set; }
        public event EventHandler Reloaded;

        private PowerPlans()
        {
            Reload();
        }

        public static PowerPlans Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PowerPlans();
                }
                return instance;
            }
        }

        public void Reload()
        {
            var output = PowercfgList();
            ParsePowercfgList(output);
            if (Reloaded != null)
            {
                Reloaded(this, new EventArgs());
            }
        }

        public void SetActivePlan(PowerPlan plan)
        {
            if (plan == null) throw new ArgumentNullException("plan");
            if (!Plans.Contains(plan))
            {
                throw new ArgumentException("Given plan is not in the list of plans", "plan");
            }

            Console.WriteLine("Setting active plan to " + plan.Name);
            PowercfgSetActive(plan.Guid);
            Reload();
        }

        public static string PowercfgList()
        {
            return RunProcess(POWERCFG, "/LIST");
        }

        public static string PowercfgSetActive(string guid)
        {
            return RunProcess(POWERCFG, "/SETACTIVE " + guid);
        }

        static string RunProcess(string executable, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = executable,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            using (Process proc = new Process())
            {
                proc.StartInfo = startInfo;
                proc.Start();
                string stdout = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                if (proc.ExitCode != 0)
                {
                    throw new InvalidOperationException(executable + " " + arguments + " returned non-zero exit code " + proc.ExitCode);
                }
                return stdout;
            }
        }

        private void ParsePowercfgList(string output)
        {
            var matches = output.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(scheme => POWER_SCHEME_REGEX.Match(scheme))
                .Where(match => match.Success)
                .ToList();

            if (matches.Count == 0)
            {
                throw new InvalidOperationException("Could not parse the output from powercfg.");
            }

            Plans = new List<PowerPlan>();
            ActivePlan = null;

            foreach (var match in matches)
            {
                var powerPlan = new PowerPlan(match.Groups[1].Value, match.Groups[2].Value);
                Plans.Add(powerPlan);
                if (match.Groups[3].Success)
                {
                    ActivePlan = powerPlan;
                }
            }

            if (ActivePlan == null)
            {
                throw new InvalidOperationException("Unable to determine the active power plan");
            }
        }

        static string GetPowercfgPath()
        {
            string powercfg = GetFullPath("powercfg.exe");
            if (powercfg == null)
            {
                return powercfg;
            }
            else
            {
                Console.WriteLine("powercfg.exe not found on the PATH - trying default location");
                powercfg = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\System32\powercfg.exe");
                if (powercfg != null && File.Exists(powercfg))
                {
                    return powercfg;
                }
                else
                {
                    throw new FileNotFoundException("Could not find powercfg on the PATH or in the System32 directory.", "powercfg.exe");
                }
            }
        }

        static string GetFullPath(string fileName)
        {
            // http://stackoverflow.com/a/3856090/47493
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

    }
}
