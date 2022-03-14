using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LH_Lib
{
	/// https://gunnarpeipman.com/dotnet-core-system-memory/
	[Serializable]
	public class MemoryMetrics
	{
		public double Total { get; set; }
		public double Free { get; set; }
		public int FreePercentage { get; set; }
		public double Used { get; set; }
		public int UsedPercentage { get; set; }
		public double Cache { get; set; }
		public int CachePercentage { get; set; }
		public int CpuUtilization { get; set; }
	}

	public static class MemoryMetricsClient
	{
		public static MemoryMetrics GetMetrics()
		{
			return !IsUnix() ? GetWindowsMetrics() : GetLinuxMetrics();
		}

		private static bool IsUnix()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
		}

		private static MemoryMetrics GetWindowsMetrics()
		{
			ProcessStartInfo info = new ProcessStartInfo();
			info.FileName = "wmic";
			info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
			info.RedirectStandardOutput = true;

			string output;
			using (Process process = Process.Start(info))
			{
				output = process.StandardOutput.ReadToEnd();
			}

			string[] lines = output.Trim().Split('\n');
			string[] freeMemoryParts = lines[0].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
			string[] totalMemoryParts = lines[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

			long total = long.Parse(totalMemoryParts[1]);
			long free = long.Parse(freeMemoryParts[1]);
			long used = total - free;

			MemoryMetrics metrics = new MemoryMetrics();
			metrics.Total = Math.Round(total / Math.Pow(1024, 2), MidpointRounding.ToEven);
			metrics.Free = Math.Round(free / Math.Pow(1024, 2), MidpointRounding.ToEven);
			metrics.FreePercentage = (int)Math.Round((100d * free) / total, MidpointRounding.ToEven);
			metrics.Used = Math.Round(used / Math.Pow(1024, 2), MidpointRounding.ToEven);
			metrics.UsedPercentage = (int)Math.Round((100d * (used)) / total, MidpointRounding.ToEven);
			metrics.Cache = 0;
			metrics.CachePercentage = 0;

			return metrics;
		}

		private static MemoryMetrics GetLinuxMetrics()
		{
			ProcessStartInfo info = new ProcessStartInfo("/bin/bash", "-c \"free -b\"");
			info.RedirectStandardOutput = true;

			string output;
			using (Process process = Process.Start(info))
			{
				output = process.StandardOutput.ReadToEnd();
			}

			string[] lines = output.Split('\n');
			string[] memory = lines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			long total = long.Parse(memory[1]);
			long free = long.Parse(memory[3]);
			long used = long.Parse(memory[2]);
			long cache = long.Parse(memory[5]);

			MemoryMetrics metrics = new MemoryMetrics();
			metrics.Total = Math.Round(total / Math.Pow(1024, 2), MidpointRounding.ToEven);
			metrics.Free = Math.Round(free / Math.Pow(1024, 2), MidpointRounding.ToEven);
			metrics.FreePercentage = (int)Math.Round((100d * free) / total, MidpointRounding.ToEven);
			metrics.Used = Math.Round(used / Math.Pow(1024, 2), MidpointRounding.ToEven);
			metrics.UsedPercentage = (int)Math.Round((100d * (used + cache)) / total, MidpointRounding.ToEven);
			metrics.Cache = Math.Round(cache / Math.Pow(1024, 2), MidpointRounding.ToEven);
			metrics.CachePercentage = (int)Math.Round((100d * cache) / total, MidpointRounding.ToEven);

			int cpuUtilization = GetLinuxCpuUtilization();
			metrics.CpuUtilization = cpuUtilization;

			return metrics;
		}

		private static int GetLinuxCpuUtilization()
		{
			string command = "awk '{u=$2+$4; t=$2+$4+$5; if (NR==1){u1=u; t1=t;} else print ($2+$4-u1) * 100 / (t-t1); }' <(grep 'cpu ' /proc/stat) <(sleep 1;grep 'cpu ' /proc/stat)";

			ProcessStartInfo info = new ProcessStartInfo("/bin/bash", $"-c \"{command}\"");
			info.RedirectStandardOutput = true;

			string output;
			using (Process process = Process.Start(info))
			{
				output = process.StandardOutput.ReadToEnd();
			}

			return (int)Math.Round(double.Parse(output));
		}
	}
}
