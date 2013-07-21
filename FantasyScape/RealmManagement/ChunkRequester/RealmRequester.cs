using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;

namespace FantasyScape.RealmManagement.ChunkRequester {
	public class RealmRequester {
		Realm Realm;
		Dictionary<Vector3i, NetworkChunk> Outgoing = new Dictionary<Vector3i, NetworkChunk>();
		Dictionary<Vector3i, NetworkChunk> Incomming = new Dictionary<Vector3i, NetworkChunk>();

		public RealmRequester(Realm Realm) {
			this.Realm = Realm;
		}

		public void QueueRequest(Vector3i Position) {
			NetworkChunk Target;
			Vector3i ChunkCoords = Position / NetworkChunk.Size;

			
			//Only make a new request if one hasn't been made, and if one isn't being resolved.
			if (!Outgoing.TryGetValue(ChunkCoords, out Target) && !Incomming.TryGetValue(ChunkCoords, out Target)) {
				Target = new NetworkChunk(Realm, ChunkCoords);
				Outgoing[ChunkCoords] = Target;
			}
		}

		public void HandleRequests() {
			NetworkChunk Target;

			for (int i = 0; i < 1 && Incomming.Count > 0; i++) {
				Vector3i Key = Incomming.Keys.First();
				NetworkChunk chunk = Incomming[Key];
				chunk.Apply(Realm);
				if (Outgoing.TryGetValue(chunk.ChunkCoords, out Target)) {
					Outgoing.Remove(chunk.ChunkCoords);
				}
				Incomming.Remove(Key);
			}

			for (int i = 0; i < 1 && Outgoing.Count > 0; i++) {
				Vector3i Key = Outgoing.Keys.First();
				NetworkChunk chunk = Outgoing[Key];
				chunk.SendRequest();
				Outgoing.Remove(Key);
			}
		}

		internal void QueueResponse(NetworkChunk Chunk) {
			Incomming[Chunk.ChunkCoords] = Chunk;
			if (Outgoing.Keys.Contains(Chunk.ChunkCoords)) {
				Outgoing.Remove(Chunk.ChunkCoords);
			}
		}

		public int OutgoingChunkCount() {
			return Outgoing.Count;
		}

		public int IncommingChunkCount() {
			return Incomming.Count;
		}
	}
}
