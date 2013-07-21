using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.RealmManagement.RealmDrawing {
	public class RealmRenderer {
		Realm Realm;
		Dictionary<Vector3i, RenderChunk> Chunks = new Dictionary<Vector3i, RenderChunk>();

		public RealmRenderer(Realm Realm) {
			this.Realm = Realm;
			Realm.OnBlockChanged += OnBlockChanged;
		}

		public void OnBlockChanged(Vector3i Position) {
			Vector3i ChunkCoords = Position / RenderChunk.Size;

			RenderChunk Target;
			if (!Chunks.TryGetValue(ChunkCoords, out Target)) {
				Target = new RenderChunk(Realm, ChunkCoords);
				Chunks[ChunkCoords] = Target;
			}

			Target.Dirty = true;
		}

		public void Draw() {
			foreach (RenderChunk chunk in Chunks.Values) {
				chunk.Draw();
			}
		}

		public void DirtyAll() {
			throw new NotImplementedException();
		}

		public int ChunkCount() {
			return Chunks.Count;
		}
	}
}
