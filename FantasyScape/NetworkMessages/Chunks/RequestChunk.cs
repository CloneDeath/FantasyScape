using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages.Chunks {
	class RequestChunk : Message {
		public RequestChunk() {
			Location = new Vector3i();
		}

		Vector3i Location;
		public RequestChunk(Vector3i loc) {
			this.Location = loc;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Location.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			Location.Read(Message);
		}

		protected override void ExecuteMessage() {
			if (!Game.World.ChunkLoaded(Location)) {
				Game.GenerateChunk(Location);
			}
			new AddChunk(Location, Game.World.Chunks[Location]).Reply();
		}
	}
}
