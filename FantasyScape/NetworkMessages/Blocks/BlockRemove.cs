using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class BlockRemove : Message {
		Vector3i Location;
		public BlockRemove() {
			Location = new Vector3i();
		}

		public BlockRemove(Vector3i location) {
			this.Location = location;
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Location.Write(Message);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			Location.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.World.RemoveBlock(Location);
			this.Forward();
		}
	}
}
