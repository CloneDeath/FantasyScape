using System;
using System.Collections.Generic;
using FantasyScape.Resources;
using System.IO;
using GitSharp;


namespace FantasyScape {
	partial class ResourceManager {
		const string ResourceLocation = @".\Data";
		Repository Repo;

		public void Load() {
			LoadRepository();

			foreach (string File in Directory.GetFiles(ResourceLocation)) {
				string Type = Path.GetExtension(File);
				if (Type == ".info") {
					Game.ServerInfo = (ServerInfo)Resource.Load(File, typeof(ServerInfo));
					Game.ServerInfo.ID = Guid.Parse(Path.GetFileNameWithoutExtension(File));
				} else {
					throw new Exception("Unrecognized type: '" + Type + "'");
				}
			}

			foreach (string Dir in Directory.GetDirectories(ResourceLocation)) {
				Guid ID;
				if (!Guid.TryParse(Path.GetFileName(Dir), out ID)){
					continue;
				}
				Package pack = Package.Load(Dir);
				pack.ID = ID;
				Package.AddPackage(pack);
			}
		}

		public void Save() {
			Game.ServerInfo.Save(ResourceLocation);			
			Package.SaveAll(ResourceLocation);

			SaveRepository();
		}

		private void LoadRepository() {
			if (!Directory.Exists(Path.Combine(ResourceLocation, ".git"))) {
				Git.Init(ResourceLocation);
			}
			Repo = new Repository(ResourceLocation);
		}

		private void SaveRepository() {
			Repo.Index.AddAll();

			if (Repo.Index.Status.AnyDifferences) {
				Repo.Commit("Automated FantasyScape Server Commit", new Author("FantasyScape Server", "none"));
			}
		}
	}
}
