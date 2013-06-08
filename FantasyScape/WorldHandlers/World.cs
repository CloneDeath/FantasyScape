using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Lidgren.Network;
using FantasyScape.NetworkMessages;
using FantasyScape.NetworkMessages.Chunks;

namespace FantasyScape {
	public class World {
		public ChunkManager Chunks = new ChunkManager();

		public World() {
		}

		#region BlockAccessors
		private void GlobalToLocal(Vector3i GlobalCoords, out Vector3i ChunkCoords, out Vector3i BlockCoords) {
			ChunkCoords = new Vector3i();
			BlockCoords = new Vector3i();

			if (GlobalCoords.X >= 0) {
				ChunkCoords.X = GlobalCoords.X / Chunk.Size;
				BlockCoords.X = GlobalCoords.X % Chunk.Size;
			} else {
				ChunkCoords.X = ((GlobalCoords.X + 1) / Chunk.Size) - 1;
				BlockCoords.X = Chunk.Size + (((GlobalCoords.X + 1) % Chunk.Size) - 1);
			}

			if (GlobalCoords.Y >= 0) {
				ChunkCoords.Y = GlobalCoords.Y / Chunk.Size;
				BlockCoords.Y = GlobalCoords.Y % Chunk.Size;
			} else {
				ChunkCoords.Y = ((GlobalCoords.Y + 1) / Chunk.Size) - 1;
				BlockCoords.Y = Chunk.Size + (((GlobalCoords.Y + 1) % Chunk.Size) - 1);
			}

			if (GlobalCoords.Z >= 0) {
				ChunkCoords.Z = GlobalCoords.Z / Chunk.Size;
				BlockCoords.Z = GlobalCoords.Z % Chunk.Size;
			} else {
				ChunkCoords.Z = ((GlobalCoords.Z + 1) / Chunk.Size) - 1;
				BlockCoords.Z = Chunk.Size + (((GlobalCoords.Z + 1) % Chunk.Size) - 1);
			}
		}

		public Block this[Vector3i Location] {
			get {
				Vector3i chunk;
				Vector3i sub;
				GlobalToLocal(Location, out chunk, out sub);

				return Chunks[chunk][sub];
			}
			set {
				Vector3i chunk;
				Vector3i sub;
				GlobalToLocal(Location, out chunk, out sub);

				Chunks[chunk][sub] = value;
			}
		}
		#endregion

		#region Block Update Wrappers
		public void Update() {
			foreach (Chunk c in Chunks) {
				c.Update(this);
			}
		}

		public void RefreshUpdateBlocks(Vector3i Location) {
			AddUpdate(Location);
			AddUpdate(Location + new Vector3i(1, 0, 0));
			AddUpdate(Location - new Vector3i(1, 0, 0));
			AddUpdate(Location + new Vector3i(0, 1, 0));
			AddUpdate(Location - new Vector3i(0, 1, 0));
			AddUpdate(Location + new Vector3i(0, 0, 1));
			AddUpdate(Location - new Vector3i(0, 0, 1));
		}

		public void AddUpdate(Vector3i Location) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(Location, out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords].AddUpdate(BlockCoords);
		}

