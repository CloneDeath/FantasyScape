using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using System.Drawing;

namespace FantasyScape.Client.Editor {
	class ConfigurationManager : WindowControl {
		ListBox Configs;

		public ConfigurationManager() : base(DevelopmentMenu.Instance) {
			this.Title = "Configuration Manager";
			this.SetSize(400, 200);

			Configs = new ListBox(this);
			Configs.Dock = Gwen.Pos.Fill;
			RefreshUI();
		}

		public void RefreshUI() {
			Object prevselconfig = null;
			if (Configs.SelectedRow != null) {
				prevselconfig = Configs.SelectedRow.UserData;
			}

			Configs.Clear();
			foreach (Configuration config in Game.ServerInfo.Configurations) {
				ListBoxRow row = Configs.AddRow(config.Name, "", config);
				if (Game.ServerInfo.CurrentConfiguration == config) {
					row.Text += " (Active)";
				}

				row.RightClicked += OnConfigurationRightClicked;
			}
			if (prevselconfig != null) {
				Configs.SelectByUserData(prevselconfig);
			}
		}

		private void OnConfigurationRightClicked(Base item, ClickedEventArgs args) {
			Configuration ClickedConfig = (Configuration)item.UserData;

			Menu RightClickMenu = new Menu(Parent);
			{
				MenuItem SetActive = RightClickMenu.AddItem("Set as Active Configuration");
				{
					SetActive.Clicked += delegate(Base sender, ClickedEventArgs args2) {
						Game.ServerInfo.CurrentConfiguration = ClickedConfig;
					};
				}
			}

			Point LocalPos = Parent.CanvasPosToLocal(new Point(args.X, args.Y));
			RightClickMenu.SetPosition(LocalPos.X - 6, LocalPos.Y - 28); //Subtracting bounds of inner panel
			RightClickMenu.Show();
		}


	}
}
