using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class PlayerAssignment : PlayerAdd {
		protected override void ExecuteMessage() {
			base.ExecuteMessage();
			Game.SetSelf(player);
		}
	}
}
