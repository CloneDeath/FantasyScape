using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;
using GLImp;
using System.Drawing;

namespace FantasyScape.Client.Editor {
	class NewTextureName : WindowControl {
		public NewTextureName(TextureEditor parent) : base(parent.Parent) {
			this.SetSize(210, 100);

			TextBox Entry = new TextBox(this);
			Entry.SetPosition(10, 10);
			Entry.SetSize(180, 30);
			Entry.Text = "Texture #" + Textures.GetAll().Count;

			Button OK = new Button(this);
			OK.SetSize(50, 20);
			OK.SetPosition(130, 45);
			OK.Text = "OK";
			OK.Clicked += delegate(Base sender) {
				CreateNewTexture(Entry.Text);
				parent.RefreshTexturesList();
				parent.Select(Entry.Text);
				this.Close();
			};

			Button Cancel = new Button(this);
			Cancel.SetSize(50, 20);
			Cancel.SetPosition(10, 45);
			Cancel.Text = "Cancel";
			Cancel.Clicked += delegate(Base sender) {
				this.Close();
			};
		}

		private void CreateNewTexture(string Name) {
			Bitmap bmp = new Bitmap(16, 16);
			for (int i = 0; i < 16; i++) {
				for (int j = 0; j < 16; j++) {
					bmp.SetPixel(i, j, Color.White);
				}
			}
			Texture tex = new Texture(bmp, Name);
			Textures.AddTexture(tex);

			NetTexture nt = new NetTexture(tex);
			nt.Send();
		}
	}
}
