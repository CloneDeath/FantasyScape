using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;
using GLImp;
using FantasyScape.NetworkMessages;
using FantasyScape.Client.Editor.PackageViewing;
using System.Drawing;
using FantasyScape.NetworkMessages.Code;

namespace FantasyScape.Client.Editor {
	[Flags]
	enum ResourceType {
		Texture		= 0x01 << 0,
		BlockType	= 0x01 << 1,
		CodeFile	= 0x01 << 2,
		Package		= 0x01 << 3,

		All			= 0xFF,
	}
	class PackageTree : TreeControl {
		public PackageTree(Base parent) : base(parent) {
			this.RefreshPackageView();
			this.ExpandAll();

			Package.OnChange += delegate(object Sender, Resource res){
				RefreshPackageView();
			};
		}

		public ResourceType Filter = ResourceType.All;

		public Action<FSTexture> TextureOpened;
		public Action<BlockType> BlockTypeOpened;
		public Action<CodeFile> CodeOpened;
		public Action<Package> PackageOpened;

		private void RefreshPackageView() {
			this.RemoveAll();
			foreach (Package pack in Package.GetPackages()) {
				ResourceNode tn = new ResourceNode(this, pack);
                tn.SetImage(@"Data\package.png");

				if ((Filter & ResourceType.Package) != 0) {
					tn.DoubleClicked += OnPackageClicked;
				}
				tn.RightClicked += OnResourceRightClicked;
				AddChildren(tn);
			}
			this.ExpandAll();
		}

		private void AddChildren(ResourceNode tn) {
			Folder folder = tn.Resource as Folder;
			if (folder != null) {
				foreach (Resource child in folder.GetChildren()) {
					ResourceNode node = new ResourceNode(tn, child);

                    if (child.GetType() == typeof(Folder)) {
                        node.SetImage(@"Data\folder.png");
					} else if (child.GetType() == typeof(FSTexture) && ((Filter & ResourceType.Texture) != 0)) {
						node.SetImage(@"Data\texture.png");
						node.DoubleClicked += OnTexClicked;
					} else if (child.GetType() == typeof(BlockType) && ((Filter & ResourceType.BlockType) != 0)) {
                        node.SetImage(@"Data\blocktype.png");
						node.DoubleClicked += OnBlockClicked;
					} else if (child.GetType() == typeof(CodeFile) && ((Filter & ResourceType.CodeFile) != 0)) {
						node.SetImage(@"Data\SharedCode.png");
						node.DoubleClicked += OnCodeClicked;
					}
					node.RightClicked += OnResourceRightClicked;

					AddChildren(node);
				}
			}
		}

		private void OnTexClicked(Base item, EventArgs args) {
			if (TextureOpened != null){
				TextureOpened(((ResourceNode)item).Resource as FSTexture);
			}
		}

		private void OnBlockClicked(Base item, EventArgs args) {
			if (BlockTypeOpened != null) {
				BlockTypeOpened(((ResourceNode)item).Resource as BlockType);
			}
		}

		private void OnCodeClicked(Base item, EventArgs args) {
			if (CodeOpened != null) {
				CodeOpened(((ResourceNode)item).Resource as CodeFile);
			}
		}

		private void OnPackageClicked(Base item, EventArgs args) {
			if (PackageOpened != null) {
				PackageOpened(((ResourceNode)item).Resource as Package);
			}
		}

		private void OnResourceRightClicked(Base item, ClickedEventArgs args) {
			Resource ClickedResource = (Resource)(((ResourceNode)item).Resource);

			Menu RightClickMenu = new Menu(Parent);
			{
				if (ClickedResource is Folder) {
					Folder FolderResource = ClickedResource as Folder;
					MenuItem Add = RightClickMenu.AddItem("Add");
					{
						MenuItem submenu = Add.Menu.AddItem("Folder");
						{
							submenu.SetImage(@"Data\folder.png");
							submenu.Clicked += delegate(Base sender, ClickedEventArgs args2) {
								Folder res = new Folder();
								res.Name = "New Folder";
								FolderResource.Add(res);
								new AddFolder(res, ClickedResource.ID).Send();
							};
						}

						submenu = Add.Menu.AddItem("Texture");
						{
							submenu.SetImage(@"Data\texture.png");
							submenu.Clicked += delegate(Base sender, ClickedEventArgs args2) {
								FSTexture tex = new FSTexture();
								tex.Name = "NewTexture.png";
								tex.Load(new Bitmap(16, 16));
								FolderResource.Add(tex);
								new AddTexture(tex, ClickedResource.ID).Send();
							};
						}

						submenu = Add.Menu.AddItem("Block Type");
						{
							submenu.SetImage(@"Data\blocktype.png");
							submenu.Clicked += delegate(Base sender, ClickedEventArgs args2) {
								BlockType bt = new BlockType();
								bt.Name = "NewTexture.png";
								FolderResource.Add(bt);
								new AddBlockType(bt, ClickedResource.ID).Send();
							};
						}

						submenu = Add.Menu.AddItem("Code File");
						{
							submenu.SetImage(@"Data\SharedCode.png");
							submenu.Clicked += delegate(Base sender, ClickedEventArgs args2) {
								CodeFile cf = new CodeFile();
								cf.Name = "NewClass.cs";
								FolderResource.Add(cf);
								new AddCode(cf, ClickedResource.ID).Send();
							};
						}
					}
				}

				MenuItem Rename = RightClickMenu.AddItem("Rename");
				Rename.Clicked += delegate(Base sender, ClickedEventArgs renameargs) {
					((ResourceNode)item).StartRename();
				};
			}

			Point LocalPos = Parent.CanvasPosToLocal(new Point(args.X, args.Y));
			RightClickMenu.SetPosition(LocalPos.X - 6, LocalPos.Y - 28); //Subtracting bounds of inner panel
			RightClickMenu.Show();
		}
	}
}
