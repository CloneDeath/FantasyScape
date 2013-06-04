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
		public string BlockTypeName;
		public BlockType BlockType {
			get {
				return BlockTypes.GetBlockType(BlockTypeName);
			}
		}

		public int Level;

		public Block() {
			this.BlockTypeName = "";
			this.Level = 16;
		}

		public Block(string Type) {
			this.BlockTypeName = Type;
			Level = 16;
		}

		public bool isSolid() {
			if (BlockType.Liquid) {
				return this.Level >= 16;
			} else {
				return true;
			}
		}

		public virtual void draw(float x, float y, float z, World world) {
			if (Level <= 0 && BlockType.Liquid) {
				return;
			}
			throw new NotImplementedException();
			//draw(x, y, z, world, Textures.GetTexture(BlockType.TopTexture),
			//    Textures.GetTexture(BlockType.SideTexture),
			//    Textures.GetTexture(BlockType.BotTexture), Level / 16.0f);
		}

		protected void draw(float x, float y, float z, World world, Texture TopTex, Texture SideTex, Texture BottomTex, double height) {
			GL.PushMatrix();
			GL.Translate(x, y, z);
			GL.Scale(1.0f, 1.0f, height);
			if (BlockType.Liquid) {
				GL.Color4(1.0f, 1.0f, 1.0f, 0.25f);
			} else {
				GL.Color3(1.0f, 1.0f, 1.0f);
			}

			GL.BindTexture(TextureTarget.Texture2D, SideTex.ID);
			GL.Begin(BeginMode.Quads);// Draw A Quad
			//Back
			if (!world.IsSolid(x, y - 1, z)) {
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 0.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 1.0f);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 1.0f);
			}

			//Front
			if (!world.IsSolid(x, y + 1, z)) {
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 1.0f);
			}

			//Left
			if (!world.IsSolid(x - 1, y, z)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(0.0f, 0.0f, 1.0f);
			}

			//Right
			if (!world.IsSolid(x + 1, y, z)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 1.0f);
			}



			GL.End();

			GL.BindTexture(TextureTarget.Texture2D, BottomTex.ID);
			GL.Begin(BeginMode.Quads);
			//Bottom
			if (!world.IsSolid(x, y, z - 1)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 0.0f);
			}
			GL.End();

			GL.BindTexture(TextureTarget.Texture2D, TopTex.ID);
			GL.Begin(BeginMode.Quads);
			//Top
			if (!world.IsSolid(x, y, z + 1)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 1.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 1.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 1.0f);
			}
			GL.End();
			GL.PopMatrix();
		}

		bool LiquidUpdated = false;
		static List<Vector2> Directions = new List<Vector2>(
			new Vector2[]{new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1)}
		);
					
		public virtual void update(int x, int y, int z, World world) {
			if (BlockType.Liquid) {
				LiquidUpdated = moveDown(x, y, z - 1, world);
				if (Level > 0) {
					List<Vector2> Copy = Directions.GetRange(0, Directions.Count);
					Copy.Shuffle();
					foreach (Vector2 dir in Copy) {
						LiquidUpdated |= moveTo(x + (int)dir.X, y + (int)dir.Y, z, world);
					}
				}
			} else {
				world.RemoveUpdate(x, y, z);
			}
		}

		public virtual void postUpdate(int x, int y, int z, World world) {
			if (BlockType.Liquid) {
				if (LiquidUpdated) {
					world.RefreshUpdateBlocks(x, y, z);
					new BlockAdd(x, y, z, this).Send();
				} else {
					world.RemoveUpdate(x, y, z);
				}
				if (Level <= 0) {
					world.RemoveBlock(x, y, z);
					new BlockRemove(x, y, z).Send();
				}
			} else {
				world.RemoveUpdate(x, y, z);
			}
		}

		public bool moveDown(int x, int y, int z, World world) {
			return GiveTo(x, y, z, world, 16, 0, true);
		}

		private bool moveTo(int x, int y, int z, World world) {
			return GiveTo(x, y, z, world, 1, 2, false);
		}

		public bool GiveTo(int x, int y, int z, World world, int MaxWater, int MinWaterDiff, bool Down) {
			if (MaxWater > Level) {
				MaxWater = Level;
			}

			if (!world.IsSolid(x, y, z)) {
				if (world[x, y, z] == null) {
					if (this.Level >= MinWaterDiff) {
						Block b = new Block(this.BlockTypeName);
						b.Level = MaxWater;
						Level -= MaxWater;
						world.AddBlock(x, y, z, b);
						new BlockAdd(x, y, z, b).Send();
						return true;
					}
				} else {
					Block b = world[x, y, z];
					if (b.BlockType.Name == this.BlockTypeName) {
						if (b.Level < 16) {
							if (Down) {
								int diff = 16 - b.Level;
								if (diff > MaxWater) {
									diff = MaxWater;
								}
								b.Level += diff;
								this.Level -= diff;
								new BlockAdd(x, y, z, b).Send();
								return true;
							} else {
								int diff = this.Level - b.Level;
								if (diff >= MinWaterDiff) {
									b.Level += MaxWater;
									this.Level -= MaxWater;
									new BlockAdd(x, y, z, b).Send();
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
			nom.Write(BlockTypeName);
			nom.Write((Int32)Level);
		}

		public void Read(NetIncomingMessage nim) {
			BlockTypeName = nim.ReadString();
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
