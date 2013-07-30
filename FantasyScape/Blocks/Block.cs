using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK.Graphics.OpenGL;
using Lidgren.Network;
using OpenTK;
using FantasyScape.NetworkMessages;
using FantasyScape.RealmManagement;

namespace FantasyScape {
	public class Block {
		public Guid BlockTypeID;
		public BlockType BlockType {
			get {
				return BlockType.Get(BlockTypeID);
			}
		}

		public int Level;
		public Block() {
			this.BlockTypeID = Guid.Empty;
			this.Level = 16;
		}

		public Block(Guid Type) {
			this.BlockTypeID = Type;
			Level = 16;
		}

		public bool IsSolid() {
			if (BlockType.Liquid) {
				return this.Level >= 16;
			} else {
				return true;
			}
		}

		bool LiquidUpdated = false;
		static List<Vector2> Directions = new List<Vector2>(
			new Vector2[]{new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1)}
		);
					
		public virtual void update(Vector3i Location, Realm world) {
			throw new NotImplementedException();
			//if (BlockType.Liquid) {
			//    LiquidUpdated = moveDown(Location - new Vector3i(0, 0, 1), world);
			//    if (Level > 0) {
			//        List<Vector2> Copy = Directions.GetRange(0, Directions.Count);
			//        Copy.Shuffle();
			//        foreach (Vector2 dir in Copy) {
			//            LiquidUpdated |= moveTo(new Vector3i(Location.X + (int)dir.X, Location.Y + (int)dir.Y, Location.Z), world);
			//        }
			//    }
			//} else {
			//    world.RemoveUpdate(Location);
			//}
		}

		//public virtual void postUpdate(Vector3i Location, Realm world) {
		//    if (BlockType.Liquid) {
		//        if (LiquidUpdated) {
		//            world.RefreshUpdateBlocks(Location);
		//            new BlockAdd(Location, this).Send();
		//        } else {
		//            world.RemoveUpdate(Location);
		//        }
		//        if (Level <= 0) {
		//            world.RemoveBlock(Location);
		//            new BlockRemove(Location).Send();
		//        }
		//    } else {
		//        world.RemoveUpdate(Location);
		//    }
		//}

		//public bool moveDown(Vector3i Location, Realm world) {
		//    return GiveTo(Location, world, 16, 0, true);
		//}

		//private bool moveTo(Vector3i Location, Realm world) {
		//    return GiveTo(Location, world, 1, 2, false);
		//}

		//public bool GiveTo(Vector3i Location, Realm world, int MaxWater, int MinWaterDiff, bool Down) {
		//    if (MaxWater > Level) {
		//        MaxWater = Level;
		//    }

		//    if (!world.IsSolid(Location)) {
		//        if (world[Location] == null) {
		//            if (this.Level >= MinWaterDiff) {
		//                Block b = new Block(this.BlockTypeID);
		//                b.Level = MaxWater;
		//                Level -= MaxWater;
		//                world.AddBlock(Location, b);
		//                new BlockAdd(Location, b).Send();
		//                return true;
		//            }
		//        } else {
		//            Block b = world[Location];
		//            if (b.BlockType.ID == this.BlockTypeID) {
		//                if (b.Level < 16) {
		//                    if (Down) {
		//                        int diff = 16 - b.Level;
		//                        if (diff > MaxWater) {
		//                            diff = MaxWater;
		//                        }
		//                        b.Level += diff;
		//                        this.Level -= diff;
		//                        new BlockAdd(Location, b).Send();
		//                        return true;
		//                    } else {
		//                        int diff = this.Level - b.Level;
		//                        if (diff >= MinWaterDiff) {
		//                            b.Level += MaxWater;
		//                            this.Level -= MaxWater;
		//                            new BlockAdd(Location, b).Send();
		//                            return true;
		//                        }
								
		//                        return false;
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    return false;
		//}
		

		public void Write(NetOutgoingMessage nom) {
			nom.Write(BlockTypeID.ToString());
			nom.Write((Int32)Level);
		}

		public void Read(NetIncomingMessage nim) {
			if (!Guid.TryParse(nim.ReadString(), out BlockTypeID)) {
				throw new Exception("Failed to read GUID");
			}
			Level = nim.ReadInt32();
		}

		internal void TryCombine(Block block) {
			if (CanCombine(block) && block != null) {
				this.Level += block.Level;
			}
		}

		internal bool CanCombine(Block block) {
			if (block == null) return true;
			return this.BlockType == block.BlockType && this.BlockType.Liquid;
		}
	}
}