		public void RemoveUpdate(Vector3i Location) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(Location, out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords].RemoveUpdate(Location);
		}

		#endregion

		#region Block Exposure Wrappers
		public void RefreshExposedBlocks() {
			foreach (Chunk chunk in Chunks) {
				chunk.RefreshExposedBlocks(this);
			}
		}

		public void ExposeBlocksAt(Vector3i Location) {
			ExposeBlock(Location + new Vector3i(1, 0, 0));
			ExposeBlock(Location - new Vector3i(1, 0, 0));
			ExposeBlock(Location + new Vector3i(0, 1, 0));
			ExposeBlock(Location - new Vector3i(0, 1, 0));
			ExposeBlock(Location + new Vector3i(0, 0, 1));
			ExposeBlock(Location - new Vector3i(0, 0, 1));
		}

		public void ExposeBlock(Vector3i Location) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(Location, out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords].ExposeBlock(BlockCoords);
		}

		public void UnexposeBlock(Vector3i Location) {
			Vector3i ChunkCoords;
			Vector3i BlockCoords;
			GlobalToLocal(Location, out ChunkCoords, out BlockCoords);

			Chunks[ChunkCoords].UnexposeBlock(BlockCoords.X, BlockCoords.Y, BlockCoords.Z);
		}

		public void CheckExposure(Vector3i Location) {
			if (this[Location] != null) {
				if (IsExposed(Location)) {
					ExposeBlock(Location);
				} else {
					UnexposeBlock(Location);
				}
			}
		}

		public bool IsExposed(Vector3i location) {
			if (!IsSolid(location + new Vector3i(1, 0, 0)) || !IsSolid(location - new Vector3i(1, 0, 0)) ||
				!IsSolid(location + new Vector3i(0, 1, 0)) || !IsSolid(location - new Vector3i(0, 1, 0)) ||
				!IsSolid(location + new Vector3i(0, 0, 1)) || !IsSolid(location - new Vector3i(0, 0, 1))) {
				return true;
			} else {
				return false;
			}
		}

		#endregion

		#region Block Addition/Removal
		public void AddBlock(Vector3i Location, Block b) {
			Block other = this[Location];
			if (b.CanCombine(other)) {
				b.TryCombine(other);
				this[Location] = b;
			}
		}

		public void RemoveBlock(Vector3i Location) {
			if (this[Location] != null) {
				UnexposeBlock(Location);
				RemoveUpdate(Location);
				this[Location] = null;
				ExposeBlocksAt(Location);
				RefreshUpdateBlocks(Location);
			}
		}

		public void SetBlock(Vector3i Location, Block b) {
			this[Location] = b;

			CheckExposure(Location);
			CheckExposure(Location + new Vector3i(1, 0, 0));
			CheckExposure(Location - new Vector3i(1, 0, 0));
			CheckExposure(Location + new Vector3i(0, 1, 0));
			CheckExposure(Location - new Vector3i(0, 1, 0));
			CheckExposure(Location + new Vector3i(0, 0, 1));
			CheckExposure(Location - new Vector3i(0, 0, 1));

			RefreshUpdateBlocks(Location);
		}
		#endregion

		public void Draw(Player p) {
			int ViewDistance = 4;

			for (int x = -ViewDistance; x <= ViewDistance; x++) {
				for (int y = -ViewDistance; y <= ViewDistance; y++) {
					for (int z = -ViewDistance; z <= ViewDistance; z++) {
						Vector3i TargetChunk = new Vector3i(
							x + (int)(p.xpos / Chunk.Size), 
							y + (int)(p.ypos / Chunk.Size), 
							z + (int)(p.zpos / Chunk.Size));
						Chunks[TargetChunk].Draw(this, p);
					}
				}
			}
			Chunk.DirtyAll = false;
		}

		public bool IsSolid(Vector3i Location) {
			Vector3i ChunkLoc;
			Vector3i BlockLoc;
			GlobalToLocal(Location, out ChunkLoc, out BlockLoc);

			Chunk chunk = null;
			if (!Chunks.TryGet(ChunkLoc, out chunk) || chunk == null) {
				return true;
			} else {
				Block b = chunk[BlockLoc];
				if (b == null)
					return false;
				else {
					return b.isSolid();
				}
			}

		}

		internal bool Ready() {
			return Chunks[new Vector3i()] != Chunk.Null;
		}

		private void TryRefreshChunk(Vector3i loc) {
			Chunk c;
			if (Game.World.Chunks.TryGet(loc, out c)) {
				c.RefreshExposedBlocks(this);
			}
		}
		internal void RefreshExposedChunks(Vector3i Location) {
			TryRefreshChunk(Location + new Vector3i(0, 0, 0));
			
			TryRefreshChunk(Location + new Vector3i(1, 0, 0));
			TryRefreshChunk(Location - new Vector3i(1, 0, 0));
			TryRefreshChunk(Location + new Vector3i(0, 1, 0));
			TryRefreshChunk(Location - new Vector3i(0, 1, 0));
			TryRefreshChunk(Location + new Vector3i(0, 0, 1));
			TryRefreshChunk(Location - new Vector3i(0, 0, 1));
			
		}
	}
}
