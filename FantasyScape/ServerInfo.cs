using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Lidgren.Network;
using FantasyScape.Resources;
using System.Xml.Serialization;
using System.Reflection;

namespace FantasyScape {
	[XmlRoot("Server")]
	public class ServerInfo : Resource {
		[XmlIgnore]
		public List<Configuration> Configurations = new List<Configuration>();

		[XmlElement("CurrentConfig")]
		private Guid CurrentConfigurationID;

		[XmlIgnore]
		public Configuration CurrentConfiguration {
			get {
				foreach (Configuration config in Configurations) {
					if (config.ID == CurrentConfigurationID) {
						return config;
					}
				}
				return null;
			}

			set {
				CurrentConfigurationID = value.ID;
			}
		}

		internal override void Write(NetOutgoingMessage Message) {
			Message.Write(Name);
			Message.Write(CurrentConfigurationID.ToString());
		}

		internal override void Read(NetIncomingMessage Message) {
			Name = Message.ReadString();
			if (!Guid.TryParse(Message.ReadString(), out CurrentConfigurationID)) {
				throw new Exception("Unable to parse GUID for CurrentConfig in server info.");
			}
		}

		internal void Copy(ServerInfo info) {
			this.Name = info.Name;
			this.CurrentConfigurationID = info.CurrentConfigurationID;
		}

		public override void SendUpdate() {
			throw new NotImplementedException();
		}
	}
}
