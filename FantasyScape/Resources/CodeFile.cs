using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Lidgren.Network;

namespace FantasyScape.Resources {
	public class CodeFile : Resource {
		public string Code = "";
		public CodeLanguage Language;
		public CodeLocation ExecutionLocation;

		public override void Save(string path) {
			string CodePath = Path.Combine(path, GetIDString());

			XDocument doc = new XDocument();
			{
				XElement Base = new XElement("Code");
				{
					XElement Name = new XElement("Name", this.Name);
					Base.Add(Name);

					XElement Language = new XElement("Language", this.Language.ToString());
					Base.Add(Language);

					XElement Location = new XElement("Location", this.ExecutionLocation.ToString());
					Base.Add(Location);

					XElement Content = new XElement("Content", this.Code);
					Base.Add(Content);
				}
				doc.Add(Base);
			}
			doc.Save(CodePath + ".code");
		}

		public override void Load(string path) {
			if (!Guid.TryParse(Path.GetFileNameWithoutExtension(path), out this.ID)) {
				throw new Exception("Could not parse GUID: " + path);
			}

			XDocument doc = XDocument.Load(path);
			XElement Base = doc.FirstNode as XElement;
			if (Base == null || Base.Name != "Code") {
				throw new Exception("Expected 'Texture' as the base element for file: " + path);
			}

			List<XElement> CodeInfo = new List<XElement>(Base.Descendants());

			foreach (XElement info in CodeInfo) {
				switch (info.Name.ToString()) {
					case "Name":
						this.Name = info.Value;
						break;
					case "Language":
						this.Language = (CodeLanguage)Enum.Parse(typeof(CodeLanguage), info.Value);
						break;
					case "Location":
						this.ExecutionLocation = (CodeLocation)Enum.Parse(typeof(CodeLocation), info.Value);
						break;
					case "Content":
						this.Code = info.Value;
						break;
					default:
						throw new Exception("Unknown element in blocktype '" + info.Name + "'.");
				}
			}
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write(Language.ToString());
			Message.Write(ExecutionLocation.ToString());
			Message.Write(Code);
		}

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			Language = (CodeLanguage)Enum.Parse(typeof(CodeLanguage), Message.ReadString());
			ExecutionLocation = (CodeLocation)Enum.Parse(typeof(CodeLocation), Message.ReadString());
			Code = Message.ReadString();
		}

	}
}
