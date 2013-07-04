using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor.PackageViewing {
	class ResourceNode : TreeNode {
		TextBox tbRename;
		public Resource Resource;

		public ResourceNode(Base parent, Resource res) : base(parent) {
			this.Resource = res;

			this.Text = res.Name;
			if (Game.ServerInfo.StartupPackage == res.ID) {
				this.Text += " (Startup)";
				this.m_Title.Font = new Gwen.Font(MainCanvas.Renderer, "Arial Black");
			} else {
				this.m_Title.Font = new Gwen.Font(MainCanvas.Renderer, "Arial");
			}
			tbRename = new TextBox(m_Title);
			tbRename.SetPosition(16, 0);
			tbRename.Height = 16;
			tbRename.AutoSizeToContents = true;
			tbRename.Hide();

			tbRename.BoundsChanged += new GwenEventHandler<EventArgs>(tbRename_BoundsChanged);

			tbRename.SubmitPressed += delegate(Base sender, EventArgs args) { EndRename(); };
			//this.Unselected += delegate(Base sender, EventArgs args) { EndRename(); };			
		}

		void tbRename_BoundsChanged(Base sender, EventArgs arguments) {
			tbRename.Height = 16;
		}

		public void StartRename() {
			tbRename.SetText(Resource.Name);
			tbRename.Show();
			tbRename.Focus();
			tbRename.CursorPos = 0;
			tbRename.CursorEnd = Resource.Name.Split('.')[0].Length;
			this.Text = "";
		}

		public void EndRename() {
			tbRename.Hide();
			Resource.Name = tbRename.Text;
			Resource.SendUpdate();
			Package.TriggerOnChangeEvent();
		}		

		public override void Focus() {
			base.Focus();
		}
	}
}
