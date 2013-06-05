using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FantasyScape.Resources;
using Lidgren.Network;

namespace FantasyScape.Blocks {
	class FSTextureReference {
		public bool Defined = false;
		private Guid _TextureID = Guid.Empty;
		public Guid TextureID {
			get {
				return _TextureID;
			}
			set {
				_TextureID = value;
				Defined = (_TextureID != Guid.Empty);
			}
		}

		public FSTexture Texture {
			get {
				Resource res = Package.FindResource(_TextureID);
				return res as FSTexture;
			}
		}

		internal void Write(NetOutgoingMessage Message) {
			Message.Write(Defined);
			Message.Write(_TextureID.ToString());
		}

		internal void Read(NetIncomingMessage Message) {
			Defined = Message.ReadBoolean();
			if (!Guid.TryParse(Message.ReadString(), out _TextureID)) {
				throw new Exception("Failed to parse Guid in FSTextureReference");
			}
		}
	}
}
