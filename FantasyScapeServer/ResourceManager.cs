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

			Game.ServerInfo.Load(ResourceLocation);
			Package.LoadAll(ResourceLocation);
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
