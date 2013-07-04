using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;
using GLImp;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client.Editor.PackageEditing {
	class PackageEditor : WindowControl {
		private Package Resource;

		public PackageEditor(Package package) : base(DevelopmentMenu.Instance){
			this.Resource = package;
			this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);

			Label lblName = new Label(this);
			lblName.AutoSizeToContents = true;
			lblName.Text = "Name";
			lblName.SetPosition(10, 10);

			TextBox tbName = new TextBox(this);
			tbName.SetPosition(50, 10);
			tbName.SetSize(150, 20);
			tbName.Text = Resource.Name;
			tbName.TextChanged += tbName_TextChanged;

			this.SetSize(220, 80);
		}

		void tbName_TextChanged(Base control, EventArgs args) {
			TextBox tbName = control as TextBox;
			Resource.Name = tbName.Text;

			new UpdatePackage(Resource).Send();
			Package.TriggerOnChangeEvent();
		}
	}
}
