using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape {
	class BlockWater : Block {
		float level;

		static float minLevel = 0.01f;
		public BlockWater() : base("Water") {
		    level = 0.95f;
		}
		public override void draw(float x, float y, float z, World world) {
			if (level > minLevel) {
				draw(x, y, z, world,
						Textures.GetTexture("Water"),
						Textures.GetTexture("Water"),
						Textures.GetTexture("Water"),
						level);
			}
		}

		public override bool isSolid() {
			if (level >= 0.9) {
				return true;
			} else {
				return false;
			}
		}

		bool WaterUpdated = false;

		public override void update(int x, int y, int z, World world) {
			WaterUpdated = moveDown(x, y, z - 1, world);
			if (level > minLevel) {
				WaterUpdated |= moveTo(x + 1, y, z, world);
				WaterUpdated |= moveTo(x - 1, y, z, world);
				WaterUpdated |= moveTo(x, y + 1, z, world);
				WaterUpdated |= moveTo(x, y - 1, z, world);
			}
		}

		public override void postUpdate(int x, int y, int z, World world) {
			if (!WaterUpdated) {
				world.removeUpdate(x, y, z);
			}
			if (level <= minLevel) {
				world.removeBlock(x, y, z);
			}
		}

		public bool moveDown(int x, int y, int z, World world) {
			if (!world.isSolid(x, y, z)) {
				if (world.blockAt(x, y, z) == null) {
					BlockWater b = new BlockWater();
					world.addBlock(x, y, z, b);
					b.level = level;
					level = 0;
					return true;
				} else {
					Block b = world.blockAt(x, y, z);
					if (b.BlockType.Name == "Water") {
						BlockWater bw = (BlockWater)b;
						if (bw.level < 1.0f) {
							float diff = (1.0f - bw.level);
							if (diff > level) {
								bw.level += level;
								level = 0;
								world.removeBlock(x, y, z + 1);
							} else {
								bw.level += diff;
								level -= diff;
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool moveTo(int x, int y, int z, World world) {
			if (!world.isSolid(x, y, z)) {
				if (world.blockAt(x, y, z) == null) {
					BlockWater b = new BlockWater();
					world.addBlock(x, y, z, b);
					b.level = level / 4.0f;
					level = level * 3.0f / 4.0f;
					return true;
				} else {
					Block b = world.blockAt(x, y, z);
					if (b.BlockType.Name == "Water") {
						BlockWater bw = (BlockWater)b;
						if (bw.level < level) {
							float diff = (level - bw.level) / 4.0f;
							float cgive = level / 4.0f;
							if (cgive > diff) {
								bw.level += diff;
								level -= diff;
							} else {
								bw.level += cgive;
								level -= cgive;
							}
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
