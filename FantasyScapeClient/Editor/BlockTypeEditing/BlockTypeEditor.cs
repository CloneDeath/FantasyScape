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

		TextBox TypeName;
        TextureRefBox[] panels = new TextureRefBox[(int)BlockSide.Count];
        LabeledCheckBox Liquid;

		public BlockTypeEditor(BlockType resource) : base(DevelopmentMenu.Instance) {
            this.Resource = resource;

            this.Title = "Block Type Editor";
            this.SetSize(375, 450);

			Label lblName = new Label(this);
			lblName.AutoSizeToContents = true;
			lblName.SetPosition(10, 10);
			lblName.Text = "Name:";

            TypeName = new TextBox(this);
            TypeName.Width = 200;
			TypeName.SetPosition(55, 10);

            for (int i = 0; i < (int)BlockSide.Count; i++) {
                panels[i] = new TextureRefBox(this, Resource.Texture[i]);
                panels[i].SetPosition(10 + ((i%2) * 170), 35 + ((i/2) * 85));
                panels[i].Text = ((BlockSide)i).ToString();
            }

            Liquid = new LabeledCheckBox(this);
            Liquid.Text = "Liquid: ";
            Liquid.SetPosition(10, 375);
			Liquid.CheckChanged += SubmitChanges;

		}

		bool EnableSubmit = true;
		private void SubmitChanges(Base sender) {
			throw new NotImplementedException();
			//if (EnableSubmit && BlockTypes.Exists(TypeName.Text)) {
			//    throw new NotImplementedException();
			//    //BlockType b = BlockTypes.GetBlockType(TypeName.Text);
			//    ////b.Name = Name;
			//    //b.TopTexture = (string)TopTex.SelectedItem.UserData;
			//    //b.SideTexture = (string)SideTex.SelectedItem.UserData;
			//    //b.BotTexture = (string)BotTex.SelectedItem.UserData;
			//    //b.Liquid = Liquid.IsChecked;

			//    //BlockTypeData btd = new BlockTypeData(b);
			//    //btd.Send();
			//}
		}
	}
}
