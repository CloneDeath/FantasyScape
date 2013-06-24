using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FantasyScape.Resources {
	public partial class Package : Folder {
		public static Dictionary<Guid, Package> Packages = new Dictionary<Guid, Package>();

		public static void LoadAll(string ResourceLocation) {
			string[] SubDirs = Directory.GetDirectories(ResourceLocation);
			foreach (string dir in SubDirs) {
				string GuidString = dir.Substring(ResourceLocation.Length + 1);

				Guid PackageGuid;
				if (Guid.TryParse(GuidString, out PackageGuid)) {
					Package pak = new Package(PackageGuid);
					pak.Load(dir);
					Packages.Add(pak.ID, pak);
				}
			}
		}

		public static void SaveAll(string ResourceLocation) {
			foreach (Package pak in Packages.Values) {
				pak.Save(ResourceLocation);
			}
		}
	}
}
