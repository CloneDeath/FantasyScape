using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class BlockTypes {
		static BlockType ErrorBlock = new BlockType();

		static BlockTypes() {
			ErrorBlock.ID = Guid.Empty;
			ErrorBlock.Liquid = false;
			ErrorBlock.Name = "Error";
		}

		static List<BlockType> Types = new List<BlockType>();

		public static BlockType GetBlockType(string BlockTypeName) {
			foreach (BlockType b in Types) {
				if (b.Name == BlockTypeName) {
					return b;
				}
			}
			return ErrorBlock;
		}

		public static List<BlockType> GetAll() {
			return Types;
		}

		public static bool Exists(string type) {
			return GetBlockType(type) != ErrorBlock;
		}

		public static void Remove(string block) {
			int found = -1;
			for (int i = 0; i < Types.Count; i++) {
				if (Types[i].Name == block) {
					found = i;
				}
			}

			if (found != -1) {
				Types.RemoveAt(found);
			}
			Chunk.DirtyAll = true;
		}
	}
}
