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

		bool once = false;
		public Chunk this[Vector3i Location] {
			get {
				Chunk c;
				Chunks.TryGetValue(Location, out c);
				if (c == null) {
					if (Game.Host == HostType.Server) {
						c = WorldGen.GenerateTerrain(Location);
						Game.World.Chunks[Location] = c;
					} else {
						if (!once) {
							once = true;
							for (int x = -1; x <= 1; x++) {
								for (int y = -1; y <= 1; y++) {
									for (int z = -1; z <= 1; z++) {
										new RequestChunk(new Vector3i(x, y, z)).Send();
									}
								}
							}
						}
						//if (!PendingRequests.Contains(Location)) {
						//    new RequestChunk(Location).Send();
						//    PendingRequests.Add(Location);
						//}
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
