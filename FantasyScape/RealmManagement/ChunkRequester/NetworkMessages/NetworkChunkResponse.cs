using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.NetworkMessages;
using Lidgren.Network;

namespace FantasyScape.RealmManagement.ChunkRequester.NetworkMessages {
	class NetworkChunkResponse : Message {
		NetworkChunk Chunk;

		public NetworkChunkResponse() {
			Chunk = new NetworkChunk(Game.World.Realm, new Vector3i());
		}

		public NetworkChunkResponse(NetworkChunk Chunk) {
			this.Chunk = Chunk;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Chunk.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			Chunk.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.World.Requester.QueueResponse(Chunk);
		}
	}
}
