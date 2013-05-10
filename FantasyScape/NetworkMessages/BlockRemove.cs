using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class BlockRemove : Message {
		int x, y, z;
		public BlockRemove() { }

		public BlockRemove(int xpos, int ypos, int zpos) {
			x = xpos;
			y = ypos;
			z = zpos;
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Message.Write((Int32)x);
			Message.Write((Int32)y);
			Message.Write((Int32)z);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			x = Message.ReadInt32();
			y = Message.ReadInt32();
			z = Message.ReadInt32();
		}

		protected override void ExecuteMessage() {
			Game.World.RemoveBlock(x, y, z);
			this.Forward();
		}
	}
}
