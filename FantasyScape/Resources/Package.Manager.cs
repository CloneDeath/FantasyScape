using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FantasyScape.Resources {
	public partial class Package : Folder {
		private static Dictionary<Guid, Package> Packages = new Dictionary<Guid, Package>();

		public static List<Package> GetPackages() {
			return new List<Package>(Packages.Values);
		}

		public static void AddPackage(Package pkg) {
			Packages.Add(pkg.ID, pkg);
			TriggerOnChangeEvent();
		}

		public static void SaveAll(string ResourceLocation) {
			foreach (Package pak in Packages.Values) {
				pak.Save(ResourceLocation);
			}
		}

		public static void RecompilePackages() {
			foreach (Package pkg in Packages.Values) {
				pkg.Recompile();
			}
		}
	}
}
