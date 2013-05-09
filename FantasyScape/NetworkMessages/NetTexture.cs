using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;
using System.Drawing;

namespace FantasyScape.NetworkMessages {
	public class NetTexture : Message {
		Texture texture = null;

		public NetTexture() { }

		public NetTexture(Texture tex) {
			this.texture = tex;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(texture.Name);
			byte[] data = GetBytes(texture);
			Message.Write((Int32)data.Length);
			Message.Write(data);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			string name = Message.ReadString();
			int datalen = Message.ReadInt32();
			byte[] data = Message.ReadBytes(datalen);
			texture = new Texture(GetBitmap(data), name, 0, 0);
		}

		protected override void ExecuteMessage() {
			Textures.AddTexture(texture);
		}

		internal byte[] GetBytes(Texture texture) {
			ImageConverter converter = new ImageConverter();
			return (byte[])converter.ConvertTo(texture.Image, typeof(byte[]));
		}

		static internal Bitmap GetBitmap(byte[] array) {
			ImageConverter ic = new ImageConverter();
			System.Drawing.Image pic = (System.Drawing.Image)ic.ConvertFrom(array);
			return new Bitmap(pic);
		}
	}
}
