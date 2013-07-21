using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.RealmManagement.ChunkRequester.NetworkMessages;
using Lidgren.Network;

namespace FantasyScape.RealmManagement.ChunkRequester {
	class NetworkChunk {
		private Realm Realm;
		private int Timeout;
		private const int TimeoutMax = 100;

		public Vector3i ChunkCoords;
		public readonly static Vector3i Size = new Vector3i(16, 16, 16);

		public Block[, ,] BlockData = new Block[Size.X, Size.Y, Size.Z];

		public NetworkChunk(Realm Realm, Vector3i ChunkCoords) {
			this.Realm = Realm;
			this.ChunkCoords = ChunkCoords;
			Timeout = TimeoutMax;
		}

		internal void Apply(Realm Realm) {
			Vector3i BlockCoords = ChunkCoords * Size;
			for (int x = 0; x < Size.X; x++) {
				for (int y = 0; y < Size.Y; y++) {
					for (int z = 0; z < Size.Z; z++) {
						Vector3i BlockAt = BlockCoords + new Vector3i(x, y, z);
						Realm.SetBlock(BlockAt, BlockData[x, y, z]);
					}
				}
			}
		}

		internal void SendRequest() {
			if (Timeout-- <= 0) {
				new NetworkChunkRequest(this).Send();
				Timeout = TimeoutMax;
			}
		}

		internal void Write(NetOutgoingMessage Message) {
			ChunkCoords.Write(Message);

			Vector3i BlockCoords = ChunkCoords * Size;
			for (int x = 0; x < Size.X; x++) {
				for (int y = 0; y < Size.Y; y++) {
					for (int z = 0; z < Size.Z; z++) {
						Vector3i BlockAt = BlockCoords + new Vector3i(x, y, z);
						Block Value = Realm.GetBlock(BlockAt);
						if (Value != null) {
							Message.Write(true);
							Value.Write(Message);
						} else {
							Message.Write(false);
						}
					}
				}
			}
		}

		internal void Read(NetIncomingMessage Message) {
			ChunkCoords.Read(Message);

			Vector3i BlockCoords = ChunkCoords * Size;
			for (int x = 0; x < Size.X; x++) {
				for (int y = 0; y < Size.Y; y++) {
					for (int z = 0; z < Size.Z; z++) {
						Vector3i BlockAt = BlockCoords + new Vector3i(x, y, z);

						bool Value = Message.ReadBoolean();
						if (Value) {
							BlockData[x, y, z] = new Block();
							BlockData[x, y, z].Read(Message);
						}
					}
				}
			}
		}
	}
}
