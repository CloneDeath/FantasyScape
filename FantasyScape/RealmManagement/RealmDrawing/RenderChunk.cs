using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;

namespace FantasyScape.RealmManagement.RealmDrawing {
	class RenderChunk {
		DisplayList ChunkDisplay;
		Realm Realm;
		Vector3i ChunkCoord;

		public static readonly Vector3i Size = new Vector3i(16, 16, 16);
		public bool Dirty;

		public RenderChunk(Realm Realm, Vector3i ChunkCoords) {
			this.Realm = Realm;
			this.ChunkDisplay = new DisplayList();
			this.ChunkCoord = ChunkCoords;
		}

		internal void Draw() {
			if (Dirty) {
				Dirty = false;
				ChunkDisplay.PassiveCapture();
				{
					for (int x = 0; x < Size.X; x++) {
						for (int y = 0; y < Size.Y; y++) {
							for (int z = 0; z < Size.Z; z++) {
								Vector3i BlockCoord = (ChunkCoord * Size) + new Vector3i(x, y, z);
								Block BlockValue = Realm.GetBlock(BlockCoord);

								if (BlockValue != null) {
									DrawBlock(BlockValue, BlockCoord);
								}
							}
						}
					}
				}
				ChunkDisplay.EndCapture();
			} else {
				ChunkDisplay.Draw();
			}
		}

		private void DrawBlock(Block BlockValue, Vector3i BlockCoord) {
			//Don't draw it if the level is too low and it's a liquid.
			if (BlockValue.Level <= 0 && BlockValue.BlockType.Liquid) {
				return;
			}

			Block Adjacent;
			Vector2d[] TexCoord = new Vector2d[]{
				new Vector2d(0.0, 1 - ((BlockValue.Level) / 16.0)),
				new Vector2d(1.0, 1 - ((BlockValue.Level) / 16.0)),
				new Vector2d(1.0, 1.0),
				new Vector2d(0.0, 1.0),
			};

			double Top = BlockCoord.Z + ((BlockValue.Level) / 16.0);
			double Bottom = BlockCoord.Z;
			double Back = BlockCoord.Y;
			double Front = BlockCoord.Y + 1.0;
			double Left = BlockCoord.X;
			double Right = BlockCoord.X + 1.0;

			GraphicsManager.SetColor(Color.White);

			//Back
			Adjacent = Realm.GetBlock(BlockCoord + new Vector3i(0, -1, 0));
			if (Adjacent == null || Adjacent.BlockType.Transparent) {
				GL.BindTexture(TextureTarget.Texture2D, BlockValue.BlockType.GetTexture(Blocks.BlockSide.Back).ID);
				GL.Begin(BeginMode.Quads);
				{
					GL.TexCoord2(TexCoord[0]); GL.Vertex3(Left,	 Back, Top);
					GL.TexCoord2(TexCoord[1]); GL.Vertex3(Right, Back, Top);
					GL.TexCoord2(TexCoord[2]); GL.Vertex3(Right, Back, Bottom);
					GL.TexCoord2(TexCoord[3]); GL.Vertex3(Left,  Back, Bottom);
				}
				GL.End();
			}
			

			//Front
			Adjacent = Realm.GetBlock(BlockCoord + new Vector3i(0, 1, 0));
			if (Adjacent == null || Adjacent.BlockType.Transparent) {
				GL.BindTexture(TextureTarget.Texture2D, BlockValue.BlockType.GetTexture(Blocks.BlockSide.Front).ID);
				GL.Begin(BeginMode.Quads);
				{
					GL.TexCoord2(TexCoord[0]); GL.Vertex3(Right, Front, Top);
					GL.TexCoord2(TexCoord[1]); GL.Vertex3(Left,  Front, Top);
					GL.TexCoord2(TexCoord[2]); GL.Vertex3(Left,  Front, Bottom);
					GL.TexCoord2(TexCoord[3]); GL.Vertex3(Right, Front, Bottom);
				}
				GL.End();
			}
			
			//Left
			Adjacent = Realm.GetBlock(BlockCoord + new Vector3i(-1, 0, 0));
			if (Adjacent == null || Adjacent.BlockType.Transparent) {
				GL.BindTexture(TextureTarget.Texture2D, BlockValue.BlockType.GetTexture(Blocks.BlockSide.Left).ID);
				GL.Begin(BeginMode.Quads);
				{
					GL.TexCoord2(TexCoord[0]); GL.Vertex3(Left, Front, Top);
					GL.TexCoord2(TexCoord[1]); GL.Vertex3(Left, Back,  Top);
					GL.TexCoord2(TexCoord[2]); GL.Vertex3(Left, Back,  Bottom);
					GL.TexCoord2(TexCoord[3]); GL.Vertex3(Left, Front, Bottom);
				}
				GL.End();
			}

			//Right
			Adjacent = Realm.GetBlock(BlockCoord + new Vector3i(1, 0, 0));
			if (Adjacent == null || Adjacent.BlockType.Transparent) {
				GL.BindTexture(TextureTarget.Texture2D, BlockValue.BlockType.GetTexture(Blocks.BlockSide.Right).ID);
				GL.Begin(BeginMode.Quads);
				{
					GL.TexCoord2(TexCoord[0]); GL.Vertex3(Right, Back,  Top);
					GL.TexCoord2(TexCoord[1]); GL.Vertex3(Right, Front, Top);
					GL.TexCoord2(TexCoord[2]); GL.Vertex3(Right, Front, Bottom);
					GL.TexCoord2(TexCoord[3]); GL.Vertex3(Right, Back,  Bottom);
				}
				GL.End();
			}

			//Bottom
			Adjacent = Realm.GetBlock(BlockCoord + new Vector3i(0, 0, -1));
			if (Adjacent == null || Adjacent.BlockType.Transparent) {
				GL.BindTexture(TextureTarget.Texture2D, BlockValue.BlockType.GetTexture(Blocks.BlockSide.Bottom).ID);
				GL.Begin(BeginMode.Quads);
				{
					GL.TexCoord2(TexCoord[0]); GL.Vertex3(Left,  Back,  Bottom);
					GL.TexCoord2(TexCoord[1]); GL.Vertex3(Left,  Front, Bottom);
					GL.TexCoord2(TexCoord[2]); GL.Vertex3(Right, Front, Bottom);
					GL.TexCoord2(TexCoord[3]); GL.Vertex3(Right, Back,  Bottom);
				}
				GL.End();
			}

			//Top
			Adjacent = Realm.GetBlock(BlockCoord + new Vector3i(0, 0, 1));
			if (Adjacent == null || Adjacent.BlockType.Transparent) {
				GL.BindTexture(TextureTarget.Texture2D, BlockValue.BlockType.GetTexture(Blocks.BlockSide.Top).ID);
				GL.Begin(BeginMode.Quads);
				{
					GL.TexCoord2(TexCoord[0]); GL.Vertex3(Left,  Back,  Top);
					GL.TexCoord2(TexCoord[1]); GL.Vertex3(Left,  Front, Top);
					GL.TexCoord2(TexCoord[2]); GL.Vertex3(Right, Front, Top);
					GL.TexCoord2(TexCoord[3]); GL.Vertex3(Right, Back,  Top);
				}
				GL.End();
			}
		}
	}
}
