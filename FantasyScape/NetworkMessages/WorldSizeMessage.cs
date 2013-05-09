using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class WorldSizeMessage : Message {
		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write((Int32)Game.Instance.world.XSize);
			Message.Write((Int32)Game.Instance.world.YSize);
			Message.Write((Int32)Game.Instance.world.ZSize);
		}

		int xsize;
		int ysize;
		int zsize;
		protected override void ReadData(NetIncomingMessage Message) {
			xsize = Message.ReadInt32();
			ysize = Message.ReadInt32();
			zsize = Message.ReadInt32();
		}

		protected override void ExecuteMessage() {
			Game.Instance.world.XSize = xsize;
			Game.Instance.world.YSize = ysize;
			Game.Instance.world.ZSize = zsize;

			Game.Instance.world.blocks = new Block[xsize, ysize, zsize];
			Game.Instance.world.Current = World.State.SendingLayersRequest;
		}
	}
}
