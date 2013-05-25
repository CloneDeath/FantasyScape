using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class BlockAdd : Message {
		int x, y, z;
		Block block;

		public BlockAdd() { }

		public BlockAdd(int xpos, int ypos, int zpos, Block b) {
			x = xpos;
			y = ypos;
			z = zpos;
			this.block = b;
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Message.Write((Int32)x);
			Message.Write((Int32)y);
			Message.Write((Int32)z);
			block.Write(Message);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			x = Message.ReadInt32();
			y = Message.ReadInt32();
			z = Message.ReadInt32();
			block = new Block();
			block.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.World.SetBlock(x, y, z, block);
			this.Forward();
		}
	}
}
