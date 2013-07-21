using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

			foreach (NetworkChunk chunk in Incomming.Values) {
				chunk.Apply(Realm);	
				if (Outgoing.TryGetValue(chunk.ChunkCoords, out Target)){
					Outgoing.Remove(chunk.ChunkCoords);
				}
			}

			foreach (NetworkChunk chunk in Outgoing.Values) {
				chunk.SendRequest();
			}
		}
	}
}
