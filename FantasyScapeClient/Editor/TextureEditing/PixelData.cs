using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GLImp;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client.Editor {
	class PixelData {
		public int Width;
		public int Height;
		private Color[,] Data;
		public string Name = null;

		public PixelData(int width, int height) {
			this.Width = width;
			this.Height = height;
			Data = new Color[width, height];
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Data[x, y] = Color.White;
				}
			}
		}

		public Color this[int x, int y] {
			get {
				if (x < Width && x >= 0 && y < Height && y >= 0) {
					return Data[x, y];
				} else {
					return Color.White;
				}
			}
			set {
				if (x < Width && x >= 0 && y < Height && y >= 0) {
					Data[x, y] = value;
				}
			}
		}

		internal void Update() {
			Bitmap bmp = new Bitmap(Width, Height);
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					bmp.SetPixel(x, y, this[x, y]);
				}
			}

			if (Textures.Exists(Name)) {
				throw new NotImplementedException();
				//Texture tex = Textures.GetTexture(Name);
				//tex.Image = bmp;

				//NetTexture nt = new NetTexture(tex);
				//nt.Send();
			}
		}

		internal void Load(Texture Tex) {
			this.Name = Tex.Name;
			this.Width = Tex.Width;
			this.Height = Tex.Height;
			Data = new Color[Width, Height];

			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					this[x, y] = Tex.Image.GetPixel(x, y);
				}
			}
		}
	}
}
