using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using System.Drawing;
using GLImp;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client.Editor {
	class TextureEditor : WindowControl {
		ListBox TexturesList;
		DrawingArea Canvas;

		public TextureEditor(Base parent) : base(parent) {
			this.SetPosition(10, 10);
			this.SetSize(800, 400);

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
						new NewTextureName(this);
						this.Disable();
					};

					Button RefreshBlocks = new Button(AboveSpace);
					RefreshBlocks.Text = "Refresh";
					RefreshBlocks.Dock = Gwen.Pos.Right;
					RefreshBlocks.SetSize(75, 30);
					RefreshBlocks.Clicked += delegate(Base sender) {
						RefreshTexturesList();
					};

					Button RemoveBlock = new Button(AboveSpace);
					RemoveBlock.Text = "Remove";
					RemoveBlock.Dock = Gwen.Pos.Left;
					RemoveBlock.SetSize(75, 30);
					RemoveBlock.Clicked += delegate(Base sender) {
						RemoveSelectedTexture();
					};
				}
				AboveSpace.SizeToChildren();

				TexturesList = new ListBox(LHS);
				TexturesList.Dock = Gwen.Pos.Fill;
				TexturesList.RowSelected += delegate(Base sender) {
					this.Select((string)TexturesList.SelectedRow.UserData);
				};

				RefreshTexturesList();
			}

			Base RHS = new Base(Splitter);
			{
				Canvas = new DrawingArea(RHS);
				Canvas.SetPosition(10, 10);
				Canvas.SetSize(300, 300);

				HSVColorPicker colorpicker = new HSVColorPicker(RHS);
				colorpicker.SetPosition(320, 10);
				colorpicker.ColorChanged += delegate(Base sender) {
					Canvas.SetColor(colorpicker.SelectedColor);
				};
			}

			Splitter.SetPanel(0, LHS);
			Splitter.SetPanel(1, RHS);
			Splitter.SetHValue(.3f);
		}

		private void RemoveSelectedTexture() {
			if (TexturesList.SelectedRow == null) return;
			Texture selected = Textures.GetTexture((string)TexturesList.SelectedRow.UserData);

			if (selected != null) {
				Textures.Remove(selected.Name);

				TextureRemove btr = new TextureRemove(selected.Name);
				btr.Send();
			}
		}

		internal void RefreshTexturesList() {
			TexturesList.Clear();
			foreach (Texture tex in Textures.GetAll()) {
				TexturesList.AddRow(tex.Name, "", tex.Name);
			}
		}

		internal void Select(string tex) {
			//Make the item is selected in the list
			if ((string)TexturesList.SelectedRow.UserData != tex) {
				TexturesList.SelectByUserData(tex);
			}

			//Update RHS info
			Texture Tex = Textures.GetTexture(tex);
			Canvas.LoadTexture(Tex);
		}
	}
}
