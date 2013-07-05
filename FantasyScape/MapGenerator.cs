using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FantasyScape {
	class MapGenerator {
		public Chunk GenerateTerrain(Vector3i Location) {
			Chunk chunk = new Chunk(Location);
			foreach (WorldGenerator gen in Generators) {
				gen.GenerateChunk(chunk);
			}
			return chunk;
		}

		List<WorldGenerator> Generators = new List<WorldGenerator>();
		internal void Clear() {
			Generators.Clear();
		}

		internal void Add(List<WorldGenerator> list) {
			Generators.AddRange(list);
		}
	}
}
