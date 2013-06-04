using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace FantasyScape.Resources {
	public partial class Package : Resource {
		public string Name = "None";
		public List<Guid> References = new List<Guid>();

		public Package(Guid UID) {
			this.ID = UID;
		}

		private void Load(string dir) {
			LoadPackageInfo(dir);
			LoadChildren(dir);
		}

		private void LoadPackageInfo(string dir) {
			XDocument doc = XDocument.Load(new StreamReader(Path.Combine(dir, "package.info")));

			XElement Package = doc.Descendants("Package").First();

			if (Package == null) {
				throw new Exception("Malformed package. Expected 'Package' element.");
			}

			List<XElement> PackageInfo = new List<XElement>(Package.Descendants());

			foreach (XElement info in PackageInfo) {
				switch (info.Name.ToString()) {
					case "Name":
						this.Name = info.Value;
						break;
					case "Reference":
						Guid ReferenceGuid;
						if (Guid.TryParse(info.Value, out ReferenceGuid)) {
							References.Add(ReferenceGuid);
						}
						break;
					default:
						throw new Exception("Unknown element in package '" + info.Name + "'.");
				}
			}
		}

		private void LoadChildren(string dir) {
			string[] Files = Directory.GetFiles(dir);
			foreach (string file in Files){
				string FileType = Path.GetExtension(file);
				switch (FileType) {
					case ".tex":
						FSTexture tex = new FSTexture(file);
						break;

					case ".block":
						BlockType blk = new BlockType(file);
						break;
				}
			}

			string[] Children = Directory.GetDirectories(dir);
			foreach (string child in Children) {
				LoadChildren(child);
			}
		}
		
	}
}
