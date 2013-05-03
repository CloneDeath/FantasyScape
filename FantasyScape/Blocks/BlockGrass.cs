using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape {
	class BlockGrass : Block {
		public override void draw(float x, float y, float z, World world) {
			draw(x, y, z, world,
					Textures.GetTexture("Grass"),
					Textures.GetTexture("Dirt"),
					Textures.GetTexture("Dirt"));
		}
	}
}
