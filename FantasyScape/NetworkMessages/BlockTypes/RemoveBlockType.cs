using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	public class RemoveBlockType : Message {
		private Guid guid;

		public RemoveBlockType(Guid guid) {
			// TODO: Complete member initialization
			this.guid = guid;
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
