using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using GLImp;
using FantasyScape.Resources;
using FantasyScape.NetworkMessages.Code;
using FantasyScape.NetworkMessages.ServerInformation;

namespace FantasyScape.NetworkMessages {
	public enum RequestType {
		Packages, NewPlayer,
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

			UpdateServerInfo info = new UpdateServerInfo(Game.ServerInfo);
			info.Reply();
		}

		private void SendPackages() {
			foreach (Package pkg in Package.GetPackages()){
				Send(pkg, Guid.Empty);
			}
		}

		private void Send(Resource res, Guid parent) {
			if (res.GetType() == typeof(Package)) {
				new AddPackage((Package)res).Reply(NetDeliveryMethod.ReliableOrdered);
				foreach (Resource child in ((Package)res).GetChildren()) {
					Send(child, res.ID);
				}
			} else if (res.GetType() == typeof(Folder)) {
				new AddFolder((Folder)res, parent).Reply(NetDeliveryMethod.ReliableOrdered);
				foreach (Resource child in ((Folder)res).GetChildren()) {
					Send(child, res.ID);
				}
			} else if (res.GetType() == typeof(FSTexture)) {
				new AddTexture((FSTexture)res, parent).Reply(NetDeliveryMethod.ReliableOrdered);
			} else if (res.GetType() == typeof(BlockType)) {
				new AddBlockType((BlockType)res, parent).Reply(NetDeliveryMethod.ReliableOrdered);
			} else if (res.GetType() == typeof(CodeFile)) {
				new AddCode((CodeFile)res, parent).Reply(NetDeliveryMethod.ReliableOrdered);
			}
		}
	}
}
