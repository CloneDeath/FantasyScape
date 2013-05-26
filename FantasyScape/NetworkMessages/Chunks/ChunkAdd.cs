using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class ChunkAdd : Message {
		private int x;
		private int y;
		private int z;
		private Chunk chunk;

		public ChunkAdd() { }

		public ChunkAdd(int x, int y, int z, Chunk chunk) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.chunk = chunk;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write((Int32)x);
			Message.Write((Int32)y);
			Message.Write((Int32)z);
			chunk.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			this.x = Message.ReadInt32();
			this.y = Message.ReadInt32();
			this.z = Message.ReadInt32();

			chunk = new Chunk();
			chunk.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.World.Chunks[x, y, z] = chunk;
			Game.World.ChunkCount++;
		}
	}
}
