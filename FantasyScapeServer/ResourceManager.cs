using System;
using System.Collections.Generic;
using FantasyScape.Resources;


namespace FantasyScape {
	partial class ResourceManager {
		public static void Load() {
			LoadRepository();
			Package.LoadAll(ResourceLocation);
		}
	}
}
