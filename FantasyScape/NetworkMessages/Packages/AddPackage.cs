using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.Resources;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	class AddPackage : Message {
		Package package;

		public AddPackage() {
			package = new Package(Guid.Empty);
		}

		public AddPackage(Package pkg){
			this.package = pkg;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			package.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			package.Read(Message);
		}

		protected override void ExecuteMessage() {
			Package.Add(package);
			new AddPackage(package).Forward();
		}
	}
}
