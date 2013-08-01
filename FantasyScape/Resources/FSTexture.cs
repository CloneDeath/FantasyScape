using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.IO;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using FantasyScape.NetworkMessages;
using System.Xml.Serialization;

namespace FantasyScape.Resources {
	/// <summary>
	/// Fantasy Scape Texture. Basically a Texture wrapper that holds the associated GUID.
	/// </summary>
	[XmlRoot("Texture")]
	public class FSTexture : Resource {
		[XmlIgnore]
		public Texture Texture {
			get;
			private set;
		}

		public FSTexture() {
			ID = Guid.NewGuid();
		}

		public void Load(Bitmap img) {
			this.Texture = new Texture(img, this.Name, 0, 0);
		}

		public override void Save(string path) {
			string TexturePath = Path.Combine(path, GetIDString());
			this.Texture.Image.Save(TexturePath + ".png", ImageFormat.Png);

			XDocument doc = new XDocument();
			{
				XElement Base = new XElement("Texture");
				{
					XElement Name = new XElement("Name", this.Name);
					Base.Add(Name);
				}
				doc.Add(Base);
			}
			doc.Save(TexturePath + ".tex");
		}

		public override void SendUpdate() {
			new UpdateTexture(this).Send();
		}
	}
}
