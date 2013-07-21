using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.Resources;

namespace FantasyScape.RealmManagement {
	public class Realm : Resource {
		Dictionary<Vector3i, MemoryChunk> Blocks = new Dictionary<Vector3i, MemoryChunk>();
		
		public Action<Vector3i> OnBlockChanged;

		private MemoryChunk Cache = null;

		public Block GetBlock(Vector3i Location) {
			MemoryCoordinate coords = new MemoryCoordinate(Location);
			MemoryChunk Return;

			if (Cache != null && Cache.Location == coords.ChunkCoord) {
				return Cache[coords.LocalCoords];
			}
			
			if (Blocks.TryGetValue(coords.ChunkCoord, out Return)) {
				Cache = Return;
				return Return[coords.LocalCoords];
			} else {
				return null;
			}
		}

		public void SetBlock(Vector3i Location, Block Data) {
			MemoryCoordinate coords = new MemoryCoordinate(Location);
			MemoryChunk Return;

			if (Cache != null && Cache.Location == coords.ChunkCoord) {
				Cache[coords.LocalCoords] = Data;
				return;
			}

			if (Blocks.TryGetValue(coords.ChunkCoord, out Return)) {
				Return[coords.LocalCoords] = Data;
			} else {
				Return = new MemoryChunk(coords.ChunkCoord);
				Blocks[coords.ChunkCoord] = Return;
				Return[coords.LocalCoords] = Data;
			}
			Cache = Return;

			if (OnBlockChanged != null) {
				OnBlockChanged(Location);
			}
		}

		public override void Save(string path) {
			throw new NotImplementedException();
		}

		public override void Load(string path) {
			throw new NotImplementedException();
		}

		public override void SendUpdate() {
			throw new NotImplementedException();
		}

		public int ChunkCount() {
			return Blocks.Count;
		}
	}
}
