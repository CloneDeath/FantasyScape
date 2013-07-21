using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.RealmManagement {
	public class MemoryChunk {
		public static readonly Vector3i Size = new Vector3i(30, 30, 30);

		public Block[, ,] BlockData = new Block[Size.X, Size.Y, Size.Z];
		public Vector3i Location;

		public MemoryChunk(Vector3i ChunkLocation) {
			this.Location = ChunkLocation;
		}

		public Block this[Vector3i Location] {
			get {
				return BlockData[Location.X, Location.Y, Location.Z];
			}
			set {
				BlockData[Location.X, Location.Y, Location.Z] = value;
			}
		}
	}
}
