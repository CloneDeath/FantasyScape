using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;
using GLImp;
using FantasyScape.NetworkMessages;
using FantasyScape.Client.Editor.PackageEditing;

namespace FantasyScape.Client.Editor{
	internal class PackageExplorer : WindowControl{
		PackageTree PackageView;
		Base TopBar;
		public PackageExplorer() : base(DevelopmentMenu.Instance) {
			this.Title = "Package Explorer";
			this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);

			/* Tree View */
			PackageView = new PackageTree(this);
			PackageView.SetPosition(10, 10);
			PackageView.TextureOpened += delegate(FSTexture tex) {
				TextureEditor window = new TextureEditor(tex);
				window.Show();
			};
			PackageView.BlockTypeOpened += delegate(BlockType bt) {
				BlockTypeEditor window = new BlockTypeEditor(bt);
				window.Show();
			};
			PackageView.CodeOpened += delegate(CodeFile cf) {
				CodeEditor window = new CodeEditor(cf);
				window.Show();
			};
			
			PackageView.Dock = Gwen.Pos.Fill;

			/* Top Bar */
			TopBar = new Base(this);
			TopBar.Height = 25;
			TopBar.Dock = Gwen.Pos.Top;
			{
				Button AddPackage = new Button(TopBar);
				AddPackage.Text = "Add Package";
				AddPackage.Clicked += new GwenEventHandler<ClickedEventArgs>(AddPackage_Clicked);
			}

			this.SetSize(340, 450);
		}

		void AddPackage_Clicked(Base control, ClickedEventArgs args) {
			Package pkg = new Package(Guid.NewGuid());
			pkg.Name = "New Package";
			Package.AddPackage(pkg);

			new AddPackage(pkg).Send();
		}
	}
}