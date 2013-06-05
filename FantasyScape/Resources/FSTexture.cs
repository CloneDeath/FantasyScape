using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.IO;
using System.Xml.Linq;
using System.Drawing;

namespace FantasyScape.Resources {
	/// <summary>
	/// Fantasy Scape Texture. Basically a normal texture wrapper that holds the GUID.
	/// </summary>
	public class FSTexture : Resource {
		public FSTexture() {
			
		}

		public void Load(Bitmap img) {
			this.Texture = new Texture(img, this.Name, 0, 0);
		}

		public override void Load(string path) {
			if (!Guid.TryParse(Path.GetFileNameWithoutExtension(path), out this.ID)) {
				throw new Exception("Could not parse GUID: " + path);
			}
			Texture = new GLImp.Texture(path.Replace(".tex", ".png"));

			XDocument doc = XDocument.Load(path);
			XElement Base = doc.FirstNode as XElement;
			if (Base == null || Base.Name != "Texture") {
				throw new Exception("Expected 'Texture' as the base element for file: " + path);
			}

			List<XElement> BlockInfo = new List<XElement>(Base.Descendants());

			foreach (XElement info in BlockInfo) {
				switch (info.Name.ToString()) {
					case "Name":
						this.Name = info.Value;
						break;
					default:
						throw new Exception("Unknown element in blocktype '" + info.Name + "'.");
				}
			}
			
		}

		public Texture Texture {
			get;
			private set;
		}
	}
}
