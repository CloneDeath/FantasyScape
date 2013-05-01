using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;

namespace FantasyScape {
	class Textures {
		public static Texture Dirt = new Texture("./Data/Dirt.png");
		public static Texture Grass = new Texture("./Data/Grass.png");
		public static Texture Granite = new Texture("./Data/Granite.png");
		public static Texture Water = new Texture("./Data/Water.png");

		public static void Initialize() {
			//Just calling this initilizes all the static parameters. We might put something here anyways though, one day...
		}
	}
}
