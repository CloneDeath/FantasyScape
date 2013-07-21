using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class BlockAdd : Message {
		Vector3i Location;
		Block block;

		public BlockAdd() {
			Location = new Vector3i();
			block = new Block();
		}

		public BlockAdd(Vector3i l, Block b) {
			this.Location = l;
			this.block = b;
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Location.Write(Message);
			block.Write(Message);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			Location.Read(Message);
			block.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.Realm.SetBlock(Location, block);
			this.Forward();
		}
	}
}
