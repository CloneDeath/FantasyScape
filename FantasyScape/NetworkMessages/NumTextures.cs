using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class NumTextures : Message {
		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write((Int32)Textures.Count);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			Textures.Count = Message.ReadInt32();
		}

		protected override void ExecuteMessage() {}
	}
}
