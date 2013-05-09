using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class WorldSizeMessage : Message {
		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write((Int32)Game.World.XSize);
			Message.Write((Int32)Game.World.YSize);
			Message.Write((Int32)Game.World.ZSize);
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
			Game.World.XSize = xsize;
			Game.World.YSize = ysize;
			Game.World.ZSize = zsize;

			Game.World.blocks = new Block[xsize, ysize, zsize];
			Game.World.Current = World.State.SendingLayersRequest;
		}
	}
}
