using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.Resources;
using GLImp;
using System.Drawing;

namespace FantasyScape.NetworkMessages {
	public class AddTexture : Message {
		FSTexture texture = null;
		Guid parent = Guid.Empty;

		public AddTexture() {
			this.texture = new FSTexture();
		}

		public AddTexture(FSTexture tex, Guid parent) {
			this.texture = tex;
			this.parent = parent;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(parent.ToString());
			texture.Write(Message);
			byte[] data = GetBytes(texture.Texture);
			Message.Write((Int32)data.Length);
			Message.Write(data);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			if (!Guid.TryParse(Message.ReadString(), out parent)) {
				throw new Exception("Failed to parse parent guid for addtexture");
			}
			texture.Read(Message);
			int datalen = Message.ReadInt32();
			byte[] data = Message.ReadBytes(datalen);
			texture.Load(GetBitmap(data));
		}

		protected override void ExecuteMessage() {
			Resource res = Package.FindResource(parent);
			if (res == null) {
				throw new Exception("Could not find parent resource for texture");
			}
			((Folder)res).Add(texture);

			new AddTexture(texture, parent).Forward();
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
