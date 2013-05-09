using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	public abstract partial class Message {
		public void Send(NetClient client, NetDeliveryMethod method) {
			this.Send(client, client.ServerConnection, method);
		}

		public void Send(NetPeer peer, NetConnection destination, NetDeliveryMethod method) {
			NetOutgoingMessage nom = peer.CreateMessage();
			nom.Write(this.GetType().Name);
			this.WriteData(nom);
			peer.SendMessage(nom, destination, method);
		}

		public void Receive(NetIncomingMessage Message) {
			ReadData(Message);
			ExecuteMessage();
		}

		protected abstract void WriteData(NetOutgoingMessage Message);
		protected abstract void ReadData(NetIncomingMessage Message);
		protected abstract void ExecuteMessage();
	}
}
