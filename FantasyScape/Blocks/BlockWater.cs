using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape {
	class BlockWater : Block {
		float level;

		static float minLevel = 0.01f;
		public BlockWater() {
			level = 0.95f;
			BlockID = 1;
		}
		public override void draw(float x, float y, float z, World world) {
			if (level > minLevel) {
				draw(x, y, z, world,
						Textures.Water,
						Textures.Water,
						Textures.Water,
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

		public override void update(int x, int y, int z, World world) {
			/*moveDown(x, y, z - 1, world);
			if (level > minLevel) {
				moveTo(x + 1, y, z, world);
				moveTo(x - 1, y, z, world);
				moveTo(x, y + 1, z, world);
				moveTo(x, y - 1, z, world);
			}*/
		}

		public override void postUpdate(int x, int y, int z, World world) {
			//if (level <= minLevel) {
			//	world.removeBlock(x, y, z);
			//}
			base.postUpdate(x, y, z, world);
		}

		public void moveDown(int x, int y, int z, World world) {
			if (!world.isSolid(x, y, z)) {
				if (world.blockAt(x, y, z) == null) {
					BlockWater b = new BlockWater();
					world.addBlock(x, y, z, b);
					b.level = level;
					level = 0;
				} else {
					Block b = world.blockAt(x, y, z);
					if (b.BlockID == 1) {
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
						}
					}
				}
			}
		}

		private void moveTo(int x, int y, int z, World world) {
			if (!world.isSolid(x, y, z)) {
				if (world.blockAt(x, y, z) == null) {
					BlockWater b = new BlockWater();
					world.addBlock(x, y, z, b);
					b.level = level / 4.0f;
					level = level * 3.0f / 4.0f;
				} else {
					Block b = world.blockAt(x, y, z);
					if (b.BlockID == 1) {
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
						}
					}
				}
			}
		}
	}
}
