using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	public abstract partial class Message {
		enum ConnectionType { Server, Client };
		static ConnectionType ConnType;
		static NetServer Server;
		static NetClient Client;
		static NetConnection LastReceivedSender;

		static bool CanReply {
			get {
				return LastReceivedSender != null;
			}
		}

		static NetPeer Connection {
			get {
				if (ConnType == ConnectionType.Client) {
					return Client;
				} else {
					return Server;
				}
			}
		}

		public static void RegisterServer(NetServer server) {
			ConnType = ConnectionType.Server;
			Server = server;
			Client = null;
		}

		public static void RegisterClient(NetClient client) {
			ConnType = ConnectionType.Client;
			Server = null;
			Client = client;
		}

		public void Reply() {
			if (CanReply) {
				this.Send(LastReceivedSender);
			} else {
				throw new Exception("Can only reply in 'Execute' method of a Message"); //It's the only way we know who to reply to.
			}
		}

		private void Send(NetConnection destination) {
			this.Send(Connection, destination, NetDeliveryMethod.ReliableUnordered);
		}

		public void Send() {
			if (ConnType == ConnectionType.Client) {
				this.Send(Client, NetDeliveryMethod.ReliableUnordered);
			} else {
				this.Send(Server, NetDeliveryMethod.ReliableUnordered);
			}
		}

		private void Send(NetClient client, NetDeliveryMethod method) {
			this.Send(client, client.ServerConnection, method);
		}

		private void Send(NetServer peer, NetDeliveryMethod method) {
			foreach (NetConnection conn in peer.Connections) {
				Send(peer, conn, method);
			}
		}

		private void Send(NetPeer peer, NetConnection destination, NetDeliveryMethod method) {
			NetOutgoingMessage nom = peer.CreateMessage();
			nom.Write(this.GetType().Name);
			this.WriteData(nom);
			peer.SendMessage(nom, destination, method);
		}

		private void Send(NetConnection destination, NetDeliveryMethod method) {
			this.Send(destination.Peer, destination, method);
		}

		public void Receive(NetIncomingMessage Message) {
			ReadData(Message);

			LastReceivedSender = Message.SenderConnection;
			ExecuteMessage();
			LastReceivedSender = null;
		}

		protected abstract void WriteData(NetOutgoingMessage Message);
		protected abstract void ReadData(NetIncomingMessage Message);
		protected abstract void ExecuteMessage();
	}
}
