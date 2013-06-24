using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using GLImp;

namespace FantasyScape {
	public class Chunk {
		public static Chunk Null = new Chunk(null);

		public const int Size = 8;
		Block[, ,] Blocks = new Block[Size, Size, Size];

		Vector3i Location;

		List<Block> exposedBlocks;
		List<Vector3i> exposedLocations;

		public List<Block> updateBlocks;
		public List<Vector3i> updateLocations;

		public bool Dirty = true;
		int DisplayList;

		public Chunk(Vector3i Location) {
			Array.Clear(Blocks, 0, Size * Size * Size);
			updateBlocks = new List<Block>();
			updateLocations = new List<Vector3i>();
			Dirty = true;
			if (Game.Render) {
				DisplayList = GraphicsManager.GenList(1);
			} else {
				DisplayList = 0;
			}
			this.Location = Location;
		}

		#region Block Access
		public Block this[Vector3i Location] {
			get {
				if (Location.X < 0 || Location.X >= Size || 
					Location.Y < 0 || Location.Y >= Size || 
					Location.Z < 0 || Location.Z >= Size) {
					return null;
				} else {
					return Blocks[Location.X, Location.Y, Location.Z];
				}
			}

			set {
				if (Location.X < 0 || Location.X >= Size || 
					Location.Y < 0 || Location.Y >= Size || 
					Location.Z < 0 || Location.Z >= Size) {
					return;
				} else {
					Blocks[Location.X, Location.Y, Location.Z] = value;
					Dirty = true;
				}
			}
		}
		#endregion

		#region Update
		public void Update(World parent) {
			if (updateBlocks.Count != 0) {
				Dirty = true;
				Block[] TempBlocks = new Block[updateBlocks.Count];
				updateBlocks.CopyTo(TempBlocks);

				Vector3i[] TempLocs = new Vector3i[updateLocations.Count];
				updateLocations.CopyTo(TempLocs);

				for (int i = 0; i < TempBlocks.Count(); i++) {
					Vector3i loc = TempLocs[i];
					TempBlocks[i].update(loc + (Location * Size), parent);
				}

				for (int i = 0; i < TempBlocks.Count(); i++) {
					Vector3i loc = TempLocs[i];
					TempBlocks[i].postUpdate(loc + (Location * Size), parent);
				}
			}
		}

		internal void AddUpdate(Vector3i Location) {
			Block b = this[Location];
			if (b != null && !updateBlocks.Contains(b)) {
				updateBlocks.Add(b);
				updateLocations.Add(Location);
				Dirty = true;
			}
		}

		internal void RemoveUpdate(Vector3i Location) {
			Block b = this[Location];
			if (b != null && updateBlocks.Contains(b)) {
				int rval = updateBlocks.IndexOf(b);
				updateBlocks.RemoveAt(rval);
				updateLocations.RemoveAt(rval);
				Dirty = true;
			}
		}
		#endregion

		#region Serialization
		internal void Write(Lidgren.Network.NetOutgoingMessage Message) {
			for (int x = 0; x < Size; x++) {
				for (int y = 0; y < Size; y++) {
					for (int z = 0; z < Size; z++) {
						Vector3i Location = new Vector3i(x, y, z);
						if (this[Location] != null) {
							Message.Write(true);
							this[Location].Write(Message);
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
						Vector3i Location = new Vector3i(x, y, z);
						if (BlockExists) {
							this[Location] = new Block();
							this[Location].Read(Message);
						} else {
							this[Location] = null;
						}
					}
				}
			}
		}
		#endregion

		internal void ExposeBlock(Vector3i Location) {
			if (this[Location] != null && Game.Render) {
				if (!exposedLocations.Contains(Location)) {
					exposedBlocks.Add(this[Location]);
					exposedLocations.Add(Location);
				} else {
					int index = exposedLocations.IndexOf(Location);
					exposedBlocks[index] = this[Location];
				}
				Dirty = true;
			}
		}

		internal void RefreshExposedBlocks(World world) {
			if (Game.Render) {
				exposedBlocks = new List<Block>();
				exposedLocations = new List<Vector3i>();

				for (int x = 0; x < Size; x++) {
					for (int y = 0; y < Size; y++) {
						for (int z = 0; z < Size; z++) {
							Vector3i loc = new Vector3i(x, y, z);
							if (this[loc] != null && world.IsExposed(loc + (Location * Size))) {
								exposedBlocks.Add(this[loc]);
								exposedLocations.Add(loc);
							}
						}
					}
				}
				Dirty = true;
			}
		}

		internal void UnexposeBlock(int x, int y, int z) {
			if (Game.Render) {
				int remi = exposedLocations.IndexOf(new Vector3i(x, y, z));
				if (remi != -1) {
					exposedBlocks.RemoveAt(remi);
					exposedLocations.RemoveAt(remi);
				}
				Dirty = true;
			}
		}

		public bool ChunkInFrustum(Player p) {
			Vector3i Offset = Location * Size;

			if (p.xpos >= Offset.X && p.xpos <= Offset.X + Size && p.ypos >= Offset.Y && p.ypos <= Offset.Y + Size
				&& p.zpos >= Offset.Z && p.zpos <= Offset.Z + Size) {
				return true;
			}

			return p.frustum.sphereInFrustum(new Vector3(Offset.X + (Size / 2), Offset.Y + (Size / 2), Offset.Z + (Size / 2)), 
				(float)(Size)) != Frustum.OUTSIDE;
		}

		public void Draw(World w, Player p) {
			if (Game.Render && Location != null && ChunkInFrustum(p) && exposedBlocks != null) {
				if (Dirty) {
					Dirty = false;
					Vector3i Offset = Location * Size;

					GraphicsManager.BeginList(DisplayList);
					for (int i = 0; i < exposedBlocks.Count(); i++) {
						Vector3i loc = exposedLocations[i];
						exposedBlocks[i].draw(loc + Offset, w);
					}
					GraphicsManager.EndList();
				}

				GraphicsManager.CallList(DisplayList);
			}
		}
	}
}
