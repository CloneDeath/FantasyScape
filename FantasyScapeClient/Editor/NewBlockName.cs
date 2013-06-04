using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client.Editor {
	class NewBlockName : WindowControl {
		public NewBlockName(BlockTypesEditor parent) : base(parent.Parent){
			this.SetSize(210, 100);

			TextBox Entry = new TextBox(this);
			Entry.SetPosition(10, 10);
			Entry.SetSize(180, 30);
			Entry.Text = "Block #" + BlockTypes.GetAll().Count;

			Button OK = new Button(this);
			OK.SetSize(50, 20);
			OK.SetPosition(130, 45);
			OK.Text = "OK";
			OK.Clicked += delegate(Base sender) {
				CreateNewBlock(Entry.Text);
				parent.RefreshBlockTypesList();
				parent.Select(Entry.Text);
				this.Close();
			};

			Button Cancel = new Button(this);
			Cancel.SetSize(50, 20);
			Cancel.SetPosition(10, 45);
			Cancel.Text = "Cancel";
			Cancel.Clicked += delegate(Base sender) {
				this.Close();
			};
		}

		private void CreateNewBlock(string Name) {
			throw new NotImplementedException();
			//BlockType b = new BlockType();
			//b.Name = Name;
			//b.TopTexture = "Dirt";
			//b.SideTexture = "Dirt";
			//b.BotTexture = "Dirt";
			//b.Liquid = false;

			//BlockTypes.Add(b);

			//BlockTypeData btd = new BlockTypeData(b);
			//btd.Send();

			//Game.Self.Inventory.Add(new Block(b.Name));
		}
	}
}
