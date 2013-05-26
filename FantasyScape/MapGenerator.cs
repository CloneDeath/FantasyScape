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

		public MapGenerator(World World) {
			pm = new PerlinMap();
			this.World = World;
			Heightmap = new float[XSize,ZSize];
		}

		public void GenerateTerrain() {
			GenerateBase();
			GenerateWater();
		}

		private const int WaterLevel = 90;

		private void GenerateWater() {
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = (int)Heightmap[x,y] + 1; z <= WaterLevel; z++) {
						World[x,y,z] = new Block("Water");
						if (z == WaterLevel) {
							World.AddUpdate(x, y, z);
						}
					}
				}
			}
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
					World[x, y, 0] = new Block("Dirt");
					for (int z = 1; z < ZSize; z++) {
						if (z < (int)Heightmap[x, y]) {
							if (z > (int)Heightmap[x, y] - (5 + ran.Next(3))) {
								World[x, y, z] = new Block("Dirt");
							} else {
								World[x, y, z] = new Block("Granite");
							}
						}

						if (z == (int)Heightmap[x, y]) {
							World[x, y, z] = new Block("Grass");
						}
					}
				}
			}
		}
	}
}
