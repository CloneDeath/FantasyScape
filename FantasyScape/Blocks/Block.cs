using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK.Graphics.OpenGL;
using Lidgren.Network;
using OpenTK;
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class Block {
		public Guid BlockTypeID;
		public BlockType BlockType {
			get {
				return BlockTypes.GetBlockType(BlockTypeID);
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

		public bool isSolid() {
			if (BlockType.Liquid) {
				return this.Level >= 16;
			} else {
				return true;
			}
		}

		#region Draw

		public virtual void draw(Vector3i Location, World world) {
			if (Level <= 0 && BlockType.Liquid) {
				return;
			}
			draw(Location, world, Level / 16.0f);
		}

		protected void draw(Vector3i Location, World world, double height) {
			GL.PushMatrix();
			GL.Translate(Location.X, Location.Y, Location.Z);
			if (BlockType.Liquid) {
				GL.Color4(1.0f, 1.0f, 1.0f, 0.25f);
			} else {
				GL.Color3(1.0f, 1.0f, 1.0f);
			}

			//Back
			GL.BindTexture(TextureTarget.Texture2D, BlockType.GetTexture(Blocks.BlockSide.Back).ID);
			GL.Begin(BeginMode.Quads);
			if (!world.IsSolid(Location - new Vector3i(0, 1, 0))) {
                GL.TexCoord2(0.0, 1 - height); GL.Vertex3(0.0, 0.0, height);
                GL.TexCoord2(1.0, 1 - height); GL.Vertex3(1.0, 0.0, height);
                GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0, 0.0, 0.0);
                GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.0, 0.0, 0.0);
			}
			GL.End();

			//Front
			GL.BindTexture(TextureTarget.Texture2D, BlockType.GetTexture(Blocks.BlockSide.Front).ID);
			GL.Begin(BeginMode.Quads);
			if (!world.IsSolid(Location + new Vector3i(0, 1, 0))) {
                GL.TexCoord2(0.0, 1 - height); GL.Vertex3(1.0f, 1.0f, height);
                GL.TexCoord2(1.0, 1 - height); GL.Vertex3(0.0f, 1.0f, height);
                GL.TexCoord2(1.0, 1.0); GL.Vertex3(0.0f, 1.0f, 0.0f);
                GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.0f, 1.0f, 0.0f);
			}
			GL.End();

			//Left
			GL.BindTexture(TextureTarget.Texture2D, BlockType.GetTexture(Blocks.BlockSide.Left).ID);
			GL.Begin(BeginMode.Quads);
			if (!world.IsSolid(Location - new Vector3i(1, 0, 0))) {
                GL.TexCoord2(0.0, 1-height); GL.Vertex3(0.0f, 1.0f, height);
                GL.TexCoord2(1.0, 1-height); GL.Vertex3(0.0f, 0.0f, height);
                GL.TexCoord2(1.0, 1.0); GL.Vertex3(0.0f, 0.0f, 0.0f);
                GL.TexCoord2(0.0, 1.0); GL.Vertex3(0.0f, 1.0f, 0.0f);
			}
			GL.End();

			//Right
			GL.BindTexture(TextureTarget.Texture2D, BlockType.GetTexture(Blocks.BlockSide.Right).ID);
			GL.Begin(BeginMode.Quads);
			if (!world.IsSolid(Location + new Vector3i(1, 0, 0))) {
                GL.TexCoord2(0.0, 1-height); GL.Vertex3(1.0f, 0.0f, height);
                GL.TexCoord2(1.0, 1-height); GL.Vertex3(1.0f, 1.0f, height);
                GL.TexCoord2(1.0, 1.0); GL.Vertex3(1.0f, 1.0f, 0.0f);
                GL.TexCoord2(0.0, 1.0); GL.Vertex3(1.0f, 0.0f, 0.0f);
			}
			GL.End();

			//Bottom
			GL.BindTexture(TextureTarget.Texture2D, BlockType.GetTexture(Blocks.BlockSide.Bottom).ID);
			GL.Begin(BeginMode.Quads);
			if (!world.IsSolid(Location - new Vector3i(0, 0, 1))) {
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.0f);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 0.0f);
			}
			GL.End();

			//Top
			GL.BindTexture(TextureTarget.Texture2D, BlockType.GetTexture(Blocks.BlockSide.Top).ID);
			GL.Begin(BeginMode.Quads);
			if (!world.IsSolid(Location + new Vector3i(0, 0, 1))) {
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, height);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, height);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, height);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, height);
			}
			GL.End();
			GL.PopMatrix();
		}

		#endregion

		bool LiquidUpdated = false;
		static List<Vector2> Directions = new List<Vector2>(
			new Vector2[]{new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1)}
		);
					
		public virtual void update(Vector3i Location, World world) {
			if (BlockType.Liquid) {
				LiquidUpdated = moveDown(Location - new Vector3i(0, 0, 1), world);
				if (Level > 0) {
					List<Vector2> Copy = Directions.GetRange(0, Directions.Count);
					Copy.Shuffle();
					foreach (Vector2 dir in Copy) {
						LiquidUpdated |= moveTo(new Vector3i(Location.X + (int)dir.X, Location.Y + (int)dir.Y, Location.Z), world);
					}
				}
			} else {
				world.RemoveUpdate(Location);
			}
		}

		public virtual void postUpdate(Vector3i Location, World world) {
			if (BlockType.Liquid) {
				if (LiquidUpdated) {
					world.RefreshUpdateBlocks(Location);
					new BlockAdd(Location, this).Send();
				} else {
					world.RemoveUpdate(Location);
				}
				if (Level <= 0) {
					world.RemoveBlock(Location);
					new BlockRemove(Location).Send();
				}
			} else {
				world.RemoveUpdate(Location);
			}
		}

		public bool moveDown(Vector3i Location, World world) {
			return GiveTo(Location, world, 16, 0, true);
		}

		private bool moveTo(Vector3i Location, World world) {
			return GiveTo(Location, world, 1, 2, false);
		}

		public bool GiveTo(Vector3i Location, World world, int MaxWater, int MinWaterDiff, bool Down) {
			if (MaxWater > Level) {
				MaxWater = Level;
			}

			if (!world.IsSolid(Location)) {
				if (world[Location] == null) {
					if (this.Level >= MinWaterDiff) {
						Block b = new Block(this.BlockTypeID);
						b.Level = MaxWater;
						Level -= MaxWater;
						world.AddBlock(Location, b);
						new BlockAdd(Location, b).Send();
						return true;
					}
				} else {
					Block b = world[Location];
					if (b.BlockType.ID == this.BlockTypeID) {
						if (b.Level < 16) {
							if (Down) {
								int diff = 16 - b.Level;
								if (diff > MaxWater) {
									diff = MaxWater;
								}
								b.Level += diff;
								this.Level -= diff;
								new BlockAdd(Location, b).Send();
								return true;
							} else {
								int diff = this.Level - b.Level;
								if (diff >= MinWaterDiff) {
									b.Level += MaxWater;
									this.Level -= MaxWater;
									new BlockAdd(Location, b).Send();
									return true;
								}
								
								return false;
							}
						}
					}
				}
			}
			return false;
		}
		

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
