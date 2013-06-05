using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape {
	class MapGenerator {
		PerlinMap pm;
		int XSize { get { return World.XSize * Chunk.Size; } }
		int YSize { get { return World.YSize * Chunk.Size; } }
		int ZSize { get { return World.ZSize * Chunk.Size; } }
		float[,] Heightmap;
		World World;

		Guid GrassGuid;
		Guid WaterGuid;
		Guid DirtGuid;
		Guid GraniteGuid;

		public MapGenerator(World World) {
			//TODO: Remove this terrible temporary hack. Will require moving terrain generation code to in-game.
			string Grass = "{6F4C26B5-BF84-41F2-A166-BA6C3FB3C47E}";
			Guid.TryParse(Grass, out GrassGuid);

			string Water = "{3484DF8C-A6E7-4D40-86AD-63EBD094D453}";
			Guid.TryParse(Water, out WaterGuid);

			string Dirt = "{E632A3E7-F377-4D60-9E89-20EC068AF3A3}";
			Guid.TryParse(Dirt, out DirtGuid);

			string Granite = "{D7C42849-C769-476A-B7B1-2EE4A806D7A5}";
			Guid.TryParse(Granite, out GraniteGuid);


			pm = new PerlinMap();
			this.World = World;
			Heightmap = new float[XSize,ZSize];
		}

		public void GenerateTerrain() {
			GenerateBase();
			GenerateWater();
		}

		private void GenerateBase() {
			//Generate
			const int variability = 25;
			const int lowlevel = 100;
			for (int y = 0; y < YSize; y++) {
				for (int x = 0; x < XSize; x++) {
					Heightmap[x, y] = ((float)pm.noise(3 * x / 256.0, 3 * y / 256.0, 100) * variability) + lowlevel;
				}
			}

			Random ran = new Random();

			//Create Terrain
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					World[x, y, 0] = new Block(DirtGuid);
					for (int z = 1; z < ZSize; z++) {
						if (z < (int)Heightmap[x, y]) {
							if (z > (int)Heightmap[x, y] - (5 + ran.Next(3))) {
								World[x, y, z] = new Block(DirtGuid);
							} else {
								World[x, y, z] = new Block(GraniteGuid);
							}
						}

						if (z == (int)Heightmap[x, y]) {
							World[x, y, z] = new Block(GrassGuid);
						}
					}
				}
			}
		}

		private const int WaterLevel = 90;
		private void GenerateWater() {
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = (int)Heightmap[x, y] + 1; z <= WaterLevel; z++) {
						World[x, y, z] = new Block(WaterGuid);
						if (z == WaterLevel) {
							World.AddUpdate(x, y, z);
						}
					}
				}
			}
		}
	}
}
