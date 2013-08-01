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
using System.Xml.Serialization;

namespace FantasyScape {
	[XmlRoot("Block")]
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
		}

		[XmlElement("Liquid")]
		public bool Liquid = false;

		[XmlElement("Transparent")]
		public bool Transparent = false;

		[XmlArray("Textures")]
		[XmlArrayItem("Texture")]
		public List<FSTextureReference> Texture = new List<FSTextureReference>();

		public Texture GetTexture(BlockSide Side) {
			FSTextureReference Candidate = null;
			do {
				Candidate = Texture.Find(delegate(FSTextureReference obj) { return obj.Location == Side; });
				if (Candidate != null) {
					return Candidate.Texture.Texture;
				} else {
					if (Side == BlockSide.All) {
						return Textures.ErrorTexture;
					} else if (Side == BlockSide.Side || Side == BlockSide.Top || Side == BlockSide.Bottom) {
						Side = BlockSide.All;
					} else {
						Side = BlockSide.Side;
					}
				}
			} while (Candidate != null);
			return Textures.ErrorTexture;
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write(Liquid);
			Message.Write(Transparent);
			Message.Write((Int32)Texture.Count);
			for (int i = 0; i < Texture.Count; i++) {
				Texture[i].Write(Message);
			}
		}

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			Liquid = Message.ReadBoolean();
			Transparent = Message.ReadBoolean();

			int TexCount = Message.ReadInt32();
			Texture.Clear();
			for (int i = 0; i < TexCount; i++) {
				FSTextureReference tex = new FSTextureReference();
				tex.Read(Message);
				Texture.Add(tex);
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
