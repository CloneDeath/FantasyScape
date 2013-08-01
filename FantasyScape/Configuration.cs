using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.Resources;
using Lidgren.Network;

namespace FantasyScape {
	public class Configuration : Resource {
		List<Guid> Packages = new List<Guid>();

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			Packages.Clear();
			
			int NumGuids = Message.ReadInt32();
			for (int i = 0; i < NumGuids; i++) {
				Packages.Add(Guid.Parse(Message.ReadString()));
			}
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write(Packages.Count());
			foreach (Guid guid in Packages) {
				Message.Write(guid.ToString());
			}
		}

		public override void SendUpdate() {
			throw new NotImplementedException();
		}
	}
}
