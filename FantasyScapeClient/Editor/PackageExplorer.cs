using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;
using GLImp;

namespace FantasyScape.Client.Editor{
	internal class PackageExplorer : WindowControl{
		PackageTree PackageView;
		public PackageExplorer() : base(DevelopmentMenu.Instance){
            this.Title = "Package Explorer";
            this.Resized += new GwenEventHandler(PackageExplorer_Resized);
            this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);

			PackageView = new PackageTree(this);
			PackageView.SetPosition(10, 10);
			PackageView.TextureOpened += delegate(Base sender) {
				TextureEditor window = new TextureEditor(sender.UserData as FSTexture);
				window.Show();
			};
			PackageView.BlockTypeOpened += delegate(Base sender) {
				BlockTypeEditor window = new BlockTypeEditor(sender.UserData as BlockType);
				window.Show();
			};
			PackageView.CodeOpened += delegate(Base sender) {
				CodeEditor window = new CodeEditor(sender.UserData as CodeFile);
				window.Show();
			};
            
            this.SetSize(340, 450);
		}

        void PackageExplorer_Resized(Base control) {
            PackageView.SetSize(this.Width - 40, this.Height - 60);
        }
	}
}