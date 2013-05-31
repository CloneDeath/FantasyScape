using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Lidgren.Network;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Server {
	class GameServer {
		static NetServer Server;
		static Stopwatch UpdateTimer;
		static double UpdateRate = 30.0;

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

			UpdateTimer = new Stopwatch();
			UpdateTimer.Start();
			
			Console.WriteLine("Ready!");
		}

		internal static void Run() {
			/* Respond to Requests and Update World */
			while (true) {
				if (UpdateTimer.Elapsed.TotalSeconds >= 1 / UpdateRate) {
					UpdateTimer.Restart();
					Game.UpdateServer();
				}

				//The remaining time in seconds until the next update
				double TimeRemaining = (1 / UpdateRate) - UpdateTimer.Elapsed.TotalSeconds;
				if (TimeRemaining <= 0) {
					TimeRemaining = 0;
				}
				ReadMessages(TimeRemaining);
			}
		}

		private static void ReadMessages(double TimeToDelay) {
			List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
			NetIncomingMessage nim = Server.WaitMessage((int)(TimeToDelay * 1000));
			if (nim != null){
				Messages.Add(nim);
				Server.ReadMessages(Messages);
				foreach (NetIncomingMessage msg in Messages) {
					HandleMessages(msg);
				}
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
