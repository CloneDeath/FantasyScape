using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK.Graphics.OpenGL;

namespace FantasyScape {
	class Block {
		public int BlockID = 0;

		public Block() {
		}

		public virtual bool isSolid() {
			return true;
		}

		public virtual void draw(float x, float y, float z, World world) {
			draw(x, y, z, world, Textures.Dirt, Textures.Dirt, Textures.Dirt);
		}

		protected void draw(float x, float y, float z, World world, Texture TopTex, Texture SideTex, Texture BottomTex) {
			draw(x, y, z, world, TopTex, SideTex, BottomTex, 1.0f);
		}

		protected void draw(float x, float y, float z, World world, Texture TopTex, Texture SideTex, Texture BottomTex, float height) {
			GL.PushMatrix();
			GL.Translate(x, y, z);
			GL.Scale(1.0f, 1.0f, height);
			GL.Color3(1.0f, 1.0f, 1.0f);

			//GL11.glColor4f(1.0f,1.0f,1.0f, 1.0f);                 // Set The Color To Blue One Time Only
			GL.BindTexture(TextureTarget.Texture2D, SideTex.ID);
			GL.Begin(BeginMode.Quads);                        // Draw A Quad
			//Back
			if (!world.isSolid(x, y - 1, z)) {
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 0.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 1.0f);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 1.0f);
			}

			//Front
			if (!world.isSolid(x, y + 1, z)) {
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 1.0f);
			}

			//Left
			if (!world.isSolid(x - 1, y, z)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(0.0f, 0.0f, 1.0f);
			}

			//Right
			if (!world.isSolid(x + 1, y, z)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 1.0f);
			}



			GL.End();

			GL.BindTexture(TextureTarget.Texture2D, BottomTex.ID);
			GL.Begin(BeginMode.Quads);
			//Bottom
			if (!world.isSolid(x, y, z - 1)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 0.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 0.0f);
			}
			GL.End();

			GL.BindTexture(TextureTarget.Texture2D, TopTex.ID);
			GL.Begin(BeginMode.Quads);
			//Top
			if (!world.isSolid(x, y, z + 1)) {
				GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 1.0f);
				GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 1.0f, 1.0f);
				GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
				GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 0.0f, 1.0f);
			}
			GL.End();
			GL.PopMatrix();
		}


		public virtual void update(int x, int y, int z, World world) {
			world.removeUpdate(x, y, z);
		}

		public virtual void postUpdate(int x, int y, int z, World world) {
			world.removeUpdate(x, y, z);
		}
	}
}
