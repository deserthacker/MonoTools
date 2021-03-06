﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MonoTools.Library;

namespace MonoTools.Debugger {

	public class MonoServerDiscovery {

		public int DiscoveryPort;


		public MonoServerDiscovery(int discoveryPort = MonoDebugServer.DefaultDiscoveryPort) {
			DiscoveryPort = discoveryPort;
		}

		public async Task<MonoServerInformation> SearchServer(CancellationToken token) {
			using (var udp = new UdpClient(new IPEndPoint(IPAddress.Any, DiscoveryPort))) {
				Task result = await Task.WhenAny(udp.ReceiveAsync(), Task.Delay(500, token));
				var task = result as Task<UdpReceiveResult>;
				if (task != null) {
					UdpReceiveResult udpResult = task.Result;
					string msg = Encoding.Default.GetString(udpResult.Buffer);
					var ip = udpResult.RemoteEndPoint.Address;
					var localips = Dns.GetHostAddresses("localhost").Concat(Dns.GetHostAddresses(Environment.MachineName));
					if (localips.Any(adr => ip == adr)) ip = IPAddress.Parse("127.0.0.1");
					return new MonoServerInformation { Message = msg, IpAddress = ip };
				}
			}

			return null;
		}

		public async Task<MonoServerInformation> SearchServer(string ipOrHost, CancellationToken token) {
			IPAddress ip;
			if (!IPAddress.TryParse(ipOrHost, out ip)) {
				IPAddress[] adresses = Dns.GetHostEntry(ipOrHost).AddressList;
				ip = adresses.FirstOrDefault(adr => adr.AddressFamily == AddressFamily.InterNetwork);
			}
			using (var udp = new UdpClient(new IPEndPoint(ip, 15000))) {
				Task result = await Task.WhenAny(udp.ReceiveAsync(), Task.Delay(500, token));
				var task = result as Task<UdpReceiveResult>;
				if (task != null) {
					UdpReceiveResult udpResult = task.Result;
					string msg = Encoding.Default.GetString(udpResult.Buffer);
					return new MonoServerInformation { Message = msg, IpAddress = udpResult.RemoteEndPoint.Address };
				}
			}

			return null;
		}
	}
}