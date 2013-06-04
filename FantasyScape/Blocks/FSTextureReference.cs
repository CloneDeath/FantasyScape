using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FantasyScape.Resources;

namespace FantasyScape.Blocks {
	class FSTextureReference {
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

		public bool Defined = false;

		public FSTexture Texture {
			get {
				throw new NotImplementedException();
			}
		}
	}
}
