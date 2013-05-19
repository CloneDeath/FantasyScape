using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client.Editor {
	class DevelopmentMenu : Base {
		private bool _Hidden = false;
		public bool Hidden {
			get { return _Hidden; }
			set {
				_Hidden = value;
				if (_Hidden) {
					this.Hide();
				} else {
					this.Show();
				}
			}
		}

		public DevelopmentMenu() : base(MainCanvas.GetCanvas()){
			this.Dock = Gwen.Pos.Fill;

			TopBar top = new TopBar(this);
		}

	}
}
