using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;
using GLImp;

namespace FantasyScape.Client.Editor {
	class BlockTypesEditor : WindowControl {
		ListBox BlockTypesList;

		#region RHS
		Label TypeName;
		ComboBox TopTex;
		ComboBox SideTex;
		ComboBox BotTex;
		CheckBox Liquid;
		#endregion

		public BlockTypesEditor(Base parent)
			: base(parent) {
			this.SetSize(800, 300);

			VerticalSplitter Splitter = new VerticalSplitter(this);
			Splitter.Dock = Gwen.Pos.Fill;

			Base LHS = new Base(Splitter);
			{
				Base AboveSpace = new Base(LHS);
				AboveSpace.Dock = Gwen.Pos.Top;
				{
					Button AddBlock = new Button(AboveSpace);
					AddBlock.Text = "Add";
					AddBlock.Dock = Gwen.Pos.Right;
					AddBlock.SetSize(40, 30);
					AddBlock.Clicked += delegate(Base sender) {
						new NewBlockName(this);
						this.Disable();
					};

					Button RefreshBlocks = new Button(AboveSpace);
					RefreshBlocks.Text = "Refresh";
					RefreshBlocks.Dock = Gwen.Pos.Right;
					RefreshBlocks.SetSize(75, 30);
					RefreshBlocks.Clicked += delegate(Base sender) {
						RefreshBlockTypesList();
					};

					Button RemoveBlock = new Button(AboveSpace);
					RemoveBlock.Text = "Remove";
					RemoveBlock.Dock = Gwen.Pos.Left;
					RemoveBlock.SetSize(75, 30);
					RemoveBlock.Clicked += delegate(Base sender) {
						RemoveSelectedBlock();
					};
				}
				AboveSpace.SizeToChildren();

				BlockTypesList = new ListBox(LHS);
				BlockTypesList.Dock = Gwen.Pos.Fill;
				BlockTypesList.RowSelected += delegate(Base sender) {
					this.Select((string)BlockTypesList.SelectedRow.UserData);
				};

				RefreshBlockTypesList();
			}

			Base RHS = new Base(Splitter);
			{
				Label lblName = new Label(RHS);
				lblName.AutoSizeToContents = true;
				lblName.SetPosition(10, 10);
				lblName.Text = "Name:";

				TypeName = new Label(RHS);
				TypeName.AutoSizeToContents = true;
				TypeName.SetPosition(105, 10);
				TypeName.Text = "---";

				Label TopTexLabel = new Label(RHS);
				TopTexLabel.AutoSizeToContents = true;
				TopTexLabel.SetText("Top Texture:");
				TopTexLabel.SetPosition(10, 40);
				TopTex = new ComboBox(RHS);
				TopTex.SetPosition(105, 40);
				TopTex.ItemSelected += SubmitChanges;

				Label SideTexLabel = new Label(RHS);
				SideTexLabel.AutoSizeToContents = true;
				SideTexLabel.SetText("Side Texture:");
				SideTexLabel.SetPosition(10, 70);
				SideTex = new ComboBox(RHS);
				SideTex.SetPosition(105, 70);
				SideTex.ItemSelected += SubmitChanges;

				Label BotTexLabel = new Label(RHS);
				BotTexLabel.AutoSizeToContents = true;
				BotTexLabel.SetText("Bottom Texture:");
				BotTexLabel.SetPosition(10, 100);
				BotTex = new ComboBox(RHS);
				BotTex.SetPosition(105, 100);
				BotTex.ItemSelected += SubmitChanges;

				Label LiquidLabel = new Label(RHS);
				LiquidLabel.AutoSizeToContents = true;
				LiquidLabel.SetText("Liquid:");
				LiquidLabel.SetPosition(10, 130);
				Liquid = new CheckBox(RHS);
				Liquid.SetPosition(105, 130);
				Liquid.CheckChanged += SubmitChanges;

				RefreshTextureTypesList();
			}

			Splitter.SetPanel(0, LHS);
			Splitter.SetPanel(1, RHS);
			Splitter.SetHValue(.3f);
		}

		bool EnableSubmit = true;
		private void SubmitChanges(Base sender) {
			if (EnableSubmit && BlockTypes.Exists(TypeName.Text)) {
				throw new NotImplementedException();
				//BlockType b = BlockTypes.GetBlockType(TypeName.Text);
				////b.Name = Name;
				//b.TopTexture = (string)TopTex.SelectedItem.UserData;
				//b.SideTexture = (string)SideTex.SelectedItem.UserData;
				//b.BotTexture = (string)BotTex.SelectedItem.UserData;
				//b.Liquid = Liquid.IsChecked;

				//BlockTypeData btd = new BlockTypeData(b);
				//btd.Send();
			}
		}

		private void RemoveSelectedBlock() {
			if (BlockTypesList.SelectedRow == null) return;
			BlockType selected = BlockTypes.GetBlockType((string)BlockTypesList.SelectedRow.UserData);

			if (selected != null) {
				BlockTypes.Remove(selected.Name);

				new RemoveBlockType(selected.ID).Send();
			}
		}

		public void RefreshBlockTypesList(){
			BlockTypesList.Clear();
			foreach (BlockType type in BlockTypes.GetAll()) {
				BlockTypesList.AddRow(type.Name, "", type.Name);
			}
		}

		public void RefreshTextureTypesList() {
			foreach (Texture tex in Textures.GetAll()) {
				TopTex.AddItem(tex.Name, "", tex.Name);
				SideTex.AddItem(tex.Name, "", tex.Name);
				BotTex.AddItem(tex.Name, "", tex.Name);
			}
		}

		internal void Select(string BlockType) {
			throw new NotImplementedException();
			////Make the item is selected in the list
			//if (BlockTypesList.SelectedRow == null || (string)BlockTypesList.SelectedRow.UserData != BlockType) {
			//    BlockTypesList.SelectByUserData(BlockType);
			//}

			////Update RHS info
			//EnableSubmit = false;
			//BlockType bt = BlockTypes.GetBlockType(BlockType);
			//TypeName.Text = bt.Name;
			//TopTex.SelectByUserData(bt.TopTexture);
			//SideTex.SelectByUserData(bt.SideTexture);
			//BotTex.SelectByUserData(bt.BotTexture);
			//Liquid.IsChecked = bt.Liquid;
			//EnableSubmit = true;
		}
	}
}
