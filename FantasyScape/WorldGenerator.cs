using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FantasyScape {
	public class WorldGenerator {
		Guid GrassGuid;
		Guid WaterGuid;
		Guid DirtGuid;
		Guid GraniteGuid;

		Bitmap current_heightmap;

		public WorldGenerator() {
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

		public virtual void GenerateChunk(Chunk chunk) {
			current_heightmap = HeightMap.GetHeightmap(chunk.Location.X, chunk.Location.Y);
			GenerateBase(chunk, chunk.Location.Z);
			GenerateWater(chunk, chunk.Location.Z);
		}

		private void GenerateBase(Chunk chunk, int ChunkZ) {
			Random ran = new Random();

			//Create Terrain
			for (int x = 0; x < Chunk.Size.X; x++) {
				for (int y = 0; y < Chunk.Size.Y; y++) {
					for (int z = 0; z < Chunk.Size.Z; z++) {
						Vector3i Location = new Vector3i(x, y, z);
						if (z + (ChunkZ * Chunk.Size.Z) < current_heightmap.GetPixel(x, y).R - 100) {
							if (z + (ChunkZ * Chunk.Size.Z) > current_heightmap.GetPixel(x, y).R - 100 - (int)(5 + ran.Next(3))) {
								chunk[Location] = new Block(DirtGuid);
							} else {
								chunk[Location] = new Block(GraniteGuid);
							}
						}

						if (z + (ChunkZ * Chunk.Size.Z) == current_heightmap.GetPixel(x, y).R - 100) {
							chunk[Location] = new Block(GrassGuid);
						}
					}
				}
			}
		}

		private const int WaterLevel = 0;
		private void GenerateWater(Chunk current, int ChunkZ) {
			for (int x = 0; x < Chunk.Size.X; x++) {
				for (int y = 0; y < Chunk.Size.Y; y++) {
					for (int z = (int)current_heightmap.GetPixel(x, y).R - 100 + 1; z <= WaterLevel; z++) {
						current[new Vector3i(x, y, z - (ChunkZ * Chunk.Size.Z))] = new Block(WaterGuid);
					}
				}
			}
		}
	}
}
