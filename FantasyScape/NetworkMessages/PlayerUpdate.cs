using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class PlayerUpdate : Message {
		Player p;
		public PlayerUpdate() { }

		public PlayerUpdate(Player p) {
			this.p = p;
		}

		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Message.Write((Int32)p.PlayerID);
			p.Write(Message);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			p = Game.FindPlayer(Message.ReadInt32());
			//TODO: nrodine3, add the player instead. Possibly merge with PlayerAdd message...?
			if (p != null) {
				p.Read(Message);
			}
		}

		protected override void ExecuteMessage() {
			this.Forward();
		}
	}
}
