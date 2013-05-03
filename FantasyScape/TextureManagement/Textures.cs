using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;

namespace FantasyScape {
	public class Textures {
		static List<NetTexture> Textures = new List<NetTexture>();

		public static void ServerLoadTextures() {
			AddTexture("Data/Dirt.png", "Dirt");
			AddTexture("Data/Grass.png", "Grass");
			AddTexture("Data/Granite.png", "Granite");
			AddTexture("Data/Water.png", "Water");
		}

		private static void AddTexture(string filename, string name) {
			NetTexture Dirt = new NetTexture(filename);
			Dirt.Name = name;

			Textures.Add(Dirt);
		}

		public static Texture GetTexture(string name) {
			foreach (Texture t in Textures) {
				if (t.Name == name) {
					return t;
				}
			}
			return null;
		}

		public static void SendTextures(NetConnection netConnection, NetServer Server) {
			foreach (NetTexture tex in Textures) {
				tex.Send(netConnection, Server);
			}
		}

		public static void AddTexture(NetTexture nettex) {
			Textures.Add(nettex);
		}
	}
}
