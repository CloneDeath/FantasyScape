using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor{
	internal class PackageExplorer : WindowControl{
		TreeControl PackageView;
		public PackageExplorer(Base parent) : base(parent){
			PackageView = new TreeControl(this);
			PackageView.SetPosition(10, 10);
			PackageView.SetSize(300, 400);
			RefreshPackageView();
			this.SetSize(340, 450);
		}

		private void RefreshPackageView() {
			foreach (Package pack in Package.Packages.Values) {
				TreeNode tn = PackageView.AddNode(pack.Name);
                tn.SetImage(@"Data\package.png");
				tn.UserData = pack;
				AddChildren(tn);
			}
		}

		private void AddChildren(TreeNode tn) {
			Folder folder = tn.UserData as Folder;
			if (folder != null) {
				foreach (Resource child in folder.Children) {
					TreeNode node = tn.AddNode(child.Name);
                    if (child.GetType() == typeof(Folder)) {
                        node.SetImage(@"Data\folder.png");
                    } else if (child.GetType() == typeof(FSTexture)) {
                        node.SetImage(@"Data\image.png");
                    }
					node.UserData = child;
					AddChildren(node);
				}
			}
		}
	}
}