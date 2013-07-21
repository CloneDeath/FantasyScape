using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Lidgren.Network;
using FantasyScape.NetworkMessages;
using FantasyScape.Resources;
using FantasyScape.CodeCompilation;

namespace FantasyScape.Server {
	class GameServer {
		static NetServer Server;
		static Stopwatch UpdateTimer;
		static double UpdateRate = 30.0;

		public ResourceManager Resources { get; private set; }
		
		public GameServer() {
			/* Load Resources */
			Console.WriteLine("Loading Resources...");
			Resources = new ResourceManager();
			Resources.Load();

			/* Setting Up Server */
			Console.WriteLine("Starting Up Server...");
			//Package.RecompilePackages();
			//CodeManager StartUp = Package.GetPackage(Game.ServerInfo.StartupPackage).CodeManager;
			//StartUp.RunMain();
			//Game.World.Chunks.ClearWorldGen();
			//Game.World.Chunks.AddWorldGens(StartUp.GetWorldGens());
			Game.World.ClearWorldGen();
			List<WorldGenerator> temp = new List<WorldGenerator>();
			temp.Add(new WorldGenerator());
			Game.World.AddWorldGen(temp);


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

		internal void Run() {
			/* Respond to Requests and Update World */
			while (true) {
				ConsoleHandler.Update(this);

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

		private void ReadMessages(double TimeToDelay) {
			List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
			NetIncomingMessage nim = Server.WaitMessage((int)(TimeToDelay * 1000));
			if (nim != null){
				Messages.Add(nim);
				Server.ReadMessages(Messages);
				foreach (NetIncomingMessage msg in Messages) {
					if (msg.MessageType == NetIncomingMessageType.Data) {
						Message.Handle(msg);
					} else {
						//Console.WriteLine(Message.ReadString());
					}
				}
			}
		}
	}
}
