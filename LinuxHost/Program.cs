using LH_Lib;

using Newtonsoft.Json;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LinuxHost
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			MemoryMetrics metrics = MemoryMetricsClient.GetMetrics();
			//Console.WriteLine($"\nFree Memory: {metrics.FreePercentage}% ({metrics.Free} MB)" +
			//	$"\nUsed Memory: {metrics.UsedPercentage}% ({metrics.Used} MB)");

			TcpServer().GetAwaiter().GetResult();
		}

		private static async Task TcpServer()
		{
			TcpListener server = new TcpListener(IPAddress.Any, 6969);
			Console.WriteLine("Starting server...");
			server.Start();
			while (true)
			{
				Console.WriteLine("Waiting for connection...");
				TcpClient client = await server.AcceptTcpClientAsync();
				Console.WriteLine("Client connected: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

				await Task.Run(() =>
				{
					while (client.Connected)
					{
						MemoryMetrics metrics = MemoryMetricsClient.GetMetrics();
						byte[] metricsBytes = SerializeObject(metrics);
						client.GetStream().Write(metricsBytes, 0, metricsBytes.Length);
						Console.WriteLine("Free: " + metrics.FreePercentage + "%");

						byte[] bytes = new byte[client.ReceiveBufferSize];
						int byteRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
						Console.WriteLine("Bytes read: " + byteRead);
					}
					Console.WriteLine("Client disconnected.");
				});
			}
		}

		private static byte[] SerializeObject(MemoryMetrics metrics)
		{
			return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metrics));
		}
	}
}
