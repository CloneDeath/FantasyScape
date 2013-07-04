using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.Resources;
using Lidgren.Network;

namespace FantasyScape.NetworkMessages {
	public class UpdatePackage : Message {
		Package package;

		public UpdatePackage() {
			package = new Package(Guid.Empty);
		}

		public UpdatePackage(Package pkg) {
			this.package = pkg;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			package.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			package.Read(Message);
		}

		protected override void ExecuteMessage() {
			Package res = Package.FindResource(package.ID) as Package;
			res.Name = package.Name;
			res.References = package.References;

			new UpdatePackage(package).Forward();
		}
	}
}
