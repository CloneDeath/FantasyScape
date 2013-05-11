using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape {
	class MapGenerator {
		PerlinMap pm;
		int XSize, YSize, ZSize;

		Block[,,] blocks;
		float[,] Heightmap;
		World world;

		public MapGenerator(int x, int y, int z, World w){
			pm = new PerlinMap();
			XSize = x;
			YSize = y;
			ZSize = z;
			blocks = new Block[XSize,YSize,ZSize];
			Heightmap = new float[XSize,ZSize];
			world = w;
		}

		public Block[,,] generateTerrain() {
			GenerateBase();
			GenerateWater();

			return blocks;
		}

		private int WaterLevel = 90;

		private void GenerateWater() {
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = (int)Heightmap[x,y]; z <= WaterLevel; z++) {
						blocks[x,y,z] = new Block("Water");
						if (z == WaterLevel) {
							Block b = blocks[x,y,z];
							if (!world.updateBlocks.Contains(b)) {
								world.updateBlocks.Add(b);
								world.updateLocations.Add(new int[] { x, y, z });
							}
						}
					}
				}
			}
		}

		private void GenerateBase() {
			//Generate
			const int variability = 20;
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
					blocks[x, y, 0] = new Block("Dirt");
					for (int z = 1; z < ZSize; z++) {
						if (z < (int)Heightmap[x, y]) {
							if (z > (int)Heightmap[x, y] - (5 + ran.Next(3))) {
								blocks[x, y, z] = new Block("Dirt");
							} else {
								blocks[x, y, z] = new Block("Granite");
							}
						}

						if (z == (int)Heightmap[x, y]) {
							blocks[x, y, z] = new Block("Grass");
						}
					}
				}
			}
		}
	}
}
