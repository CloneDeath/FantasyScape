using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class UpdateBlockType : Message {
		public UpdateBlockType() {
			throw new NotImplementedException();
		}

		public UpdateBlockType(BlockType block) {
			throw new NotImplementedException();
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			throw new NotImplementedException();
		}

		protected override void ReadData(NetIncomingMessage Message) {
			throw new NotImplementedException();
		}

		protected override void ExecuteMessage() {
			throw new NotImplementedException();
		}
	}
}
