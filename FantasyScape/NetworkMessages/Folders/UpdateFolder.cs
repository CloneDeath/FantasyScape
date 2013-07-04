using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.Resources;

namespace FantasyScape.NetworkMessages {
	public class UpdateFolder : Message {
		private Folder folder;

		public UpdateFolder() {
			folder = new Folder();
		}

		public UpdateFolder(Folder folder) {
			this.folder = folder;
		}

		protected override void WriteData(NetOutgoingMessage Message) {
			folder.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			folder.Read(Message);
		}

		protected override void ExecuteMessage() {
			Resource res = Package.FindResource(folder.ID) as Folder;
			if (res == null) {
				throw new Exception("Could not find existing folder resource.");
			}
			res.Name = folder.Name;
			
			new UpdateFolder(folder).Forward();
		}
	}
}
