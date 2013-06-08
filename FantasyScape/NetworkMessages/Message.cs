using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	public abstract partial class Message {
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
				if (Game.Host == HostType.Client) {
					return Client;
				} else {
					return Server;
				}
			}
		}

		public static void RegisterServer(NetServer server) {
			Game.Host = HostType.Server;
			Server = server;
			Client = null;
		}

		public static void RegisterClient(NetClient client) {
			Game.Host = HostType.Client;
			Server = null;
			Client = client;
		}

		#region Reply
		public void Reply(NetDeliveryMethod method) {
			if (CanReply) {
				this.Send(LastReceivedSender, method);
			} else {
				throw new Exception("Can only reply in 'Execute' method of a Message"); //It's the only way we know who to reply to.
			}
		}
		public void Reply() {
			this.Reply(NetDeliveryMethod.ReliableUnordered);
		}
		#endregion

		private void Send(NetConnection destination) {
			this.Send(Connection, destination, NetDeliveryMethod.ReliableUnordered);
		}

		public void Send() {
			if (Game.Host == HostType.Client) {
				this.Send(Client, NetDeliveryMethod.ReliableUnordered);
			} else {
				this.Send(Server, NetDeliveryMethod.ReliableUnordered);
			}
		}

		private void Send(NetClient client, NetDeliveryMethod method) {
			this.Send(client, client.ServerConnection, method);
		}

		private void Send(NetServer server, NetDeliveryMethod method) {
			this.Send(server, server.Connections, NetDeliveryMethod.ReliableUnordered);
		}

		private void Send(NetPeer peer, List<NetConnection> clients, NetDeliveryMethod method) {
			foreach (NetConnection conn in clients) {
				Send(peer, conn, method);
			}
		}

		protected virtual int InitialMessageSize() {
			return 16;
		}

		private void Send(NetPeer peer, NetConnection destination, NetDeliveryMethod method) {
			NetOutgoingMessage nom = peer.CreateMessage(InitialMessageSize());
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

		/// <summary>
		/// Sends the message to everyone EXCEPT the person who you would reply to.
		/// 
		/// Thus, this is a server only command. Sending it as a client does nothing.
		/// </summary>
		internal void Forward() {
			if (Game.Host == HostType.Client) {
				//Do nothing
			} else {
				List<NetConnection> Replyto = new List<NetConnection>(
					Server.Connections.Where((c)=> {return c != LastReceivedSender;}
				));
				this.Send(Server, Replyto, NetDeliveryMethod.ReliableUnordered);
			}
		}

		protected abstract void WriteData(NetOutgoingMessage Message);
		protected abstract void ReadData(NetIncomingMessage Message);
		protected abstract void ExecuteMessage();
	}
}
