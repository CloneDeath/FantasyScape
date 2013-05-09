using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using GLImp;

namespace FantasyScape.NetworkMessages {
	public enum RequestType {
		Textures, BlockTypes, WorldSize, BlockLayers
	}
	public class RequestMessage : Message {
		RequestType Type;

		public RequestMessage(RequestType type) {
			this.Type = type;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(Type.ToString());
		}

		NetConnection Sender;
		protected override void ReadData(NetIncomingMessage Message) {
			Type = (RequestType)Enum.Parse(typeof(RequestType), Message.ReadString());
			Sender = Message.SenderConnection;
		}

		protected override void ExecuteMessage() {
			switch (Type) {
				case RequestType.Textures:
					SendTextures();
					break;
				case RequestType.BlockTypes:
					SendBlockTypes();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private void SendTextures() {
			Message NumTexture = new NumTextures();
			NumTexture.Send(Sender, NetDeliveryMethod.ReliableUnordered);

			foreach (Texture t in Textures.GetAll()){
				NetTexture nettex = new NetTexture(t);
				nettex.Send(Sender, NetDeliveryMethod.ReliableUnordered);
			}
		}

		private void SendBlockTypes() {
			BlockTypesNumber btn = new BlockTypesNumber();
			btn.Send(Sender, NetDeliveryMethod.ReliableUnordered);

			foreach (BlockType bt in BlockTypes.GetAll()) {
				BlockTypeData btd = new BlockTypeData(bt);
				btd.Send(Sender, NetDeliveryMethod.ReliableUnordered);
			}
		}
	}
}
