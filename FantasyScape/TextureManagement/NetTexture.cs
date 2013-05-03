using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;
using System.Drawing;

namespace FantasyScape {
	public class NetTexture : Texture {
		Bitmap img;

		public NetTexture(byte[] data, string name) : base(GetBitmap(data), name) {
			img = GetBitmap(data);
		}

		public NetTexture(string filename) : base(filename) {
			img = new Bitmap(filename);
		}

		internal void Send(NetConnection netConnection, NetServer Server) {
			NetOutgoingMessage nom = Server.CreateMessage();
			nom.Write("NetTexture");
			nom.Write(this.Name);
			byte[] data = GetBytes();
			nom.Write((Int32)data.Length);
			nom.Write(data);

			netConnection.SendMessage(nom, NetDeliveryMethod.ReliableSequenced, SequenceChannels.Textures);
		}

		internal static void Receive(NetIncomingMessage nim) {
			string name = nim.ReadString();
			int datalen = nim.ReadInt32();
			byte[] data = nim.ReadBytes(datalen);
			NetTexture nettex = new NetTexture(data, name);
			Textures.AddTexture(nettex);
		}

		internal byte[] GetBytes() {
			ImageConverter converter = new ImageConverter();
			return (byte[])converter.ConvertTo(img, typeof(byte[]));
		}

		static internal Bitmap GetBitmap(byte[] array) {
			ImageConverter ic = new ImageConverter();
			System.Drawing.Image pic = (System.Drawing.Image)ic.ConvertFrom(array);
			return new Bitmap(pic);
		}
	}
}
