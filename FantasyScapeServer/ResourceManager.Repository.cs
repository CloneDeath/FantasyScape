using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSharp;
using System.IO;

namespace FantasyScape {
	partial class ResourceManager {
		const string ResourceLocation = @".\Data";
		static Repository Repo;
		private static void LoadRepository() {
			if (!Directory.Exists(ResourceLocation)) {
				Git.Init(ResourceLocation);
			}
			Repo = new Repository(ResourceLocation);
		}
	}
}
