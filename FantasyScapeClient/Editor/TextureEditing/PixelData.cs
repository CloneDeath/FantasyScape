using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GLImp;
using FantasyScape.NetworkMessages;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor {
	class PixelData {
        private bool _subscribed = false;

		public int Width;
		public int Height;
		private Color[,] Data;
		public Guid ID = Guid.Empty;

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

            FSTexture tex = Package.FindResource(ID) as FSTexture;
			if (tex != null) {
                tex.Texture.Image = bmp;
                new UpdateTexture(tex).Send();
                tex.TriggerUpdateEvent(this);
			}
		}

		internal void Load(FSTexture Tex) {
			this.ID = Tex.ID;
			this.Width = Tex.Texture.Width;
			this.Height = Tex.Texture.Height;
			Data = new Color[Width, Height];

			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					this[x, y] = Tex.Texture.Image.GetPixel(x, y);
				}
			}

            if (!_subscribed){
                _subscribed = true;
                Tex.OnUpdate += FSTextureUpdated;
            }
		}

        private void FSTextureUpdated(object sender, Resource tex) {
            if (sender != this) {
                this.Load(tex as FSTexture);
            }
        }
	}
}
