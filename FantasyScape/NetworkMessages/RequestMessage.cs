using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

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

		protected override void ReadData(NetIncomingMessage Message) {
			Type = (RequestType)Enum.Parse(typeof(RequestType), Message.ReadString());
		}

		protected override void ExecuteMessage() {
			throw new NotImplementedException();
		}
	}
}
