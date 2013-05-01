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

		private int WaterLevel = 85;

		private void GenerateWater() {
			for (int x = 0; x < XSize; x++) {
				for (int z = 0; z < ZSize; z++) {

					for (int y = (int)Heightmap[x,z]; y <= WaterLevel; y++) {
						blocks[x,y,z] = new BlockWater();
						if (y == WaterLevel) {
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
			for (int z = 0; z < ZSize; z++) {
				for (int x = 0; x < XSize; x++) {
					Heightmap[x, z] = ((float)pm.noise(3 * x / 256.0, 3 * z / 256.0, 100) * 50) + 100;
				}
			}

			Random ran = new Random();

			//Create Terrain
			for (int x = 0; x < XSize; x++) {
				for (int z = 0; z < ZSize; z++) {
					blocks[x, 0, z] = new BlockDirt();
					for (int y = 1; y < YSize; y++) {
						if (y < (int)Heightmap[x, z]) {
							if (y > (int)Heightmap[x, z] - (5 + ran.Next(3))) {
								blocks[x, y, z] = new BlockDirt(); //Dirt
							} else {
								blocks[x, y, z] = new BlockGranite(); //Granite
							}
						}

						if (y == (int)Heightmap[x, z]) {
							blocks[x, y, z] = new BlockGrass();
						}
					}
				}
			}
		}
	}
}
