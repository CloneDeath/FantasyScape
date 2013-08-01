using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using System.Xml;
using FantasyScape.Blocks;

namespace FantasyScape.Resources {
	public abstract class Resource {
		#region Public Members
		[XmlIgnore]
		public Guid ID;
		[XmlElement("Name")]
		public string Name = "[New Resource]";
		#endregion

		#region Abstract Functions
		public abstract void SendUpdate();
		#endregion


		public string GetIDString() {
			return "{" + ID.ToString().ToUpper() + "}";
		}

		internal virtual void Write(NetOutgoingMessage Message) {
			Message.Write(ID.ToString());
			Message.Write(Name);
		}

		internal virtual void Read(NetIncomingMessage Message) {
			if (!Guid.TryParse(Message.ReadString(), out ID)) {
				throw new Exception("Failed to parse GUID");
			}
			Name = Message.ReadString();
		}

		internal virtual void Copy(Resource other) {
			this.Name = other.Name;
		}

		internal virtual Resource GetResource(Guid ResourceID) {
			if (this.ID == ResourceID) {
				return this;
			} else {
				return null;
			}
		}

        public delegate void OnChangeEvent(object sender, Resource res);
        public event OnChangeEvent OnUpdate;

        public void TriggerUpdateEvent(object sender) {
            if (OnUpdate != null) {
                OnUpdate(sender, this);
            }
        }

		public static Resource Load(string File, Type ResourceType) {
			List<Type> Resources = new List<Type>(
			    Assembly.GetExecutingAssembly().GetTypes().Where(
			        delegate(Type t) { return typeof(Resource).IsAssignableFrom(t); }
			    )
			);
			Resources.Add(typeof(FSTextureReference)); //We need a level above Resource, like ISerializable

			XmlSerializer Reader = new XmlSerializer(ResourceType, Resources.ToArray());
			return (Resource)Reader.Deserialize(new StreamReader(File));
		}

		public virtual void Save(string File) {
			List<Type> Resources = new List<Type>(
				Assembly.GetExecutingAssembly().GetTypes().Where(
					delegate(Type t) { return typeof(Resource).IsAssignableFrom(t); }
				)
			);
			Resources.Add(typeof(FSTextureReference)); //We need a level above Resource, like ISerializable

			XmlSerializer Writer = new XmlSerializer(this.GetType(), Resources.ToArray());

			Writer.Serialize(new StreamWriter(File), this);
		}
	}
}
