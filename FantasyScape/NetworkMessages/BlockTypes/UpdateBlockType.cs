using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.Resources;

namespace FantasyScape.NetworkMessages {
	class UpdateBlockType : Message {
		BlockType bt;
		public UpdateBlockType() {
			bt = new BlockType();
		}

		public UpdateBlockType(BlockType block) {
			bt = block;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			bt.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			bt.Read(Message);
		}

		protected override void ExecuteMessage() {
			BlockType res = (BlockType)Package.FindResource(bt.ID);
			res.Copy(bt);
			new UpdateBlockType(bt).Forward();
		}
	}
}
