using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor {
	class PackageTree : TreeControl {
		public PackageTree(Base parent) : base(parent) {
			this.RefreshPackageView();
		}

		private void RefreshPackageView() {
			foreach (Package pack in Package.Packages.Values) {
				TreeNode tn = this.AddNode(pack.Name);
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
                            TextureEditor window = new TextureEditor(sender.UserData as FSTexture);
                            window.Show();
                        };
                    } else if (child.GetType() == typeof(BlockType)) {
                        node.SetImage(@"Data\blocktype.png");
                        node.DoubleClicked += delegate(Base sender) {
                            BlockTypeEditor window = new BlockTypeEditor(sender.UserData as BlockType);
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
