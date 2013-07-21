using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FantasyScape.RealmManagement.WorldGeneration;
using FantasyScape.RealmManagement;

namespace FantasyScape {
	public class MapGenerator {
		private Realm Realm;

		public MapGenerator(Realm realm) {
			this.Realm = realm;
		}

		public Sector GenerateTerrain(Vector3i Location) {
			Sector chunk = new Sector(Realm, Location);
			foreach (WorldGenerator gen in Generators) {
				gen.Generate(chunk);
			}
			return chunk;
		}

		List<WorldGenerator> Generators = new List<WorldGenerator>();
		public void Clear() {
			Generators.Clear();
		}

		public void Add(List<WorldGenerator> list) {
			Generators.AddRange(list);
		}
	}
}
