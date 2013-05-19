﻿using System;
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
			Texture Dirt = new Texture(filename);
			Dirt.Name = name;

			TextureList.Add(Dirt);
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
				Console.WriteLine("Received Duplicate Texture Name: " + nettex.Name);
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
	}
}
