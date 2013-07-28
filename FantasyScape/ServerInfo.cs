using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Lidgren.Network;
using FantasyScape.Resources;

namespace FantasyScape {
	public class ServerInfo {
		public string Name;
		private Guid CurrentConfigGuid;

		public Configuration CurrentConfig {
			get {
				foreach (Configuration config in AllConfigs) {
					if (config.ID == CurrentConfigGuid) {
						return config;
					}
				}
				return null;
			}

			set {
				CurrentConfigGuid = value.ID;
			}
		}

		public List<Configuration> AllConfigs = new List<Configuration>();

		public void Load(string ResourceLocation) {
			string InfoFile = Path.Combine(ResourceLocation, "server.info");
			if (File.Exists(InfoFile)) {
				LoadServerInfo(ResourceLocation, InfoFile);
			} else {
				Name = "FantasyScape Server";
				AllConfigs.Add(new Configuration());
				CurrentConfigGuid = AllConfigs[0].ID;
			}
		}

		private void LoadServerInfo(string ResourceLocation, string InfoFile) {
			XDocument doc = XDocument.Load(InfoFile);

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
					case "CurrentConfig":
						if (!Guid.TryParse(info.Value, out CurrentConfigGuid)) {
							throw new Exception("Unable to parse Guid for default configuration");
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

					XElement Package = new XElement("CurrentConfig", "{" + (this.CurrentConfigGuid.ToString().ToUpper()) + "}");
					Base.Add(Package);
				}
				doc.Add(Base);
			}
			doc.Save(Path.Combine(ResourceLocation, "server.info"));
		}

		internal void Write(NetOutgoingMessage Message) {
			Message.Write(Name);
			Message.Write(CurrentConfigGuid.ToString());
		}

		internal void Read(NetIncomingMessage Message) {
			Name = Message.ReadString();
			if (!Guid.TryParse(Message.ReadString(), out CurrentConfigGuid)) {
				throw new Exception("Unable to parse GUID for CurrentConfig in server info.");
			}
		}

		internal void Copy(ServerInfo info) {
			this.Name = info.Name;
			this.CurrentConfigGuid = info.CurrentConfigGuid;
		}
	}
}
