using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class PlayerAssignment : PlayerAdd {
		public PlayerAssignment() { }

		public PlayerAssignment(bool Dummy) {
			player = Game.AddNewPlayer();
		}

		protected override void ExecuteMessage() {
			base.ExecuteMessage();
			Game.SetSelf(player);
		}
	}
}
