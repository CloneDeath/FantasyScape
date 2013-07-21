using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.NetworkMessages;
using Lidgren.Network;

namespace FantasyScape.RealmManagement.ChunkRequester.NetworkMessages {
	class NetworkChunkRequest : Message{
		Vector3i Location;

		public NetworkChunkRequest() {
			Location = new Vector3i();
		}
		
		public NetworkChunkRequest(NetworkChunk Chunk) {
			Location = Chunk.ChunkCoords;
		}
		
		protected override void WriteData(NetOutgoingMessage Message) {
			Location.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			Location.Read(Message);
		}

		protected override void ExecuteMessage() {
			new NetworkChunkResponse(new NetworkChunk(Game.World.Realm, Location)).Reply();
		}
	}
}
