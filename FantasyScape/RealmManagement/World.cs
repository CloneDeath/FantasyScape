using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FantasyScape.RealmManagement.RealmDrawing;
using FantasyScape.RealmManagement.ChunkRequester;

namespace FantasyScape.RealmManagement {
	public class World {
		public Realm Realm;
		public RealmRenderer Renderer;
		public RealmRequester Requester;
		public MapGenerator MapGenerator;

		public World() {
			Realm = new Realm();
			Renderer = new RealmRenderer(Realm);
			MapGenerator = new MapGenerator(Realm);
			Requester = new RealmRequester(Realm);
		}

		public Block this[Vector3i Location] {
			get {
				return Realm.GetBlock(Location);
			}

			set {
				Realm.SetBlock(Location, value);
			}
		}

		internal void Draw() {
			Renderer.Draw();
		}

		public void Update() {
			Requester.HandleRequests();
		}

		public void Request(Vector3i Location) {
			Requester.QueueRequest(Location);
		}

		public void ClearWorldGen() {
			MapGenerator.Clear();
		}

		public void AddWorldGen(List<WorldGenerator> temp) {
			MapGenerator.Add(temp);
		}

		public void DirtyAll() {
			Renderer.DirtyAll();
		}
	}
}
