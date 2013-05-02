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
			game = new Game();

			NetPeerConfiguration Configuration = new NetPeerConfiguration("FantasyScape");
			Configuration.Port = 54987;
			Configuration.MaximumConnections = 20;
			Server = new NetServer(Configuration);
			Server.Start();

			while (true) {
				Server.WaitMessage(1000);
				List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
				if (Server.ReadMessages(Messages) > 0) {
					HandleMessages(Messages);
				}
			}
		}

		private static void HandleMessages(List<NetIncomingMessage> Messages) {
			foreach (NetIncomingMessage Message in Messages) {
				if (Message.MessageType == NetIncomingMessageType.Data) {
					string RequestType = Message.ReadString();
					if (RequestType == "Textures") {
						Textures.SendTextures(Message.SenderConnection, Server);
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
