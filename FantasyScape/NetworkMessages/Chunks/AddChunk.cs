﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;

namespace FantasyScape.NetworkMessages {
	class AddChunk : Message {
		private Vector3i Location;
		private Chunk chunk;

		public AddChunk() {
			Location = new Vector3i();
		}

		public AddChunk(Vector3i loc, Chunk chunk) {
			Location = loc;
			this.chunk = chunk;
		}

		protected override int InitialMessageSize() {
			return (Chunk.Size.X * Chunk.Size.Y * Chunk.Size.Z) * (1 + 4 + 38);
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Location.Write(Message);
			chunk.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			Location.Read(Message);
			chunk = new Chunk(Location);
			chunk.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.World.Chunks[Location] = chunk;
			Game.World.RefreshExposedChunks(Location);
		}
	}
}
