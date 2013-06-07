using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client.Editor {
	class DevelopmentMenu : Base {
        public static DevelopmentMenu Instance = new DevelopmentMenu();

		private bool _Hidden = false;
		public static bool Hidden {
			get { return Instance._Hidden; }
			set {
				Instance._Hidden = value;
				if (Instance._Hidden) {
                    Instance.Hide();
				} else {
					Instance.Show();
				}
			}
		}

		private DevelopmentMenu() : base(MainCanvas.GetCanvas()){
			this.Dock = Gwen.Pos.Fill;

			TopBar top = new TopBar(this);
		}

	}
}
