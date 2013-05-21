using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class Textures : Resource{
		static Texture ErrorTexture = new Texture(@"Data\Error.png");
		static List<Texture> TextureList = new List<Texture>();
		static bool RequestSent = false;
		public static int Count = -1;

		public void Load() {
			AddTexture("Data/Dirt.png", "Dirt");
			AddTexture("Data/Grass.png", "Grass");
			AddTexture("Data/Granite.png", "Granite");
			AddTexture("Data/Water.png", "Water");
			AddTexture("Data/Player.png", "Player");
		}

		private static void AddTexture(string filename, string name) {
			Texture tex = new Texture(filename);
			tex.Name = name;

			TextureList.Add(tex);
		}

		public static Texture GetTexture(string name) {
			foreach (Texture t in TextureList) {
				if (t.Name == name) {
					return t;
				}
			}
			return ErrorTexture;
		}

		public static void AddTexture(Texture nettex) {
			if (!Exists(nettex.Name)) {
				TextureList.Add(nettex);
			} else {
				Texture oldtex = GetTexture(nettex.Name);
				oldtex.Image = nettex.Image;
			}
		}

		internal static bool Ready() {
			if (!RequestSent) {
				RequestMessage msg = new RequestMessage(RequestType.Textures);
				msg.Send();
				RequestSent = true;
			}

			return TextureList.Count == Count;
		}

		public static List<Texture> GetAll() {
			return TextureList;
		}

		public static bool Exists(string tex) {
			return GetTexture(tex) != ErrorTexture;
		}

		public static void Remove(string tex) {
			int found = -1;
			for (int i = 0; i < TextureList.Count; i++) {
				if (TextureList[i].Name == tex) {
					found = i;
				}
			}

			if (found != -1) {
				TextureList.RemoveAt(found);
			}
		}
	}
}
