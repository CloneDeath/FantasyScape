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
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class BlockType : Resource {
		static BlockType ErrorBlock = new BlockType();

		static BlockType() {
			ErrorBlock.ID = Guid.Empty;
			ErrorBlock.Liquid = false;
			ErrorBlock.Name = "Error";
		}

		public static BlockType Get(Guid BlockTypeID){
			return Package.FindResource(BlockTypeID) as BlockType ?? ErrorBlock;
		}

		public BlockType() {
			ID = Guid.NewGuid();
			Liquid = false;
			Transparent = false;
			for (int i = 0; i < Texture.Length; i++) {
				Texture[i] = new FSTextureReference();
			}
		}

		public bool Liquid;
		public bool Transparent;
		public FSTextureReference[] Texture = new FSTextureReference[(int)BlockSide.Count];

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

		public override void Save(string path) {
			string BlockPath = Path.Combine(path, GetIDString() + ".block");

			XDocument doc = new XDocument();
			{
				XElement Base = new XElement("Block");
				{
					XElement Name = new XElement("Name", this.Name);
					Base.Add(Name);

					for (int i = 0; i < (int)BlockSide.Count; i++){
						FSTextureReference tex = Texture[i];
						if (tex.Defined){
							XElement TexNode = new XElement("Texture");
							TexNode.SetAttributeValue("Location", ((BlockSide)i).ToString());
							TexNode.SetValue(tex.TextureID);
							Base.Add(TexNode);
						}
					}

					if (Liquid){
						XElement LiquidNode = new XElement("Liquid");
						Base.Add(LiquidNode);
					}

					if (Transparent) {
						XElement LiquidNode = new XElement("Transparent");
						Base.Add(LiquidNode);
					}
				}
				doc.Add(Base);
			}
			doc.Save(BlockPath);
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
					case "Liquid":
						Liquid = true;
						break;
					case "Transparent":
						Transparent = true;
						break;
					default:
						throw new Exception("Unknown element in blocktype '" + info.Name + "'.");
				}
			}
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write(Liquid);
			Message.Write(Transparent);
			for (int i = 0; i < (int)BlockSide.Count; i++) {
				Texture[i].Write(Message);
			}
		}

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			Liquid = Message.ReadBoolean();
			Transparent = Message.ReadBoolean();
			for (int i = 0; i < (int)BlockSide.Count; i++) {
				Texture[i].Read(Message);
			}
		}

		internal override void Copy(Resource res) {
			base.Copy(res);

			BlockType other = res as BlockType;
			this.Liquid = other.Liquid;
			this.Transparent = other.Transparent;
			for (int i = 0; i < (int)BlockSide.Count; i++) {
				Texture[i] = other.Texture[i];
			}
		}

		public static Block GetInstance(string BlockName) {
			return new Block(Package.FindResourceByName(BlockName).ID);
		}

		public override void SendUpdate() {
			new UpdateBlockType(this).Send();
		}
	}
}
