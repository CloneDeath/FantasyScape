using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class BlockAdd : Message {
		int x, y, z;
		string BlockType;

		public BlockAdd() { }

		public BlockAdd(int xpos, int ypos, int zpos, string BlockType) {
			x = xpos;
			y = ypos;
			z = zpos;
			this.BlockType = BlockType;
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Message.Write((Int32)x);
			Message.Write((Int32)y);
			Message.Write((Int32)z);
			Message.Write(BlockType);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			x = Message.ReadInt32();
			y = Message.ReadInt32();
			z = Message.ReadInt32();
			BlockType = Message.ReadString();
		}

		protected override void ExecuteMessage() {
			Game.World.addBlock(x, y, z, BlockType);
			this.Forward();
		}
	}
}
