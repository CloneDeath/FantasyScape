using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.RealmManagement.WorldGeneration {
	public class Sector {
		public static Vector3i Size = new Vector3i(16, 16, 16);
		public Vector3i ChunkLocation;
		private Realm Realm;

		public Sector(Realm realm, Vector3i Location) {
			this.Realm = realm;
			this.ChunkLocation = Location;
		}


		public Block this[Vector3i Location] {
			get {
				return Realm.GetBlock((ChunkLocation * Size) + Location);
			}

			set {
				Realm.SetBlock((ChunkLocation * Size) + Location, value);
			}
		}
	}
}
