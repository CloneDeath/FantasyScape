using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages.ServerInformation {
	public class UpdateServerInfo : Message {
		ServerInfo info;
		
		public UpdateServerInfo() {
			info = new ServerInfo();
		}

		public UpdateServerInfo(ServerInfo info) {
			this.info = info;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			info.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			info.Read(Message);
		}

		protected override void ExecuteMessage() {
			Game.ServerInfo.Copy(info);
			new UpdateServerInfo(info).Forward();
		}
	}
}
