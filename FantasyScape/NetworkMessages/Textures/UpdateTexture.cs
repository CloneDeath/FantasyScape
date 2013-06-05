using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.Resources;
using GLImp;
using System.Drawing;

namespace FantasyScape.NetworkMessages {
	public class UpdateTexture : Message {
		FSTexture texture = null;
        Bitmap img = null;
        private FSTexture tex;

		public UpdateTexture() {
			this.texture = new FSTexture();
		}

        public UpdateTexture(FSTexture tex) {
			this.texture = tex;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			texture.Write(Message);
			byte[] data = GetBytes(texture.Texture);
			Message.Write((Int32)data.Length);
			Message.Write(data);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			texture.Read(Message);
			int datalen = Message.ReadInt32();
			byte[] data = Message.ReadBytes(datalen);
			img = GetBitmap(data);
		}

		protected override void ExecuteMessage() {
			FSTexture res = Package.FindResource(texture.ID) as FSTexture;
			if (res == null) {
				throw new Exception("Expected to find a texture with given ID, found something else instead.");
			}
            res.Name = texture.Name;
            res.Texture.Image = img;
            res.TriggerUpdateEvent(this);

            new UpdateTexture(res).Forward();
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
