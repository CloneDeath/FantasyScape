using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK.Graphics.OpenGL;
using Lidgren.Network;
using OpenTK;

namespace FantasyScape {
	public class Block {
		public string BlockTypeName;
		public BlockType BlockType {
			get {
				return BlockTypes.GetBlockType(BlockTypeName);
			}
		}

		int level;

		public Block() {
			this.BlockTypeName = "";
			this.level = 16;
		}

		public Block(string Type) {
			this.BlockTypeName = Type;
			level = 16;
		}

		public bool isSolid() {
			if (BlockType.Liquid) {
				return this.level >= 16;
			} else {
				return true;
			}
		}

		public virtual void draw(float x, float y, float z, World world) {
			if (level <= 0 && BlockType.Liquid) {
				return;
			}
			draw(x, y, z, world, Textures.GetTexture(BlockType.TopTexture),
				Textures.GetTexture(BlockType.SideTexture),
				Textures.GetTexture(BlockType.BotTexture), level / 16.0f);
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
				if (level > 0) {
					List<Vector2> Copy = Directions.GetRange(0, Directions.Count);
					Copy.Shuffle();
					foreach (Vector2 dir in Copy) {
						LiquidUpdated |= moveTo(x + (int)dir.X, y + (int)dir.Y, z, world);
					}
				}
			} else {
				world.removeUpdate(x, y, z);
			}
		}

		public virtual void postUpdate(int x, int y, int z, World world) {
			if (BlockType.Liquid) {
				if (LiquidUpdated) {
					world.refreshUpdateBlocks(x, y, z);
				} else {
					world.removeUpdate(x, y, z);
				}
				if (level <= 0) {
					world.RemoveBlock(x, y, z);
				}
			} else {
				world.removeUpdate(x, y, z);
			}
		}

		public bool moveDown(int x, int y, int z, World world) {
			return GiveTo(x, y, z, world, 16, 0, true);
		}

		private bool moveTo(int x, int y, int z, World world) {
			return GiveTo(x, y, z, world, 1, 2, false);
		}

		public bool GiveTo(int x, int y, int z, World world, int MaxWater, int MinWaterDiff, bool Down) {
			if (MaxWater > level) {
				MaxWater = level;
			}

			if (!world.IsSolid(x, y, z)) {
				if (world.blockAt(x, y, z) == null) {
					if (this.level >= MinWaterDiff) {
						Block b = new Block(this.BlockTypeName);
						world.addBlock(x, y, z, b);
						b.level = MaxWater;
						level -= MaxWater;
						return true;
					}
				} else {
					Block b = world.blockAt(x, y, z);
					if (b.BlockType.Name == this.BlockTypeName) {
						if (b.level < 16) {
							if (Down) {
								int diff = 16 - b.level;
								if (diff > MaxWater) {
									diff = MaxWater;
								}
								b.level += diff;
								this.level -= diff;
								return true;
							} else {
								int diff = this.level - b.level;
								if (diff >= MinWaterDiff) {
									b.level += MaxWater;
									this.level -= MaxWater;
								}
								
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		

		public void Write(NetOutgoingMessage nom) {
			nom.Write(BlockTypeName);
			nom.Write((Int32)level);
		}

		public void Read(NetIncomingMessage nim) {
			BlockTypeName = nim.ReadString();
			level = nim.ReadInt32();
		}
	}
}
