using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;
using FantasyScape.Resources;
using System.Xml.Linq;
using FantasyScape.Blocks;
using System.IO;

namespace FantasyScape {
	public class BlockType : Resource {
		public BlockType() {
			for (int i = 0; i < Texture.Length; i++) {
				Texture[i] = new FSTextureReference();
			}
		}

		public bool Liquid;
		FSTextureReference[] Texture = new FSTextureReference[(int)BlockSide.Count];

		public Texture GetTexture(BlockSide Side) {
			switch (Side) {
				case BlockSide.Top:
				case BlockSide.Bottom:
					if (Texture[(int)Side].Defined) {
						return Texture[(int)Side].Texture.Texture;
					} else {
						goto case BlockSide.All;
					}
				case BlockSide.Left:
				case BlockSide.Right:
				case BlockSide.Front:
				case BlockSide.Back:
					if (Texture[(int)Side].Defined) {
						return Texture[(int)Side].Texture.Texture;
					} else {
						goto case BlockSide.Side;
					}
				case BlockSide.Side:
					if (Texture[(int)BlockSide.Side].Defined) {
						return Texture[(int)BlockSide.Side].Texture.Texture;
					} else {
						goto case BlockSide.All;
					}
				case BlockSide.All:
					if (Texture[(int)BlockSide.All].Defined) {
						return Texture[(int)BlockSide.All].Texture.Texture;
					}
					break;
			}

			return Textures.ErrorTexture;
		}

		public override void Load(string path) {
			if (!Guid.TryParse(Path.GetFileNameWithoutExtension(path), out this.ID)){
				throw new Exception("Could not parse GUID: " + path);
			}
			XDocument doc = XDocument.Load(path);
			XElement Base = doc.FirstNode as XElement;
			if (Base == null || Base.Name != "Block") {
				throw new Exception("Expected 'Block' as the base element for file: " + path);
			}

			List<XElement> BlockInfo = new List<XElement>(Base.Descendants());

			foreach (XElement info in BlockInfo) {
				switch (info.Name.ToString()) {
					case "Name":
						this.Name = info.Value;
						break;
					case "Texture":
						Guid TexID;
						if (!Guid.TryParse(info.Value, out TexID)) {
							throw new Exception("Failed to parse texture guid");
						}
						Texture[(int)Enum.Parse(typeof(BlockSide), info.Attribute("Location").Value)].TextureID = TexID;
						break;
					default:
						throw new Exception("Unknown element in blocktype '" + info.Name + "'.");
				}
			}
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write(Liquid);
			for (int i = 0; i < (int)BlockSide.Count; i++) {
				Texture[i].Write(Message);
			}
		}

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			Liquid = Message.ReadBoolean();
			for (int i = 0; i < (int)BlockSide.Count; i++) {
				Texture[i].Read(Message);
			}
		}
	}
}
