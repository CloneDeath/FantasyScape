using System;

namespace FantasyScape.Server {
	class Program {
		static void Main(string[] args) {
			GameServer Server = new GameServer();
			Server.Run();
		}
	}
}
