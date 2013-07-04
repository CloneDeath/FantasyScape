using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.Resources;

namespace FantasyScape.NetworkMessages.Code {
	public class AddCode : Message {
		CodeFile Code;
		Guid Parent;

		public AddCode() {
			this.Code = new CodeFile();
			this.Parent = Guid.Empty;
		}

		public AddCode(CodeFile CodeFile, Guid Parent) {
			this.Code = CodeFile;
			this.Parent = Parent;
		}
		protected override void WriteData(Lidgren.Network.NetOutgoingMessage Message) {
			Message.Write(Parent.ToString());
			this.Code.Write(Message);
		}

		protected override void ReadData(Lidgren.Network.NetIncomingMessage Message) {
			if (!Guid.TryParse(Message.ReadString(), out Parent)) {
				throw new Exception("Could not parse parent guid for block type");
			}
			Code.Read(Message);
		}

		protected override void ExecuteMessage() {
			Resource res = Package.FindResource(Parent);
			if (res == null) {
				throw new Exception("Could not find parent resource for texture");
			}
			((Folder)res).Add(Code);

			new AddCode(Code, Parent).Forward();
		}
	}
}
