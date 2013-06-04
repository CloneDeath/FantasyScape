using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace FantasyScape.Resources {
	public partial class Package : Folder {
		public List<Guid> References = new List<Guid>();

		public Package(Guid UID) {
			this.ID = UID;
		}

		public override void Load(string dir) {
			LoadPackageInfo(dir);
			base.LoadChildren(dir);
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

		internal static bool Ready() {
			throw new NotImplementedException();
		}
	}
}
