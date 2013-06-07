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
            
            this.SetSize(340, 450);
		}

        void PackageExplorer_Resized(Base control) {
            PackageView.SetSize(this.Width - 40, this.Height - 60);
        }
	}
}