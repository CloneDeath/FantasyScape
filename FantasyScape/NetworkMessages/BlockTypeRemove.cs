using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	public class BlockTypeRemove : Message {
		string block;
		public BlockTypeRemove(string b) {
			block = b;
		}

		public BlockTypeRemove() { }

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(block);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			block = Message.ReadString();
		}

		protected override void ExecuteMessage() {
			BlockTypes.Remove(block);

			new BlockTypeRemove(block).Forward();
		}
	}
}
