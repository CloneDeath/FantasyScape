using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using FantasyScape.NetworkMessages;
using FantasyScape.Resources;

namespace FantasyScape {
	public class BlockTypes {
		static BlockType ErrorBlock = new BlockType();

		static BlockTypes() {
			ErrorBlock.ID = Guid.Empty;
			ErrorBlock.Liquid = false;
			ErrorBlock.Name = "Error";
		}

		internal static BlockType GetBlockType(Guid BlockTypeID) {
			return Package.FindResource(BlockTypeID) as BlockType ?? ErrorBlock;
		}
	}
}
