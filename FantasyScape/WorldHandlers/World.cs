using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Lidgren.Network;
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class World {
		public const int XSize = 16;
		public const int YSize = 16;
		public const int ZSize = 16;

		public Chunk[, ,] Chunks = new Chunk[XSize, YSize, ZSize];

		public World() {
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = 0; z < ZSize; z++) {
						Chunks[x, y, z] = new Chunk();
					}
				}
			}
		}

		#region BlockAccessors
		private void GlobalToLocal(Vector3i GlobalCoords, out Vector3i ChunkCoords, out Vector3i BlockCoords) {
			ChunkCoords = new Vector3i(
				GlobalCoords.X / Chunk.Size,
				GlobalCoords.Y / Chunk.Size,
				GlobalCoords.Z / Chunk.Size);

			BlockCoords = new Vector3i(
				GlobalCoords.X % Chunk.Size,
				GlobalCoords.Y % Chunk.Size,
				GlobalCoords.Z % Chunk.Size);
		}

		public Block this[int x, int y, int z] {
			get {
				if (OutsideBounds(x, y, z)) {
					return null;
				}

				Vector3i chunk;
				Vector3i sub;
				GlobalToLocal(new Vector3i(x, y, z), out chunk, out sub);

				return Chunks[chunk.X, chunk.Y, chunk.Z][sub.X, sub.Y, sub.Z];
			}

			set {
				if (OutsideBounds(x, y, z)) {
					return;
				}

				Vector3i chunk;
				Vector3i sub;
				GlobalToLocal(new Vector3i(x, y, z), out chunk, out sub);

				Chunks[chunk.X, chunk.Y, chunk.Z][sub.X, sub.Y, sub.Z] = value;
			}
		}

		private static bool OutsideBounds(int x, int y, int z) {
			return x < 0 || x >= XSize * Chunk.Size || y < 0 || y >= YSize * Chunk.Size || z < 0 || z >= ZSize * Chunk.Size;
		}
		#endregion

		#region Client World Initialization
		public int ChunkCount = 0;
		bool SentRequest = false;
		internal bool Ready() {
			if (!SentRequest) {
				SentRequest = true;
				RequestMessage msg = new RequestMessage(RequestType.Chunks);
				msg.Send();
			}

			return ChunkCount == XSize * YSize * ZSize;
		}
		#endregion

		#region Block Update Wrappers
		public void Update() {
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = 0; z < ZSize; z++) {
						Chunks[x, y, z].Update(x, y, z, this);
					}
				}
			}
		}

		public void RefreshUpdateBlocks(int x, int y, int z) {
			AddUpdate(x, y, z);
			AddUpdate(x + 1, y, z);
			AddUpdate(x - 1, y, z);
			AddUpdate(x, y + 1, z);
			AddUpdate(x, y - 1, z);
			AddUpdate(x, y, z + 1);
			AddUpdate(x, y, z - 1);
		}

		public void AddUpdate(int x, int y, int z) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(new Vector3i(x, y, z), out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords.X, ChunkCoords.Y, ChunkCoords.Z].AddUpdate(BlockCoords.X, BlockCoords.Y, BlockCoords.Z);
		}

		public void RemoveUpdate(int x, int y, int z) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(new Vector3i(x, y, z), out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords.X, ChunkCoords.Y, ChunkCoords.Z].RemoveUpdate(BlockCoords.X, BlockCoords.Y, BlockCoords.Z);
		}

		#endregion

		#region Block Exposure Wrappers
		public void RefreshExposedBlocks() {
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = 0; z < ZSize; z++) {
						Chunks[x, y, z].RefreshExposedBlocks(x, y, z, this);
					}
				}
			}
		}

		public void ExposeBlocksAt(int x, int y, int z) {
			ExposeBlock(x + 1, y, z);
			ExposeBlock(x - 1, y, z);
			ExposeBlock(x, y + 1, z);
			ExposeBlock(x, y - 1, z);
			ExposeBlock(x, y, z + 1);
			ExposeBlock(x, y, z - 1);
		}

		public void ExposeBlock(int x, int y, int z) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(new Vector3i(x, y, z), out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords.X, ChunkCoords.Y, ChunkCoords.Z].ExposeBlock(BlockCoords.X, BlockCoords.Y, BlockCoords.Z);
		}

		public void UnexposeBlock(int x, int y, int z) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(new Vector3i(x, y, z), out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords.X, ChunkCoords.Y, ChunkCoords.Z].UnexposeBlock(BlockCoords.X, BlockCoords.Y, BlockCoords.Z);
		}

		public void CheckExposure(int x, int y, int z) {
			if (this[x, y, z] != null) {
				if (IsExposed(x, y, z)) {
					ExposeBlock(x, y, z);
				} else {
					UnexposeBlock(x, y, z);
				}
			}
		}

		public bool IsExposed(float x, float y, float z) {
			int xat = (int)x;
			int yat = (int)y;
			int zat = (int)z;

			if (xat >= XSize * Chunk.Size || xat < 0 || zat >= ZSize * Chunk.Size || zat < 0 || yat < 0 || yat >= YSize * Chunk.Size) {
				return false;
			}

			if (this[xat, yat, zat] == null) {
				return false;
			}

			if (!IsSolid(xat, yat + 1, zat) || !IsSolid(xat + 1, yat, zat) ||
				!IsSolid(xat - 1, yat, zat) || !IsSolid(xat, yat, zat + 1) ||
				!IsSolid(xat, yat, zat - 1) || !IsSolid(xat, yat - 1, zat)) {
				return true;
			} else {
				return false;
			}
		}

		#endregion

		#region Block Addition/Removal
		public void AddBlock(int x, int y, int z, Block b) {
			if (b.CanCombine(this[x, y, z])) {
				b.TryCombine(this[x, y, z]);
				SetBlock(x, y, z, b);
			}
		}

		public void RemoveBlock(int x, int y, int z) {
			if (this[x, y, z] != null) {
				UnexposeBlock(x, y, z);
				RemoveUpdate(x, y, z);
				this[x, y, z] = null;
				ExposeBlocksAt(x, y, z);
				RefreshUpdateBlocks(x, y, z);
			}
		}

		public void SetBlock(int x, int y, int z, Block b) {
			this[x, y, z] = b;

			CheckExposure(x, y, z);
			CheckExposure(x + 1, y, z);
			CheckExposure(x - 1, y, z);
			CheckExposure(x, y + 1, z);
			CheckExposure(x, y - 1, z);
			CheckExposure(x, y, z + 1);
			CheckExposure(x, y, z - 1);

			RefreshUpdateBlocks(x, y, z);
		}
		#endregion

		public void Draw(Player p) {
			//int ViewDistance = 100;
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = 0; z < ZSize; z++) {
						Chunks[x, y, z].Draw(x, y, z, this, p);
					}
				}
			}
			Chunk.DirtyAll = false;
		}

		public bool IsSolid(double x, double y, double z) {
			int xat = (int)x;
			int yat = (int)y;
			int zat = (int)z;

			if (xat >= XSize * Chunk.Size || xat < 0 || yat >= YSize * Chunk.Size || yat < 0 || zat < 0) {
				return true;
			}

			if (zat >= ZSize * Chunk.Size) {
				return false;
			}


			if (this[xat, yat, zat] == null)
				return false;
			else {
				return this[xat, yat, zat].isSolid();
			}

		}
	}
}
