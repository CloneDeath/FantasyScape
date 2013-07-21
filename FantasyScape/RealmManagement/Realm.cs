using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.Resources;

namespace FantasyScape.RealmManagement {
	public class Realm : Resource {
		Dictionary<Vector3i, Block> Blocks = new Dictionary<Vector3i,Block>();
		
		public Action<Vector3i> OnBlockChanged;

		public Block GetBlock(Vector3i Location) {
			Block Return;
			if (Blocks.TryGetValue(Location, out Return)) {
				return Return;
			} else {
				return null;
			}
		}

		public void SetBlock(Vector3i Location, Block Data) {
			Blocks[Location] = Data;
			if (OnBlockChanged != null) {
				OnBlockChanged(Location);
			}
		}

		public override void Save(string path) {
			throw new NotImplementedException();
		}

		public override void Load(string path) {
			throw new NotImplementedException();
		}

		public override void SendUpdate() {
			throw new NotImplementedException();
		}
	}
}
