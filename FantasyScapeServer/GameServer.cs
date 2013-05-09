using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Server {
	class GameServer {
		static Game game;
		static NetServer Server;

		public GameServer() {
			//Create Game World
			game = new Game();

			/* Load Resources */
			ResourceManager.Load();

			/* Generate World */
			Console.WriteLine("Generating World");
			game.GenerateWorld();


			/* Listen for Clients */
			NetPeerConfiguration Configuration = new NetPeerConfiguration("FantasyScape");
			Configuration.Port = 54987;
			Configuration.MaximumConnections = 20;
			Server = new NetServer(Configuration);
			Server.Start();
			Console.WriteLine("Ready!");
		}

		internal static void Run() {
			/* Respond to Requests */
			while (true) {
				NetIncomingMessage Message = Server.WaitMessage(1000);
				if (Message != null) {
					HandleMessages(Message);
				}
			}
		}

		private static void HandleMessages(NetIncomingMessage nim) {
			if (nim.MessageType == NetIncomingMessageType.Data) {
				Message.Handle(nim);

					//if (msg.RequestType == "Textures") {
					//    Console.WriteLine("Sending Response for 'Request Textures'");
					//    Textures.SendTextures(nim.SenderConnection, Server);
					//} else if (msg.RequestType == "WorldSize") {
					//    Console.WriteLine("Sending Response for 'Request World Size'");
					//    game.world.SendWorldSize(nim.SenderConnection, Server);
					//} else if (msg.RequestType == "BlockLayers") {
					//    Console.WriteLine("Sending Response for 'Request Block Layers'");
					//    game.world.SendBlockLayers(nim.SenderConnection, Server);
					//} else if (msg.RequestType == "BlockTypes") {
					//    Console.WriteLine("Sending Response for 'Request BlockTypes'");
					//    BlockTypes.SendBlockTypes(nim.SenderConnection, Server);
					//}
			} else {
				//Console.WriteLine(Message.ReadString());
			}
		}
	}
}
