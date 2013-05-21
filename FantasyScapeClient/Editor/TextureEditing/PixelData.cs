using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FantasyScape.Client.Editor {
	class PixelData {
		public int Width;
		public int Height;
		private Color[,] Data;

		public PixelData(int width, int height) {
			// TODO: Complete member initialization
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
			//TODO: The texture saving here
			//throw new NotImplementedException();
		}
	}
}
