using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using FantasyScape.NetworkMessages.Chunks;

namespace FantasyScape {
	public class ChunkManager : IEnumerable {
		private MapGenerator WorldGen = new MapGenerator();
		public Dictionary<Vector3i, Chunk> Chunks = new Dictionary<Vector3i, Chunk>();
		public List<Vector3i> PendingRequests = new List<Vector3i>();

		public Chunk this[Vector3i Location] {
			get {
				Chunk c;
				Chunks.TryGetValue(Location, out c);
				if (c == null) {
					if (Game.Host == HostType.Server) {
						c = WorldGen.GenerateTerrain(Location);
						Game.World.Chunks[Location] = c;
					} else {
						if (!PendingRequests.Contains(Location) /*&& PendingRequests.Count <= 1*/) {
							new RequestChunk(Location).Send();
							PendingRequests.Add(Location);
						}
						c = Chunk.Null;
					}
				}
					
				return c;
			}
			set {
				if (PendingRequests.Contains(Location)) {
					PendingRequests.Remove(Location);
				}
				Chunks[Location] = value;
			}
		}

		public IEnumerator GetEnumerator() {
			return Chunks.Values.GetEnumerator();
		}

		internal void TryGet(Vector3i ChunkLoc, out Chunk chunk) {
			Chunks.TryGetValue(ChunkLoc, out chunk);
		}
	}
}
