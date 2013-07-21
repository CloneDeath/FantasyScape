﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace FantasyScape.Resources {
	public abstract class Resource {
		#region Public Members
		public Guid ID;
		public string Name = "[New Resource]";
		#endregion

		#region Abstract Functions
		public abstract void Save(string path);
		public abstract void Load(string path);
		public abstract void SendUpdate();
		#endregion


		protected string GetIDString() {
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
	}
}
