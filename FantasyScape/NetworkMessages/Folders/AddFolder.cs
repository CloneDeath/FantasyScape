using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.Resources;

namespace FantasyScape.NetworkMessages {
	class AddFolder : Message {
		Guid parent = Guid.Empty;
		private Folder folder;

		public AddFolder() {
			folder = new Folder();
		}

		public AddFolder(Folder folder, Guid parent) {
			this.folder = folder;
			this.parent = parent;
		}
		protected override void WriteData(NetOutgoingMessage Message) {
			Message.Write(parent.ToString());
			folder.Write(Message);
		}

		protected override void ReadData(NetIncomingMessage Message) {
			if (!Guid.TryParse(Message.ReadString(), out parent)) {
				throw new Exception("Failed to pare parent for folder");
			}
			folder.Read(Message);
		}

		protected override void ExecuteMessage() {
			Resource res = Package.FindResource(parent);
			if (res == null) {
				throw new Exception("Could not find parent resource for texture");
			}
			((Folder)res).Children.Add(folder);

			new AddFolder(folder, parent).Forward();
		}
	}
}
