using LH_Lib;

using Newtonsoft.Json;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinuxManager
{
	public partial class Main : Form
	{
		public Main()
		{
			this.InitializeComponent();
		}

		private void button_connect_Click(object sender, EventArgs e)
		{
			IPEndPoint serverEp = new IPEndPoint(IPAddress.Parse("75.119.142.199"), 6969);
			//IPEndPoint serverEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6969);
			TcpClient client = new TcpClient();

			client.Connect(serverEp);
			if (client.Connected)
			{
				button_connect.Enabled = false;
				button_connect.Text = "Connected";
			}

			Task.Run(() =>
			{
				while (client.Connected)
				{
					NetworkStream ns = client.GetStream();
					if (client.ReceiveBufferSize > 0)
					{
						byte[] bytes = new byte[client.ReceiveBufferSize];
						int byteRead = ns.Read(bytes, 0, client.ReceiveBufferSize);

						if (byteRead > 0)
						{
							MemoryMetrics metrics = DeserializeObject(bytes);
							this.Log("Free: " + metrics.FreePercentage + "% / CPU: " + metrics.CpuUtilization + "%");
						}
						else
						{
							this.Log("Connection closed.");
						}
						client.GetStream().Write(new byte[] { 1 }, 0, 1);
					}
				}
				this.Log("Connection closed.");
			});
		}

		private static MemoryMetrics DeserializeObject(byte[] obj)
		{
			return JsonConvert.DeserializeObject<MemoryMetrics>(Encoding.UTF8.GetString(obj));
		}

		private void Log(string text)
		{
			listBox_output.Items.Add(text);
			listBox_output.SelectedIndex = listBox_output.Items.Count - 1;
		}
	}
}