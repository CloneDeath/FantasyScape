using System;
using System.Collections.Generic;
using FantasyScape.Resources;


namespace FantasyScape {
	partial class ResourceManager {
		const string ResourceLocation = @".\Data";

		public static void Load() {
			LoadRepository();

			Game.ServerInfo.Load(ResourceLocation);
			Package.LoadAll(ResourceLocation);
		}

		internal static void Save() {
			Game.ServerInfo.Save(ResourceLocation);			
			Package.SaveAll(ResourceLocation);

			SaveRepository();
		}
	}
}
