using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using GLImp;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor.BlockTypeEditing {
	class OpenTextureWindow : WindowControl {
		PackageTree PackageView;
		Button OK;
		Button Cancel;

		public delegate void TextureCallback(FSTexture texture);
		TextureCallback Callback;

		public OpenTextureWindow(TextureCallback callback) : base(DevelopmentMenu.Instance) {
			this.Title = "Select Texture";
			this.Resized += new GwenEventHandler(OnResize);
			this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);
			Callback = callback;

			PackageView = new PackageTree(this);
			PackageView.SetPosition(10, 10);
			PackageView.Filter = ResourceType.Texture;
			PackageView.TextureOpened += ItemSelected;

			OK = new Button(this);
			OK.Text = "OK";
			/*OK.Clicked += delegate(Base item) {
				ItemSelected(PackageView
			};*/

			Cancel = new Button(this);
			Cancel.Text = "Cancel";
			Cancel.Clicked += delegate(Base item) {
				this.Close();
			};

			this.SetSize(340, 450);
		}

		private void ItemSelected(Base item) {
			if (Callback != null) {
				Callback(item.UserData as FSTexture);
			}
			this.Close();
		}

		void OnResize(Base control) {
			PackageView.SetSize(this.Width - 40, this.Height - 85);
			OK.SetPosition(this.Width - 120, this.Height - 70);
			Cancel.SetPosition(10, this.Height - 70);
		}
	}
}
