using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;

namespace FantasyScape {
	public class Textures {
		static List<NetTexture> TextureList = new List<NetTexture>();
		static bool RequestSent = false;
		static int TextureCount = -1;

		public static void ServerLoadTextures() {
			AddTexture("Data/Dirt.png", "Dirt");
			AddTexture("Data/Grass.png", "Grass");
			AddTexture("Data/Granite.png", "Granite");
			AddTexture("Data/Water.png", "Water");
		}

		private static void AddTexture(string filename, string name) {
			NetTexture Dirt = new NetTexture(filename);
			Dirt.Name = name;

			TextureList.Add(Dirt);
		}

		public static Texture GetTexture(string name) {
			foreach (Texture t in TextureList) {
				if (t.Name == name) {
					return t;
				}
			}
			return null;
		}

		public static void SendTextures(NetConnection netConnection, NetServer Server) {
			NetOutgoingMessage nom = Server.CreateMessage();
			nom.Write("NumTextures");
			nom.Write((Int32)TextureList.Count);
			Server.SendMessage(nom, netConnection, NetDeliveryMethod.ReliableUnordered);

			foreach (NetTexture tex in TextureList) {
				tex.Send(netConnection, Server);
			}
		}

		public static void AddTexture(NetTexture nettex) {
			if (GetTexture(nettex.Name) == null) {
				TextureList.Add(nettex);
			} else {
				Console.WriteLine("Received Duplicate Texture Name: " + nettex.Name);
			}
		}

		internal static bool ReceiveClient(List<NetIncomingMessage> Messages, NetClient Client) {
			foreach (NetIncomingMessage Message in Messages) {
				if (Message.MessageType == NetIncomingMessageType.Data) {
					string Type = Message.ReadString();
					if (Type == "NumTextures") {
						TextureCount = Message.ReadInt32();
					} else if (Type == "NetTexture") {
						NetTexture.Receive(Message);
					}
					Message.Position = 0;
				}
			}


			if (!RequestSent) {
				NetOutgoingMessage nom = Client.CreateMessage();
				nom.Write("Request");
				nom.Write("Textures");
				Client.SendMessage(nom, NetDeliveryMethod.ReliableUnordered);
				RequestSent = true;
			}

			if (TextureList.Count == TextureCount) {
				return true;
			} else {
				return false;
			}
		}
	}
}
