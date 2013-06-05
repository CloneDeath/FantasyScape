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
				MenuItem PackageView = Edit.Menu.AddItem("Package Viewer");
				PackageView.Clicked += delegate(Base sender) {
					PackageExplorer win = new PackageExplorer(this.Parent);
				};

				MenuItem BlockEditor = Edit.Menu.AddItem("Block Types");
				BlockEditor.Clicked += delegate(Base sender) {
					WindowControl win = new BlockTypesEditor(this.Parent);
					win.SetPosition(0, 22);
				};

				MenuItem CodeEdit = Edit.Menu.AddItem("Code Editor");
				CodeEdit.Clicked += delegate(Base sender) {
					CodeEditor win = new CodeEditor(this.Parent);
					win.SetPosition(0, 22);
				};
			}
		}
	}
}
