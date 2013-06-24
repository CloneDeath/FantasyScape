using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor {
	enum ResourceType {
		All,
		Texture,
		BlockType
	}
	class PackageTree : TreeControl {
		public PackageTree(Base parent) : base(parent) {
			this.RefreshPackageView();
			this.ExpandAll();
		}

		public ResourceType Filter = ResourceType.All;

		public GwenEventHandler TextureOpened;
		public GwenEventHandler BlockTypeOpened;

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
					} else if (child.GetType() == typeof(FSTexture) && (Filter == ResourceType.All || Filter == ResourceType.Texture)) {
                        node.SetImage(@"Data\image.png");
						node.DoubleClicked += OnTexClicked;
                    } else if (child.GetType() == typeof(BlockType) && (Filter == ResourceType.All || Filter == ResourceType.BlockType)) {
                        node.SetImage(@"Data\blocktype.png");
						node.DoubleClicked += OnBlockClicked;
                    }
					node.UserData = child;
					AddChildren(node);
				}
			}
		}

		private void OnTexClicked(Base item) {
			if (TextureOpened != null){
				TextureOpened(item);
			}
		}

		private void OnBlockClicked(Base item) {
			if (BlockTypeOpened != null) {
				BlockTypeOpened(item);
			}
		}

	}
}
