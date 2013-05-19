using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client.Editor {
	class NewBlockWindow : WindowControl {
		public NewBlockWindow() : base(MainCanvas.GetCanvas()) {
			this.SetPosition(40, 40);
			this.SetSize(400, 400);

			Label NameLabel = new Label(this);
			NameLabel.SetText("Name:");
			NameLabel.SetPosition(10, 10);
			TextBox Name = new TextBox(this);
			Name.SetPosition(105, 10);
			Name.SetText("NewBlock");

			Label TopTexLabel = new Label(this);
			TopTexLabel.SetText("Top Texture:");
			TopTexLabel.SetPosition(10, 40);
			ComboBox TopTex = new ComboBox(this);
			TopTex.SetPosition(105, 40);
			foreach (BlockType bt in BlockTypes.GetAll()) {
				TopTex.AddItem(bt.Name, "", bt.Name);
			}

			Label SideTexLabel = new Label(this);
			SideTexLabel.SetText("Side Texture:");
			SideTexLabel.SetPosition(10, 70);
			ComboBox SideTex = new ComboBox(this);
			SideTex.SetPosition(105, 70);
			foreach (BlockType bt in BlockTypes.GetAll()) {
				SideTex.AddItem(bt.Name, "", bt.Name);
			}

			Label BotTexLabel = new Label(this);
			BotTexLabel.SetText("Bottom Texture:");
			BotTexLabel.SetPosition(10, 100);
			ComboBox BotTex = new ComboBox(this);
			BotTex.SetPosition(105, 100);
			foreach (BlockType bt in BlockTypes.GetAll()) {
				BotTex.AddItem(bt.Name, "", bt.Name);
			}

			Label LiquidLabel = new Label(this);
			LiquidLabel.SetText("Liquid:");
			LiquidLabel.SetPosition(10, 130);
			CheckBox Liquid = new CheckBox(this);
			Liquid.SetPosition(105, 130);

			Button Create = new Button(this);
			Create.SetText("Create Block");
			Create.SetPosition(100, 170);
			Create.Clicked += delegate(Base Sender) {
				//if (BlockTypes.Exists(Name.Text)) {
				//    NameLabel.Text = "*Name:";
				//    return;
				//}

				BlockType b = new BlockType();
				b.Name = Name.Text;
				b.TopTexture = (string)TopTex.SelectedItem.UserData;
				b.SideTexture = (string)SideTex.SelectedItem.UserData;
				b.BotTexture = (string)BotTex.SelectedItem.UserData;
				b.Liquid = Liquid.IsChecked;

				BlockTypes.Add(b);

				BlockTypeData btd = new BlockTypeData(b);
				btd.Send();

				Game.Self.Inventory.Add(new Block(b.Name));

				this.Hide();
			};
		}
	}
}
