using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using FantasyScape.NetworkMessages;
using Lidgren.Network;

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

		static bool RequestSent = false;
		internal static bool Ready() {
			if (!RequestSent) {
				new RequestMessage(RequestType.Packages).Send();
				RequestSent = true;
			}

			return true;
		}

		internal override void Write(NetOutgoingMessage Message) {
			base.Write(Message);
			Message.Write((Int32)References.Count);
			foreach (Guid UUID in References) {
				Message.Write(UUID.ToString());
			}
		}

		internal override void Read(NetIncomingMessage Message) {
			base.Read(Message);
			int RefCount = Message.ReadInt32();
			References = new List<Guid>();
			for (int i = 0; i < RefCount; i++) {
				Guid UUID;
				if (!Guid.TryParse(Message.ReadString(), out UUID)) {
					throw new Exception("Could not parse reference GUID");
				} else {
					References.Add(UUID);
				}
			}
		}

		internal static void Add(Package package) {
			Packages.Add(package.ID, package);
		}

		public static Resource FindResource(Guid ResourceID) {
			foreach (Package pkg in Packages.Values) {
				Resource ret = pkg.GetResource(ResourceID);
				if (ret != null) {
					return ret;
				}
			}

			return null;
		}
	}
}
