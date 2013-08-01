using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.Resources;

namespace FantasyScape.NetworkMessages.Code {
	public class UpdateCode : Message {
		CodeFile Resource;

		public UpdateCode() {
			this.Resource = new CodeFile();
		}

		public UpdateCode(CodeFile resource) {
			this.Resource = resource;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			Resource.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			Resource.Read(Message);
		}

		protected override void ExecuteMessage() {
			CodeFile res = Package.FindResource(Resource.ID) as CodeFile;
			if (res == null) {
				throw new Exception("Expected to find a texture with given ID, found something else instead.");
			}
			res.Name = Resource.Name;
			res.Language = Resource.Language;
			res.Location = Resource.Location;
			res.Source = Resource.Source;
			res.TriggerUpdateEvent(this);

			new UpdateCode(res).Forward();
		}
	}
}
