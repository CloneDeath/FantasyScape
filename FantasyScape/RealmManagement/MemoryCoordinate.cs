using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.RealmManagement {
	class MemoryCoordinate {
		public Vector3i GlobalCoords;


		public Vector3i ChunkCoord {
			get {
				Vector3i ret = new Vector3i();
				if (GlobalCoords.X >= 0) {
					ret.X = GlobalCoords.X / MemoryChunk.Size.X;
				} else {
					ret.X = ((GlobalCoords.X - 1) / MemoryChunk.Size.X) - 1;
				}

				if (GlobalCoords.Y >= 0) {
					ret.Y = GlobalCoords.Y / MemoryChunk.Size.Y;
				} else {
					ret.Y = ((GlobalCoords.Y - 1) / MemoryChunk.Size.Y) - 1;
				}

				if (GlobalCoords.Z >= 0) {
					ret.Z = GlobalCoords.Z / MemoryChunk.Size.Z;
				} else {
					ret.Z = ((GlobalCoords.Z - 1) / MemoryChunk.Size.Z) - 1;
				}
				return ret;
			}
		}

		public Vector3i LocalCoords {
			get {
				Vector3i ret = new Vector3i();
				if (GlobalCoords.X >= 0) {
					ret.X = GlobalCoords.X % MemoryChunk.Size.X;
				} else {
					ret.X = (MemoryChunk.Size.X + (GlobalCoords.X % MemoryChunk.Size.X)) - 1;
				}

				if (GlobalCoords.Y >= 0) {
					ret.Y = GlobalCoords.Y % MemoryChunk.Size.Y;
				} else {
					ret.Y = (MemoryChunk.Size.Y + (GlobalCoords.Y % MemoryChunk.Size.Y)) - 1;
				}

				if (GlobalCoords.Z >= 0) {
					ret.Z = GlobalCoords.Z % MemoryChunk.Size.Z;
				} else {
					ret.Z = (MemoryChunk.Size.Z + (GlobalCoords.Z % MemoryChunk.Size.Z)) - 1;
				}
				return ret;
			}
		}

		public MemoryCoordinate(Vector3i GlobalCoords) {
			this.GlobalCoords = GlobalCoords;
		}
	}
}
