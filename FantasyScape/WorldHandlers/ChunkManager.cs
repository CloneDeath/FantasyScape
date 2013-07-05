﻿using System;
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

		List<Vector3i> RefreshQueue = new List<Vector3i>();

		public Chunk this[Vector3i Location] {
			get {
				Chunk c;
				Chunks.TryGetValue(Location, out c);
				if (c == null) {
					if (Game.Host == HostType.Server) {
						c = WorldGen.GenerateTerrain(Location);
						Game.World.Chunks[Location] = c;
					} else {
						if (!PendingRequests.Contains(Location) && PendingRequests.Count <= 1) {
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

		internal bool TryGet(Vector3i ChunkLoc, out Chunk chunk) {
			return Chunks.TryGetValue(ChunkLoc, out chunk);
		}

		public void AddRefreshQueue(Vector3i Location) {
			if (!RefreshQueue.Contains(Location)) {
				RefreshQueue.Add(Location);
			}
		}

		internal void Draw(World world) {
			for (int i = 0; i < 10; i++) {
				if (RefreshQueue.Count != 0) {
					Vector3i Loc = RefreshQueue[0];
					RefreshQueue.RemoveAt(0);

					Chunk chunk;
					if (TryGet(Loc, out chunk)) {
						chunk.RefreshExposedBlocks(world);
					}
				}
			}
		}

		public void ClearWorldGen() {
			WorldGen.Clear();
		}

		public void AddWorldGens(List<WorldGenerator> list) {
			WorldGen.Add(list);
		}
	}
}
