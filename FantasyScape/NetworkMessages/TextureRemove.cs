using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	public class TextureRemove : Message {
		string tex;
		public TextureRemove(string b) {
			tex = b;
		}

		public TextureRemove() { }

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(tex);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			tex = Message.ReadString();
		}

		protected override void ExecuteMessage() {
			Textures.Remove(tex);

			new BlockTypeRemove(tex).Forward();
		}
	}
}
