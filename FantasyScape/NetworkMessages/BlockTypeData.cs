using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class BlockTypeData : Message {
		BlockType block = null;

		public BlockTypeData() { }
		public BlockTypeData(BlockType ToSend) {
			this.block = ToSend;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(block.Name);
			Message.Write(block.TopTexture);
			Message.Write(block.SideTexture);
			Message.Write(block.BotTexture);
			Message.Write(block.Liquid);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			block = new BlockType();
			block.Name = Message.ReadString();
			block.TopTexture = Message.ReadString();
			block.SideTexture = Message.ReadString();
			block.BotTexture = Message.ReadString();
			block.Liquid = Message.ReadBoolean();
		}

		protected override void ExecuteMessage() {
			BlockTypes.AddBlockType(block);
		}
	}
}
