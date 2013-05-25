using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.NetworkMessages;
using System.Threading;

namespace FantasyScape.Server {
	class GameServer {
		static NetServer Server;

		public GameServer() {
			/* Load Resources */
			ResourceManager.Load();

			/* Generate World */
			Console.WriteLine("Generating World");
			Game.GenerateWorld();


			/* Listen for Clients */
			NetPeerConfiguration Configuration = new NetPeerConfiguration("FantasyScape");
			Configuration.Port = 54987;
			Configuration.MaximumConnections = 20;
			Server = new NetServer(Configuration);
			Server.Start();
			Message.RegisterServer(Server);
			
			Console.WriteLine("Ready!");
		}

		internal static void Run() {
			/* Respond to Requests */
			while (true) {
				Game.UpdateServer();
				ReadMessages();
			}
		}

		private static void ReadMessages() {
			List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
			Server.ReadMessages(Messages);
			foreach (NetIncomingMessage Message in Messages) {
				HandleMessages(Message);
			}
		}

		private static void HandleMessages(NetIncomingMessage nim) {
			if (nim.MessageType == NetIncomingMessageType.Data) {
				Message.Handle(nim);
			} else {
				//Console.WriteLine(Message.ReadString());
			}
		}
	}
}
