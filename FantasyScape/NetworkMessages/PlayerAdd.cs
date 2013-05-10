using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class PlayerAdd : Message {
		public Player player;

		public PlayerAdd() { }

		public PlayerAdd(Player player) {
			this.player = player;
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			player = Game.AddNewPlayer();
			player.Write(Message);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			player = new Player();
			player.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.AddPlayer(player);
		}
	}
}
