using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;
using GLImp;

namespace FantasyScape.Client.Editor{
	internal class PackageExplorer : WindowControl{
		TreeControl PackageView;
		public PackageExplorer(Base parent) : base(parent){
            this.Title = "Package Explorer";
            this.Resized += new GwenEventHandler(PackageExplorer_Resized);
            this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);

			PackageView = new TreeControl(this);
			PackageView.SetPosition(10, 10);
			RefreshPackageView();

            
            this.SetSize(340, 450);
		}

        void PackageExplorer_Resized(Base control) {
            PackageView.SetSize(this.Width - 40, this.Height - 60);
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
                        node.DoubleClicked += delegate(Base sender) {
                            TextureEditor window = new TextureEditor(this.Parent, sender.UserData as FSTexture);
                            window.Show();
                        };
                    } else if (child.GetType() == typeof(BlockType)) {
                        node.SetImage(@"Data\blocktype.png");
                        node.DoubleClicked += delegate(Base sender) {
                            BlockTypeEditor window = new BlockTypeEditor(this.Parent, sender.UserData as BlockType);
                            window.Show();
                        };
                    }
					node.UserData = child;
					AddChildren(node);
				}
			}
		}
	}
}