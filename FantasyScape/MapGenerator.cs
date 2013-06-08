using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FantasyScape {
	class MapGenerator {
		Guid GrassGuid;
		Guid WaterGuid;
		Guid DirtGuid;
		Guid GraniteGuid;

		Bitmap current_heightmap;

		public MapGenerator() {
			//TODO: Remove this terrible temporary hack. Will require moving terrain generation code to in-game.
			string Grass = "{6F4C26B5-BF84-41F2-A166-BA6C3FB3C47E}";
			Guid.TryParse(Grass, out GrassGuid);

			string Water = "{3484DF8C-A6E7-4D40-86AD-63EBD094D453}";
			Guid.TryParse(Water, out WaterGuid);

			string Dirt = "{E632A3E7-F377-4D60-9E89-20EC068AF3A3}";
			Guid.TryParse(Dirt, out DirtGuid);

			string Granite = "{D7C42849-C769-476A-B7B1-2EE4A806D7A5}";
			Guid.TryParse(Granite, out GraniteGuid);
		}

		public Chunk GenerateTerrain(int X, int Y, int Z) {
			Chunk ret = new Chunk();
			current_heightmap = HeightMap.GetHeightmap(X, Y);
			GenerateBase(ret, Z);
			GenerateWater(ret, Z);
			return ret;
		}

		private void GenerateBase(Chunk chunk, int ChunkZ) {

			Random ran = new Random();

			//Create Terrain
			for (int x = 0; x < Chunk.Size; x++) {
				for (int y = 0; y < Chunk.Size; y++) {
					for (int z = 0; z < Chunk.Size; z++) {
						if (z + (ChunkZ * Chunk.Size) < current_heightmap.GetPixel(x, y).R) {
							if (z + (ChunkZ * Chunk.Size) > current_heightmap.GetPixel(x, y).R - (int)(5 + ran.Next(3))) {
								chunk[x, y, z] = new Block(DirtGuid);
							} else {
								chunk[x, y, z] = new Block(GraniteGuid);
							}
						}

						if (z + (ChunkZ * Chunk.Size) == current_heightmap.GetPixel(x, y).R) {
							chunk[x, y, z] = new Block(GrassGuid);
						}
					}
				}
			}
		}

		private const int WaterLevel = 90;
		private void GenerateWater(Chunk current, int ChunkZ) {
			for (int x = 0; x < Chunk.Size; x++) {
				for (int y = 0; y < Chunk.Size; y++) {
					for (int z = (int)current_heightmap.GetPixel(x, y).R + 1; z <= WaterLevel; z++) {
						current[x, y, z - (ChunkZ * Chunk.Size)] = new Block(WaterGuid);
					}
				}
			}
		}
	}
}
