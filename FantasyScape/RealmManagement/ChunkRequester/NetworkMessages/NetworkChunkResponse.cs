using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.NetworkMessages;

namespace FantasyScape.RealmManagement.ChunkRequester.NetworkMessages {
	class NetworkChunkResponse : Message {
		public NetworkChunkResponse() {
			throw new NotImplementedException();
		}

		public NetworkChunkResponse(NetworkChunk Chunk) {
			throw new NotImplementedException();
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			throw new NotImplementedException();
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			throw new NotImplementedException();
		}

		protected override void ExecuteMessage() {
			throw new NotImplementedException();
		}
	}
}
