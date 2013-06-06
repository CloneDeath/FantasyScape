using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using System.Drawing;

namespace FantasyScape.Client.Editor.BlockTypeEditing {
    class GLImpTexturePanel : ImagePanel {
        public GLImpTexturePanel(Base parent) : base(parent) {
        
        }

        private GLImp.Texture gltex;
        public GLImp.Texture Texture {
            get {
                return gltex;
            }
            set {
                gltex = value;
                dirty = true;
            }
        }

        private bool dirty = true;
        private Gwen.Texture tex;

        protected override void Render(Gwen.Skin.Base skin) {
            if (dirty) {
                dirty = false;
                byte[] pixelData = new byte[Width * Height * 4];

                for (int x = 0; x < Width; x++) {
                    for (int y = 0; y < Height; y++) {
                        int XLoc = (int)((((float)x) / Width) * gltex.Width);
                        int YLoc = (int)((((float)y) / Height) * gltex.Height);
                        Color c = gltex.Image.GetPixel(XLoc, YLoc);
                        pixelData[4 * (x + y * Width)] = c.R;
                        pixelData[4 * (x + y * Width) + 1] = c.G;
                        pixelData[4 * (x + y * Width) + 2] = c.B;
                        pixelData[4 * (x + y * Width) + 3] = c.A;
                    }
                }

                tex = new Gwen.Texture(skin.Renderer);
                tex.Width = Width;
                tex.Height = Height;
                tex.LoadRaw(Width, Height, pixelData);
            }
            skin.Renderer.DrawColor = Color.White;
            skin.Renderer.DrawTexturedRect(tex, RenderBounds);
        }
    }
}
