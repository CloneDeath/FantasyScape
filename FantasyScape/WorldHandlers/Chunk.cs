using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace FantasyScape {
	public class Chunk {
		public const int Size = 16;
		Block[, ,] Blocks = new Block[Size, Size, Size];

		List<Block> exposedBlocks;
		List<Vector3i> exposedLocations;

		public List<Block> updateBlocks;
		public List<Vector3i> updateLocations;
		
		public Chunk() {
			Array.Clear(Blocks, 0, Size * Size * Size);
			updateBlocks = new List<Block>();
			updateLocations = new List<Vector3i>();
		}

		#region Block Access
		public Block this[int x, int y, int z] {
			get {
				if (x < 0 || x >= Size || y < 0 || y >= Size || z < 0 || z >= Size) {
					return null;
				} else {
					return Blocks[x, y, z];
				}
			}

			set {
				if (x < 0 || x >= Size || y < 0 || y >= Size || z < 0 || z >= Size) {
					return;
				} else {
					Blocks[x, y, z] = value;
				}
			}
		}
		#endregion

		#region Update
		public void Update(int x, int y, int z, World parent) {
			if (updateBlocks.Count != 0) {
				Block[] TempBlocks = new Block[updateBlocks.Count];
				updateBlocks.CopyTo(TempBlocks);

				Vector3i[] TempLocs = new Vector3i[updateLocations.Count];
				updateLocations.CopyTo(TempLocs);

				for (int i = 0; i < TempBlocks.Count(); i++) {
					Vector3i loc = TempLocs[i];
					TempBlocks[i].update((x * Size) + loc.X, (y * Size) + loc.Y, (z * Size) + loc.Z, parent);
				}

				for (int i = 0; i < TempBlocks.Count(); i++) {
					Vector3i loc = TempLocs[i];
					TempBlocks[i].postUpdate((x * Size) + loc.X, (y * Size) + loc.Y, (z * Size) + loc.Z, parent);
				}
			}
		}

		internal void AddUpdate(int x, int y, int z) {
			Block b = this[x, y, z];
			if (b != null && !updateBlocks.Contains(b)) {
				updateBlocks.Add(b);
				updateLocations.Add(new Vector3i(x, y, z));
			}
		}

		internal void RemoveUpdate(int x, int y, int z) {
			Block b = this[x, y, z];
			if (b != null && updateBlocks.Contains(b)) {
				int rval = updateBlocks.IndexOf(b);
				updateBlocks.RemoveAt(rval);
				updateLocations.RemoveAt(rval);
			}
		}
		#endregion

		#region Serialization
		internal void Write(Lidgren.Network.NetOutgoingMessage Message) {
			for (int x = 0; x < Size; x++) {
				for (int y = 0; y < Size; y++) {
					for (int z = 0; z < Size; z++) {
						if (this[x, y, z] != null) {
							Message.Write(true);
							this[x, y, z].Write(Message);
						} else {
							Message.Write(false);
						}
					}
				}
			}
		}

		internal void Read(Lidgren.Network.NetIncomingMessage Message) {
			for (int x = 0; x < Size; x++) {
				for (int y = 0; y < Size; y++) {
					for (int z = 0; z < Size; z++) {
						bool BlockExists = Message.ReadBoolean();
						if (BlockExists) {
							this[x, y, z] = new Block();
							this[x, y, z].Read(Message);
						} else {
							this[x, y, z] = null;
						}
					}
				}
			}
		}
		#endregion

		internal void ExposeBlock(int x, int y, int z) {
			if (this[x, y, z] != null) {
				if (!exposedBlocks.Contains(this[x, y, z])) {
					exposedBlocks.Add(this[x, y, z]);
					exposedLocations.Add(new Vector3i( x, y, z ));
				}
			}
		}

		internal void RefreshExposedBlocks(int X, int Y, int Z, World world) {
			exposedBlocks = new List<Block>();
			exposedLocations = new List<Vector3i>();

			for (int x = 0; x < Size; x++) {
				for (int y = 0; y < Size; y++) {
					for (int z = 0; z < Size; z++) {
						if (world.IsExposed(x + (X * Size), y + (Y * Size), z + (Z * Size))) {
							exposedBlocks.Add(this[x, y, z]);
							exposedLocations.Add(new Vector3i(x, y, z));
						}
					}
				}
			}
		}

		internal void UnexposeBlock(int x, int y, int z) {
			int remi = exposedBlocks.IndexOf(this[x, y, z]);
			if (remi != -1) {
				exposedBlocks.RemoveAt(remi);
				exposedLocations.RemoveAt(remi);
			}
		}

		public void Draw(int x, int y, int z, World w, Player p) {
			int XOffset = x * Size;
			int YOffset = y * Size;
			int ZOffset = z * Size;

			for (int i = 0; i < exposedBlocks.Count(); i++) {
				Vector3i loc = exposedLocations[i];
				//if (Math.Abs(p.xpos - loc.X) < ViewDistance &&
				//    Math.Abs(p.ypos - loc[1]) < ViewDistance &&
				//    Math.Abs(p.zpos - loc[2]) < ViewDistance) {
					Vector3 box = new Vector3(loc.X + 0.5f + XOffset, loc.Y + 0.5f + YOffset, loc.Z + 0.5f + ZOffset);
					if (p.frustum.pointInFrustum(box) != Frustum.OUTSIDE) {
						exposedBlocks[i].draw(loc.X + XOffset, loc.Y + YOffset, loc.Z + ZOffset, w);
					}
				//}
			}
		}
	}
}
