using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Client.Editor.BlockTypesEditor;
using FantasyScape.Client.Editor.TextureEditor;

namespace FantasyScape.Client.Editor {
	class EditorWindow : WindowControl {
		public EditorWindow() : base(MainCanvas.GetCanvas()) {
			this.SetPosition(30, 30);
			this.SetSize(200, 300);

			Button NewBlock = new Button(this);
			NewBlock.SetPosition(10, 10);
			NewBlock.SetText("New Block");
			NewBlock.Clicked += delegate(Base sender) {
				new NewBlockWindow();
			};

			Button DelBlock = new Button(this);
			DelBlock.SetPosition(10, 40);
			DelBlock.SetText("Delete Block");
			DelBlock.Clicked += delegate(Base sender) {
				new DeleteBlockWindow();
			};

			Button NewTex = new Button(this);
			NewTex.SetPosition(10, 70);
			NewTex.SetText("New Texture");
			NewTex.Clicked += delegate(Base sender) {
				new NewTextureWindow();
			};

			Button Close = new Button(this);
			Close.SetPosition(10, 100);
			Close.SetText("Close");
			Close.Clicked += delegate(Base sender) {
				MainCanvas.GetCanvas().RemoveChild(this, true);
			};
		}
	}
}
