using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FantasyScape.Resources;
using Lidgren.Network;
using System.Xml.Serialization;

namespace FantasyScape.Blocks {
	[XmlRoot("Texture")]
	public class FSTextureReference {
		[XmlAttribute("Location")]
		public BlockSide Location = BlockSide.All;

		[XmlText()]
		public Guid TextureID = Guid.Empty;

		[XmlIgnore]
		public FSTexture Texture {
			get {
				Resource res = Package.FindResource(TextureID);
				return (res as FSTexture) ?? Textures.FSErrorTexture;
			}

			set {
				TextureID = value.ID;
			}
		}

		internal void Write(NetOutgoingMessage Message) {
			Message.Write(TextureID.ToString());
			Message.Write((Int32)Location);
		}

		internal void Read(NetIncomingMessage Message) {
			if (!Guid.TryParse(Message.ReadString(), out TextureID)) {
				throw new Exception("Failed to parse Guid in FSTextureReference");
			}
			Location = (BlockSide)Message.ReadInt32();
		}
	}
}
