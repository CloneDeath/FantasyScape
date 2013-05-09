using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class BlockTypes : Resource {
		static List<BlockType> Types = new List<BlockType>();

		public void Load() {
			AddBlockType("Dirt", "Dirt", false);
			AddBlockType("Granite", "Granite", false);
			AddBlockType("Grass", "Grass", false);
			AddBlockType("Water", "Water", true);
		}

		private static void AddBlockType(string BlockTypeName, string TextureName, bool Liquid) {
			BlockType b = new BlockType();
			b.Name = BlockTypeName;
			b.Texture = TextureName;
			b.Liquid = Liquid;
			Types.Add(b);
		}

		public static void SendBlockTypes(NetConnection netConnection, NetServer Server) {
			NetOutgoingMessage nom = Server.CreateMessage();
			nom.Write("BlockTypesNum");
			nom.Write((Int32)Types.Count);
			Server.SendMessage(nom, netConnection, NetDeliveryMethod.ReliableUnordered);

			foreach (BlockType bt in Types) {
				bt.Send(netConnection, Server);
			}
		}

		internal static BlockType GetBlockType(string BlockTypeName) {
			foreach (BlockType b in Types) {
				if (b.Name == BlockTypeName) {
					return b;
				}
			}
			return null;
		}

		static bool SentRequest = false;
		public static int Count = -1;
		internal static bool ReceiveClient(List<NetIncomingMessage> Messages, NetClient Client) {
			if (!SentRequest) {
				SentRequest = true;
				RequestMessage msg = new RequestMessage(RequestType.BlockTypes);
				msg.Send(Client, Client.ServerConnection, NetDeliveryMethod.ReliableUnordered);
			}

			foreach (NetIncomingMessage Message in Messages) {
				if (Message.MessageType == NetIncomingMessageType.Data) {
					string Type = Message.ReadString();
					if (Type == "BlockTypesNum") {
						Count = Message.ReadInt32();
					} else if (Type == "BlockType") {
						BlockType.Receive(Message);
					}
					Message.Position = 0;
				}
			}

			if (Count == Types.Count) {
				return true;
			} else {
				return false;
			}
		}

		internal static void AddBlockType(BlockType b) {
			Types.Add(b);
		}
	}
}
