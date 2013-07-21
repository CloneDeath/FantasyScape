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

		private Dictionary<Vector3i, Sector> Mappings = new Dictionary<Vector3i,Sector>();

		public MapGenerator(Realm realm) {
			this.Realm = realm;
		}

		public void GenerateSector(Vector3i Location) {
			Sector chunk = new Sector(Realm, Location);
			foreach (WorldGenerator gen in Generators) {
				gen.Generate(chunk);
			}
			Mappings.Add(Location, chunk);
		}

		public bool Exists(Vector3i Location) {
			return Mappings.ContainsKey(Location);
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
