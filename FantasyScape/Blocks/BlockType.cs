using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;

namespace FantasyScape {
	public class BlockType {
		public string Name;

		public string Texture {
			set {
				TopTexture = value;
				SideTexture = value;
				BotTexture = value;
			}
		}

		public string TopTexture;
		public string SideTexture;
		public string BotTexture;
		public bool Liquid;
	}
}
