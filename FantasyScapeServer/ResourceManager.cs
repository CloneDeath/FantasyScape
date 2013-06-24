using System;
using System.Collections.Generic;
using FantasyScape.Resources;


namespace FantasyScape {
	partial class ResourceManager {
		const string ResourceLocation = @".\Data";

		public static void Load() {
			LoadRepository();
			Package.LoadAll(ResourceLocation);
		}

		internal static void Save() {
			Package.SaveAll(ResourceLocation);
			SaveRepository();
		}
	}
}
