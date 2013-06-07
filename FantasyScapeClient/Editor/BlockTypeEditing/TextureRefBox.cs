using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Blocks;

namespace FantasyScape.Client.Editor.BlockTypeEditing {
    class TextureRefBox : GroupBox {
        FSTextureReference Texture;
        Label TextureName;
        LabeledCheckBox Defined;
        GLImpTexturePanel panel;

        public TextureRefBox(Base parent, FSTextureReference Texture) : base(parent) {
            this.Texture = Texture;
            this.SetSize(160, 75);

            TextureName = new Label(this);
            TextureName.AutoSizeToContents = true;
            TextureName.SetPosition(10, 0);

            Defined = new LabeledCheckBox(this);
            Defined.Text = "Defined";
            Defined.SetPosition(10, 20);

            panel = new GLImpTexturePanel(this);
            panel.SetSize(50, 50);
            panel.SetPosition(85, 0);
            panel.Clicked += delegate(Base sender) {
                PackageExplorer pak = new PackageExplorer();
                pak.Show();
            };

            RefreshTexture();
        }

        private void RefreshTexture() {
            if (Texture.Defined) {
                TextureName.Text = Texture.Texture.Name;
            } else {
                TextureName.Text = "[None]";
            }
            Defined.IsChecked = Texture.Defined;
            panel.Texture = Texture.Texture.Texture; //lol, I need better names
        }
    }
}
