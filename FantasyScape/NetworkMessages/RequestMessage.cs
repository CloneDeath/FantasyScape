using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using GLImp;

namespace FantasyScape.NetworkMessages {
	public enum RequestType {
		Packages, Chunks, NewPlayer,
	}
	public class RequestMessage : Message {
		RequestType Type;

		public RequestMessage() { }

		public RequestMessage(RequestType type) {
			this.Type = type;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(Type.ToString());
		}

		protected override void ReadData(NetIncomingMessage Message) {
			Type = (RequestType)Enum.Parse(typeof(RequestType), Message.ReadString());
		}

		protected override void ExecuteMessage() {
			switch (Type) {
				case RequestType.Packages:
					SendPackages();
					break;
				case RequestType.Chunks:
					SendChunks();
					break;
				case RequestType.NewPlayer:
					SendPlayerData();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private void SendPlayerData() {
			foreach (Player p in Game.Players) {
				PlayerAdd add = new PlayerAdd(p);
				add.Reply();
			}

			PlayerAssignment passign = new PlayerAssignment(true);
			passign.Reply();

			PlayerAdd padd = new PlayerAdd(passign.player);
			padd.Forward();
		}

		private void SendChunks() {
			for (int x = 0; x < World.XSize; x++) {
				for (int y = 0; y < World.YSize; y++) {
					for (int z = 0; z < World.ZSize; z++) {
						new ChunkAdd(x, y, z, Game.World.Chunks[x,y,z]).Reply();
					}
				}
			}
		}

		private void SendPackages() {
			throw new NotImplementedException();
		}
	}
}
