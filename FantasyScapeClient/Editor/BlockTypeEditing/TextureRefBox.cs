using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Blocks;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor.BlockTypeEditing {
    class TextureRefBox : GroupBox {
        public FSTextureReference Texture;
        Label TextureName;
        LabeledCheckBox Defined;
        GLImpTexturePanel panel;
		public GwenEventHandler<EventArgs> TextureChanged;

        public TextureRefBox(Base parent, FSTextureReference Texture) : base(parent) {
            this.Texture = Texture;
            this.SetSize(160, 75);

            TextureName = new Label(this);
            TextureName.AutoSizeToContents = true;
            TextureName.SetPosition(10, 0);

            Defined = new LabeledCheckBox(this);
            Defined.Text = "Defined";
            Defined.SetPosition(10, 20);
			Defined.CheckChanged += new GwenEventHandler<EventArgs>(Defined_CheckChanged);

            panel = new GLImpTexturePanel(this);
            panel.SetSize(50, 50);
            panel.SetPosition(85, 0);
			panel.Clicked += delegate(Base sender, ClickedEventArgs args) {
				OpenTextureWindow otw = new OpenTextureWindow(SetTexture);
				otw.Show();
            };

            RefreshAll();
        }

        private void RefreshAll() {
            if (Texture.Defined) {
                TextureName.Text = Texture.Texture.Name;
            } else {
                TextureName.Text = "[None]";
            }
            Defined.IsChecked = Texture.Defined;
            panel.Texture = Texture.Texture.Texture; //lol, I need better names
        }

		private void SetTexture(FSTexture tex) {
			Texture.Texture = tex;
			if (tex.ID != Guid.Empty) {
				Texture.Defined = true;
			}
			RefreshAll();
			Game.Renderer.DirtyAll();
			if (TextureChanged != null) {
				TextureChanged(this, EventArgs.Empty);
			}
		}

		void Defined_CheckChanged(Base sender, EventArgs arguments) {
			Texture.Defined = Defined.IsChecked;
			RefreshAll();
			Game.Renderer.DirtyAll();
			if (TextureChanged != null) {
				TextureChanged(this, EventArgs.Empty);
			}
		}
    }
}
