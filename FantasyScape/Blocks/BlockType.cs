using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;

namespace FantasyScape {
	public class BlockType {
		public string Name;

		public string Texture {
			set {
				TopTexture = value;
				SideTexture = value;
				BotTexture = value;
			}
		}

		public string TopTexture;
		public string SideTexture;
		public string BotTexture;
		public bool Liquid;


		internal void Send(NetConnection netConnection, NetServer Server) {
			NetOutgoingMessage nom = Server.CreateMessage();
			nom.Write("BlockType");
			nom.Write(Name);
			nom.Write(TopTexture);
			nom.Write(SideTexture);
			nom.Write(BotTexture);
			nom.Write(Liquid);
			Server.SendMessage(nom, netConnection, NetDeliveryMethod.ReliableUnordered);
		}

		internal static void Receive(NetIncomingMessage Message) {
			BlockType b = new BlockType();
			b.Name = Message.ReadString();
			b.TopTexture = Message.ReadString();
			b.SideTexture = Message.ReadString();
			b.BotTexture = Message.ReadString();
			b.Liquid = Message.ReadBoolean();
			BlockTypes.AddBlockType(b);
		}
	}
}
