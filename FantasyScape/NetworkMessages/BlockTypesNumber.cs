using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.NetworkMessages {
	class BlockTypesNumber : Message {
		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Message.Write((Int32)BlockTypes.GetAll().Count);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			BlockTypes.Count = Message.ReadInt32();
		}

		protected override void ExecuteMessage() {}
	}
}
