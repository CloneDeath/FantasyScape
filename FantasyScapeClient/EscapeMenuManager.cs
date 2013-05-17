using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client {
	class EscapeMenuManager {
		WindowControl EscapeWindow;
		WindowControl Editor;
		WindowControl NewBlockWindow;

		public EscapeMenuManager() {
			CreateNewBlockWindow();
			CreateEditorWindow();
			CreateEscapeWindow();
		}

		private bool _Hidden = true;
		public bool Hidden{
			get { return _Hidden; }
			set {
				_Hidden = value;
				if (_Hidden) {
					EscapeWindow.Hide();
					Editor.Hide();
					NewBlockWindow.Hide();
				} else {
					EscapeWindow.Show();
				}
			}

		}

		private void CreateNewBlockWindow() {
			NewBlockWindow = new WindowControl(MainCanvas.GetCanvas());
			NewBlockWindow.SetPosition(40, 40);
			NewBlockWindow.SetSize(400, 400);

			Label NameLabel = new Label(NewBlockWindow);
			NameLabel.SetText("Name:");
			NameLabel.SetPosition(10, 10);
			TextBox Name = new TextBox(NewBlockWindow);
			Name.SetPosition(105, 10);
			Name.SetText("NewBlock");

			Label TopTexLabel = new Label(NewBlockWindow);
			TopTexLabel.SetText("Top Texture:");
			TopTexLabel.SetPosition(10, 40);
			ComboBox TopTex = new ComboBox(NewBlockWindow);
			TopTex.SetPosition(105, 40);
			foreach (BlockType bt in BlockTypes.GetAll()) {
				TopTex.AddItem(bt.Name, "", bt.Name);
			}

			Label SideTexLabel = new Label(NewBlockWindow);
			SideTexLabel.SetText("Side Texture:");
			SideTexLabel.SetPosition(10, 70);
			ComboBox SideTex = new ComboBox(NewBlockWindow);
			SideTex.SetPosition(105, 70);
			foreach (BlockType bt in BlockTypes.GetAll()) {
				SideTex.AddItem(bt.Name, "", bt.Name);
			}

			Label BotTexLabel = new Label(NewBlockWindow);
			BotTexLabel.SetText("Bottom Texture:");
			BotTexLabel.SetPosition(10, 100);
			ComboBox BotTex = new ComboBox(NewBlockWindow);
			BotTex.SetPosition(105, 100);
			foreach (BlockType bt in BlockTypes.GetAll()){
				BotTex.AddItem(bt.Name, "", bt.Name);
			}			

			Label LiquidLabel = new Label(NewBlockWindow);
			LiquidLabel.SetText("Liquid:");
			LiquidLabel.SetPosition(10, 130);
			CheckBox Liquid = new CheckBox(NewBlockWindow);
			Liquid.SetPosition(105, 130);

			Button Create = new Button(NewBlockWindow);
			Create.SetText("Create Block");
			Create.SetPosition(100, 170);
			Create.Clicked += delegate(Base Sender) {
				if (BlockTypes.Exists(Name.Text)) {
					NameLabel.Text = "*Name:";
					return;
				}

				BlockType b = new BlockType();
				b.Name = Name.Text;
				b.TopTexture = (string)TopTex.SelectedItem.UserData;
				b.SideTexture = (string)SideTex.SelectedItem.UserData;
				b.BotTexture = (string)BotTex.SelectedItem.UserData;
				b.Liquid = Liquid.IsChecked;

				BlockTypes.AddBlockType(b);

				BlockTypeData btd = new BlockTypeData(b);
				btd.Send();

				Game.Self.Inventory.Add(new Block(b.Name));

				NewBlockWindow.Hide();
			};

			NewBlockWindow.Hide();
		}

		private void CreateEditorWindow() {
			Editor = new WindowControl(MainCanvas.GetCanvas());
			Editor.SetPosition(30, 30);
			Editor.SetSize(200, 300);

			Button NewBlock = new Button(Editor);
			NewBlock.SetPosition(10, 10);
			NewBlock.SetText("New Block");
			NewBlock.Clicked += delegate(Base sender) {
				NewBlockWindow.Hide();
				CreateNewBlockWindow();
				NewBlockWindow.Show();
			};

			Button Close = new Button(Editor);
			Close.SetPosition(10, 40);
			Close.SetText("Close");
			Close.Clicked += delegate(Base sender) {
				Editor.Hide();
			};

			Editor.Hide();
		}

		private void CreateEscapeWindow() {
			EscapeWindow = new WindowControl(MainCanvas.GetCanvas());
			EscapeWindow.SetPosition(10, 10);
			EscapeWindow.SetSize(200, 200);

			Button Close = new Button(EscapeWindow);
			Close.SetPosition(10, 10);
			Close.SetText("Continue");
			Close.Clicked += delegate(Base sender) {
				EscapeWindow.Hide();
				Game.LockMouse = true;
			};

			Button Edit = new Button(EscapeWindow);
			Edit.SetPosition(10, 40);
			Edit.SetText("Open Editor");
			Edit.Clicked += delegate(Base sender) {
				Editor.Show();
			};

			Button Quit = new Button(EscapeWindow);
			Quit.SetPosition(10, 70);
			Quit.SetText("Quit");
			Quit.Clicked += delegate(Base sender) {
				MainCanvas.Dispose();
				Environment.Exit(0);
			};

			EscapeWindow.Hide();
		}
	}
}
