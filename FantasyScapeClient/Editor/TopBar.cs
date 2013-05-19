using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client.Editor {
	class TopBar : MenuStrip {
		public TopBar(Base parent) : base(parent) {
			this.Dock = Gwen.Pos.Top;
			MenuItem Edit = this.AddItem("Editor");
			{
				MenuItem BlockEditor = Edit.Menu.AddItem("Block Types");
				BlockEditor.Clicked += delegate(Base sender) {
					WindowControl win = new BlockTypesEditor(this.Parent);
					win.SetPosition(0, 22);
				};

				MenuItem TextureEditor = Edit.Menu.AddItem("Textures");
				TextureEditor.Clicked += delegate(Base sender) {
					WindowControl win = new TextureEditor(this.Parent);
					win.SetPosition(0, 22);
				};
			}
		}
	}
}
