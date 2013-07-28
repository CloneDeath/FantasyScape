using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client.Editor {
	class TopBar : MenuStrip {
		public TopBar(Base parent) : base(parent) {
			this.Dock = Gwen.Pos.Top;
			
			MenuItem Edit = this.AddItem("Package Viewer");
			Edit.Clicked += delegate(Base sender, ClickedEventArgs args) {
				PackageExplorer win = new PackageExplorer();
			};

			MenuItem Config = this.AddItem("Configuration Manager");
			Config.Clicked += delegate(Base sender, ClickedEventArgs args) {
				ConfigurationManager win = new ConfigurationManager();
			};
		}
	}
}
