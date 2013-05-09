using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class BlockLayersMessage : Message {
		private int x;
		private int y;

		public BlockLayersMessage() { }

		public BlockLayersMessage(int x, int y) {
			this.x = x;
			this.y = y;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write((Int32)x);
			Message.Write((Int32)y);
			for (int z = 0; z < Game.World.ZSize; z++) {
				Block b = Game.World.blockAt(x, y, z);
				if (b != null) {
					Message.Write(z);
					b.Write(Message);
				}
			}
		}

		protected override void ReadData(NetIncomingMessage Message) {
			this.x = Message.ReadInt32();
			this.y = Message.ReadInt32();

			while (Message.PositionInBytes < Message.LengthBytes) {
				Block b = new Block();
				int z = Message.ReadInt32();
				b.Read(Message);
				Game.World.blocks[x, y, z] = b;
			}
		}

		protected override void ExecuteMessage() {
			Game.World.LayerCount++;

			if (Game.World.LayerCount >= Game.World.XSize * Game.World.YSize) {
				Game.World.Current = World.State.Done;
				Game.World.refreshExposedBlocks();
			}
		}
	}
}
