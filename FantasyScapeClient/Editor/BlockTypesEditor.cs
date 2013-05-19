using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client.Editor {
	class BlockTypesEditor : WindowControl {
		ListBox BlockTypesList;

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
				RefreshBlockTypesList();
			}

			Base RHS = new Base(Splitter);
			{

			}

			Splitter.SetPanel(0, LHS);
			Splitter.SetPanel(1, RHS);
			Splitter.SetHValue(.3f);
		}

		private void RemoveSelectedBlock() {
			if (BlockTypesList.SelectedRow == null) return;
			BlockType selected = BlockTypes.GetBlockType((string)BlockTypesList.SelectedRow.UserData);

			if (selected != null) {
				BlockTypes.Remove(selected.Name);

				BlockTypeRemove btr = new BlockTypeRemove(selected.Name);
				btr.Send();
			}
		}

		public void RefreshBlockTypesList(){
			BlockTypesList.Clear();
			foreach (BlockType type in BlockTypes.GetAll()) {
				BlockTypesList.AddRow(type.Name, "", type.Name);
			}
		}
	}
}
