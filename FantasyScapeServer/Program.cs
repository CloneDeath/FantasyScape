using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.Server {
	class Program {
		static Game game;
		static NetServer Server;
		static void Main(string[] args) {
			//Create Game World
			game = new Game(GameMode.Server);


			/* Mount Resources */
			Console.WriteLine("Mounting Textures");
			game.MountTextures();

			Console.WriteLine("Mounting Block Types");
			BlockTypes.LoadBlockTypes();

			
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

			/* Respond to Requests */
			while (true) {
				NetIncomingMessage Message = Server.WaitMessage(1000);
				if (Message != null) {
					HandleMessages(Message);
				}
			}
		}

		private static void HandleMessages(NetIncomingMessage Message) {
			if (Message.MessageType == NetIncomingMessageType.Data) {
				string Type = Message.ReadString();
				if (Type == "Request") {
					string RequestType = Message.ReadString();
					if (RequestType == "Textures") {
						Console.WriteLine("Sending Response for 'Request Textures'");
						Textures.SendTextures(Message.SenderConnection, Server);
					} else if (RequestType == "WorldSize") {
						Console.WriteLine("Sending Response for 'Request World Size'");
						game.world.SendWorldSize(Message.SenderConnection, Server);
					} else if (RequestType == "BlockLayers"){
						Console.WriteLine("Sending Response for 'Request Block Layers'");
						game.world.SendBlockLayers(Message.SenderConnection, Server);
					} else if (RequestType == "BlockTypes") {
						Console.WriteLine("Sending Response for 'Request BlockTypes'");
						BlockTypes.SendBlockTypes(Message.SenderConnection, Server);
					}
				}
			}
		}

		//private static object GetInstance(string ObjectName) {
		//    Type t = Type.GetType(ObjectName);
		//    //if (t.GetInterfaces().Contains(typeof(Program))) {
		//        return t.GetConstructor(new Type[0]).Invoke(new object[0]);
		//    //} else {
		//    //	return null;
		//    //}
		//}
	}
}
