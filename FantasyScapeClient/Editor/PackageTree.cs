using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor {
	[Flags]
	enum ResourceType {
		Texture		= 0x01 << 0,
		BlockType	= 0x01 << 1,
		CodeFile	= 0x01 << 2,

		All			= 0xFF,
	}
	class PackageTree : TreeControl {
		public PackageTree(Base parent) : base(parent) {
			this.RefreshPackageView();
			this.ExpandAll();
		}

		public ResourceType Filter = ResourceType.All;

		public GwenEventHandler TextureOpened;
		public GwenEventHandler BlockTypeOpened; 
		public GwenEventHandler CodeOpened;

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
					} else if (child.GetType() == typeof(FSTexture) && ((Filter | ResourceType.Texture) != 0)) {
                        node.SetImage(@"Data\image.png");
						node.DoubleClicked += OnTexClicked;
					} else if (child.GetType() == typeof(BlockType) && ((Filter | ResourceType.BlockType) != 0)) {
                        node.SetImage(@"Data\blocktype.png");
						node.DoubleClicked += OnBlockClicked;
					} else if (child.GetType() == typeof(CodeFile) && ((Filter | ResourceType.CodeFile) != 0)) {
						node.SetImage(@"Data\SharedCode.png");
						node.DoubleClicked += OnCodeClicked;
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

		private void OnCodeClicked(Base item) {
			if (CodeOpened != null) {
				CodeOpened(item);
			}
		}

	}
}
