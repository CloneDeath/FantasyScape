using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;
using GLImp;
using FantasyScape.Client.Editor.BlockTypeEditing;
using FantasyScape.Blocks;

namespace FantasyScape.Client.Editor {
	class BlockTypeEditor : WindowControl {
        BlockType Resource;
        TextureRefBox[] panels = new TextureRefBox[(int)BlockSide.Count];
        LabeledCheckBox Liquid;

		public BlockTypeEditor(BlockType resource) : base(DevelopmentMenu.Instance) {
            this.Resource = resource;

            this.Title = "Block Type Editor";
            this.SetSize(375, 425);
			this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);

            for (int i = 0; i < (int)BlockSide.Count; i++) {
                panels[i] = new TextureRefBox(this, Resource.Texture[i]);
                panels[i].SetPosition(10 + ((i%2) * 170), 10 + ((i/2) * 85));
                panels[i].Text = ((BlockSide)i).ToString();
				panels[i].TextureChanged += SubmitChanges;
            }

            Liquid = new LabeledCheckBox(this);
            Liquid.Text = "Liquid: ";
            Liquid.SetPosition(10, 350);
			Liquid.CheckChanged += SubmitChanges;

		}

		private void SubmitChanges(Base sender, EventArgs args) {
			for (int i = 0; i < (int)BlockSide.Count; i++) {
				Resource.Texture[i] = panels[i].Texture;
			}

			Resource.Liquid = Liquid.IsChecked;

			Resource.SendUpdate();
		}
	}
}
