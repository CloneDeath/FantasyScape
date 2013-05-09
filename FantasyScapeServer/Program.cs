using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.Server {
	class Program {
		static void Main(string[] args) {
			GameServer Server = new GameServer();
			GameServer.Run();
		}
	}
}
