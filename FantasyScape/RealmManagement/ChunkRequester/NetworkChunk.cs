using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.RealmManagement.ChunkRequester.NetworkMessages;
using Lidgren.Network;
using FantasyScape.RealmManagement.WorldGeneration;

namespace FantasyScape.RealmManagement.ChunkRequester {
	class NetworkChunk {
		private Realm Realm;

		public Vector3i ChunkCoords;
		public readonly static Vector3i Size = new Vector3i(16, 16, 16);
		public bool RequestSent;

		public Block[, ,] BlockData = new Block[Size.X, Size.Y, Size.Z];

		public NetworkChunk(Realm Realm, Vector3i ChunkCoords) {
			this.Realm = Realm;
			this.ChunkCoords = ChunkCoords;
			RequestSent = false;
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
			if (!RequestSent) {
				new NetworkChunkRequest(this).Send();
				RequestSent = true;
			}
		}

		internal void Write(NetOutgoingMessage Message) {
			ChunkCoords.Write(Message);

			Vector3i BlockCoords = ChunkCoords * Size;
			for (int x = 0; x < Size.X; x++) {
				for (int y = 0; y < Size.Y; y++) {
					for (int z = 0; z < Size.Z; z++) {
						Vector3i BlockAt = BlockCoords + new Vector3i(x, y, z);

						if (!Game.World.MapGenerator.Exists(BlockAt / Sector.Size)) {
							Game.World.MapGenerator.GenerateSector(BlockAt / Sector.Size);
						}

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
