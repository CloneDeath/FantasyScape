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
				case RequestType.Textures:
					SendTextures();
					break;
				case RequestType.BlockTypes:
					SendBlockTypes();
					break;
				case RequestType.BlockLayers:
					SendBlockLayers();
					break;
				case RequestType.WorldSize:
					SendWorldSize();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		private void SendWorldSize() {
			WorldSizeMessage wsm = new WorldSizeMessage();
			wsm.Reply();
		}

		private void SendBlockLayers() {
			for (int x = 0; x < Game.World.XSize; x++) {
				for (int y = 0; y < Game.World.YSize; y++) {
					BlockLayersMessage blm = new BlockLayersMessage(x, y);
					blm.Reply();
				}
			}
		}

		private void SendTextures() {
			Message NumTexture = new NumTextures();
			NumTexture.Reply();

			foreach (Texture t in Textures.GetAll()){
				NetTexture nettex = new NetTexture(t);
				nettex.Reply();
			}
		}

		private void SendBlockTypes() {
			BlockTypesNumber btn = new BlockTypesNumber();
			btn.Reply();

			foreach (BlockType bt in BlockTypes.GetAll()) {
				BlockTypeData btd = new BlockTypeData(bt);
				btd.Reply();
			}
		}
	}
}
