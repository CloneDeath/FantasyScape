using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK.Graphics.OpenGL;
using Lidgren.Network;

namespace FantasyScape {
	public class Block {
		public string BlockTypeName;
		public BlockType BlockType {
			get {
				return BlockTypes.GetBlockType(BlockTypeName);
			}
		}

		float level;
		const float minLevel = 0.01f;

		public Block() {
			this.BlockTypeName = "";
			this.level = 1.0f;
		}

		public Block(string Type) {
			this.BlockTypeName = Type;
			level = 1.0f;
		}

		public bool isSolid() {
			if (BlockType.Liquid) {
				if (level >= 0.9) {
					return true;
				} else {
					return false;
				}
			} else {
				return true;
			}
		}

		public virtual void draw(float x, float y, float z, World world) {
			if (level < minLevel && BlockType.Liquid) {
				return;
			}
			draw(x, y, z, world, Textures.GetTexture(BlockType.TopTexture),
				Textures.GetTexture(BlockType.SideTexture),
				Textures.GetTexture(BlockType.BotTexture), level);
		}

		protected void draw(float x, float y, float z, World world, Texture TopTex, Texture SideTex, Texture BottomTex, double height) {
			GL.PushMatrix();
			GL.Translate(x, y, z);
			GL.Scale(1.0f, 1.0f, height);
			GL.Color3(1.0f, 1.0f, 1.0f);

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
		public virtual void update(int x, int y, int z, World world) {
			if (BlockType.Liquid) {
				LiquidUpdated = moveDown(x, y, z - 1, world);
				if (level > minLevel) {
					LiquidUpdated |= moveTo(x + 1, y, z, world);
					LiquidUpdated |= moveTo(x - 1, y, z, world);
					LiquidUpdated |= moveTo(x, y + 1, z, world);
					LiquidUpdated |= moveTo(x, y - 1, z, world);
				}
			} else {
				world.removeUpdate(x, y, z);
			}
		}

		public virtual void postUpdate(int x, int y, int z, World world) {
			if (BlockType.Liquid) {
				if (!LiquidUpdated) {
					world.removeUpdate(x, y, z);
				}
				if (level <= minLevel) {
					world.RemoveBlock(x, y, z);
				}
			} else {
				world.removeUpdate(x, y, z);
			}
		}

		public bool moveDown(int x, int y, int z, World world) {
			if (!world.IsSolid(x, y, z)) {
				if (world.blockAt(x, y, z) == null) {
					Block b = new Block("Water");
					world.addBlock(x, y, z, b);
					b.level = level;
					level = 0;
					return true;
				} else {
					Block b = world.blockAt(x, y, z);
					if (b.BlockType.Name == "Water") {
						if (b.level < 1.0f) {
							float diff = (1.0f - b.level);
							if (diff > level) {
								b.level += level;
								level = 0;
								world.RemoveBlock(x, y, z + 1);
							} else {
								b.level += diff;
								level -= diff;
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool moveTo(int x, int y, int z, World world) {
			if (!world.IsSolid(x, y, z)) {
				if (world.blockAt(x, y, z) == null) {
					Block b = new Block("Water");
					world.addBlock(x, y, z, b);
					b.level = level / 4.0f;
					level = level * 3.0f / 4.0f;
					return true;
				} else {
					Block b = world.blockAt(x, y, z);
					if (b.BlockType.Name == "Water") {
						if (b.level < level) {
							float diff = (level - b.level) / 4.0f;
							float cgive = level / 4.0f;
							if (cgive > diff) {
								b.level += diff;
								level -= diff;
							} else {
								b.level += cgive;
								level -= cgive;
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		public void Write(NetOutgoingMessage nom) {
			nom.Write(BlockTypeName);
			nom.Write(level);
		}

		public void Read(NetIncomingMessage nim) {
			BlockTypeName = nim.ReadString();
			level = nim.ReadFloat();
		}
	}
}
