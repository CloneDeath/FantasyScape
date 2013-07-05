using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Lidgren.Network;

namespace FantasyScape {
	public class ServerInfo {
		public string Name;
		public Guid StartupPackage;

		public void Load(string ResourceLocation) {
			XDocument doc = XDocument.Load(Path.Combine(ResourceLocation, "server.info"));

			XElement Base = doc.FirstNode as XElement;
			if (Base == null || Base.Name != "Server") {
				throw new Exception("Expected 'Server' as the base element for file: " + Path.Combine(ResourceLocation, "server.info"));
			}

			List<XElement> CodeInfo = new List<XElement>(Base.Descendants());
			foreach (XElement info in CodeInfo) {
				switch (info.Name.ToString()) {
					case "Name":
						this.Name = info.Value;
						break;
					case "StartupPackage":
						if (!Guid.TryParse(info.Value, out StartupPackage)){
							throw new Exception("Unable to parse Guid for startup package");
						}
						break;
					default:
						throw new Exception("Unknown element in ServerInfo '" + info.Name + "'.");
				}
			}
		}

		public void Save(string ResourceLocation) {
			XDocument doc = new XDocument();
			{
				XElement Base = new XElement("Server");
				{
					XElement Name = new XElement("Name", this.Name);
					Base.Add(Name);

					XElement Language = new XElement("StartupPackage", "{" + (StartupPackage.ToString().ToUpper()) + "}");
					Base.Add(Language);
				}
				doc.Add(Base);
			}
			doc.Save(Path.Combine(ResourceLocation, "server.info"));
		}

		internal void Write(NetOutgoingMessage Message) {
			Message.Write(Name);
			Message.Write(StartupPackage.ToString());
			Message.Write((Int32)Chunk.Size.X);
			Message.Write((Int32)Chunk.Size.Y);
			Message.Write((Int32)Chunk.Size.Z);
		}

		internal void Read(NetIncomingMessage Message) {
			Name = Message.ReadString();
			if (!Guid.TryParse(Message.ReadString(), out StartupPackage)) {
				throw new Exception("Unable to parse GUID for startup package in server info.");
			}
			Chunk.Size.X = Message.ReadInt32();
			Chunk.Size.Y = Message.ReadInt32();
			Chunk.Size.Z = Message.ReadInt32();
		}

		internal void Copy(ServerInfo info) {
			this.Name = info.Name;
			this.StartupPackage = info.StartupPackage;
			Chunk.SetSize(Chunk.Size.X, Chunk.Size.Y, Chunk.Size.Z);
		}
	}
}
