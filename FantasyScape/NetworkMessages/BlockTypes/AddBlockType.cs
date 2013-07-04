using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.Resources;

namespace FantasyScape.NetworkMessages {
	public class AddBlockType : Message {
		private BlockType blockType;
		Guid parent = Guid.Empty;

		public AddBlockType() {
			blockType = new BlockType();
		}

		public AddBlockType(BlockType blockType, Guid parent) {
			this.blockType = blockType;
			this.parent = parent;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(parent.ToString());
			blockType.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			if (!Guid.TryParse(Message.ReadString(), out parent)) {
				throw new Exception("Could not parse parent guid for block type");
			}
			blockType.Read(Message);
		}

		protected override void ExecuteMessage() {
			Resource res = Package.FindResource(parent);
			if (res == null) {
				throw new Exception("Could not find parent resource for texture");
			}
			((Folder)res).Add(blockType);

			new AddBlockType(blockType, parent).Forward();
		}
	}
}
