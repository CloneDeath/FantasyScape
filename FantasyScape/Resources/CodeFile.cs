using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Lidgren.Network;
using FantasyScape.NetworkMessages.Code;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace FantasyScape.Resources {
	[XmlRoot("Code")]
	public class CodeFile : Resource {

		public string Source = "";
		public CodeLanguage Language;
		public CodeLocation Location;

		public List<CompilerError> Errors = new List<CompilerError>();

		public CodeFile() {
			ID = Guid.NewGuid();
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write(Language.ToString());
			Message.Write(Location.ToString());
			Message.Write(Source);
		}

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			Language = (CodeLanguage)Enum.Parse(typeof(CodeLanguage), Message.ReadString());
			Location = (CodeLocation)Enum.Parse(typeof(CodeLocation), Message.ReadString());
			Source = Message.ReadString();
		}


		public override void SendUpdate() {
			new UpdateCode(this).Send();
		}

		internal void ClearErrors() {
			Errors.Clear();
		}

		internal void AddError(CompilerError error) {
			Errors.Add(error);
		}
	}
}
