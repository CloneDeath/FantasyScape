﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class BlockTypes : Resource {
		static List<BlockType> Types = new List<BlockType>();

		public void Load() {
			AddBlockType("Dirt", "Dirt", false);
			AddBlockType("Granite", "Granite", false);
			AddBlockType("Grass", "Grass", false);
			AddBlockType("Water", "Water", true);
		}

		private static void AddBlockType(string BlockTypeName, string TextureName, bool Liquid) {
			BlockType b = new BlockType();
			b.Name = BlockTypeName;
			b.Texture = TextureName;
			b.Liquid = Liquid;
			Types.Add(b);
		}

		public static BlockType GetBlockType(string BlockTypeName) {
			foreach (BlockType b in Types) {
				if (b.Name == BlockTypeName) {
					return b;
				}
			}
			return null;
		}

		static bool SentRequest = false;
		public static int Count = -1;
		internal static bool Ready() {
			if (!SentRequest) {
				SentRequest = true;
				RequestMessage msg = new RequestMessage(RequestType.BlockTypes);
				msg.Send();
			}
			
			return Count == Types.Count;
		}

		public static void AddBlockType(BlockType b) {
			Types.Add(b);
		}

		public static List<BlockType> GetAll() {
			return Types;
		}

		public static bool Exists(string type) {
			return GetBlockType(type) != null;
		}
	}
}
