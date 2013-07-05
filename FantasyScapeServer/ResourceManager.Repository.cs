using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSharp;
using System.IO;

namespace FantasyScape {
	partial class ResourceManager {
		static Repository Repo;
		private static void LoadRepository() {
			if (!Directory.Exists(Path.Combine(ResourceLocation, ".git"))) {
				Git.Init(ResourceLocation);
			}
			Repo = new Repository(ResourceLocation);
		}

		private static void SaveRepository() {
			Repo.Index.AddAll();

			if (Repo.Index.Status.AnyDifferences) {
				Repo.Commit("Automated FantasyScape Server Commit", new Author("FantasyScape Server", "none"));
			}
		}
	}
}
