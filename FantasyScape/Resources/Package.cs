using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using FantasyScape.NetworkMessages;
using Lidgren.Network;
using FantasyScape.CodeCompilation;
using System.Xml.Serialization;

namespace FantasyScape.Resources {
	[XmlRoot("Package")]
	public partial class Package : Folder {
		public List<Guid> References = new List<Guid>();
		public CodeManager CodeManager = new CodeManager();

		public Package() {
			this.ID = Guid.NewGuid();
		}

		public Package(Guid UID) {
			this.ID = UID;
		}

		public override void Save(string path) {
			base.Save(Path.Combine(path, "package.info"));
			this.SaveChildren(path);
		}

		public static new Package Load(string path) {
			Package dir = (Package)Resource.Load(Path.Combine(path, "package.info"), typeof(Package));
			dir.LoadChildren(path);
			return dir;
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
			Package.TriggerOnChangeEvent();
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

		public static event OnChangeEvent OnChange;
		public static void TriggerOnChangeEvent() {
			if (OnChange != null) {
				OnChange(null, null);
			}
		}

		private void Recompile() {
			List<CodeFile> codes = new List<CodeFile>();
			foreach (Resource child in this.GetAllChildren()) {
				if (child is CodeFile) {
					codes.Add(child as CodeFile);
				}
			}

			CodeManager.Compile(codes);
		}

		public static Package GetPackage(Guid guid) {
			return Packages[guid];
		}

		internal static Resource FindResourceByName(string ResourceName) {
			foreach (Package pkg in Packages.Values) {
				foreach (Resource res in pkg.GetAllChildren()) {
					if (res.Name == ResourceName) {
						return res;
					}
				}
			}
			return null;
		}
	}
}
