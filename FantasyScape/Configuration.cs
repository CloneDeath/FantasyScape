using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.Resources;
using Lidgren.Network;

namespace FantasyScape {
	public class Configuration : Resource {
		List<Guid> EnabledPackages = new List<Guid>();

		public override void Save(string path) {
			throw new NotImplementedException();
		}

		public override void Load(string path) {
			throw new NotImplementedException();
		}

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			int NumGuids = Message.ReadInt32();

			EnabledPackages.Clear();
			for (int i = 0; i < NumGuids; i++) {
				EnabledPackages.Add(Guid.Parse(Message.ReadString()));
			}
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write(EnabledPackages.Count());
			foreach (Guid guid in EnabledPackages) {
				Message.Write(guid.ToString());
			}
		}

		public override void SendUpdate() {
			throw new NotImplementedException();
		}
	}
}
