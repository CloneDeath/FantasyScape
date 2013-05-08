using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Lidgren.Network;

namespace FantasyScape {
	public class World {
		Block[,,] blocks;
		public int XSize = 256;
		public int YSize = 256;
		public int ZSize = 256;
		List<Block> exposedBlocks;
		List<int[]> exposedLocations;

		public List<Block> updateBlocks;
		public List<int[]> updateLocations;


		public World() {
			updateBlocks = new List<Block>();
			updateLocations = new List<int[]>();		
		}

		public void refreshUpdateBlocks(int x, int y, int z) {
			addUpdate(x, y, z);
			addUpdate(x + 1, y, z);
			addUpdate(x - 1, y, z);
			addUpdate(x, y + 1, z);
			addUpdate(x, y - 1, z);
			addUpdate(x, y, z + 1);
			addUpdate(x, y, z - 1);
		}

		public void addUpdate(int x, int y, int z) {
			Block b = blockAt(x, y, z);
			if (b != null && !updateBlocks.Contains(b)) {
				updateBlocks.Add(b);
				updateLocations.Add(new int[] { x, y, z });
			}
		}

		public void removeUpdate(int x, int y, int z) {
			Block b = blockAt(x, y, z);
			if (b != null && updateBlocks.Contains(b)) {
				int rval = updateBlocks.IndexOf(b);
				updateBlocks.RemoveAt(rval);
				updateLocations.RemoveAt(rval);
			}
		}

		public void update(){
			List<Block> tblocks = new List<Block>();
			foreach (Block b in updateBlocks){
				tblocks.Add(b);
			}

			List<int[]> tlocs = new List<int[]>();
			foreach (int[] i in updateLocations){
				tlocs.Add(i);
			}
			for (int i = 0; i < tblocks.Count(); i++){
				int[] loc = tlocs[i];
				tblocks[i].update(loc[0], loc[1], loc[2], this);

				//System.out.println("X:"+loc[0]+" Y:"+loc[1]+" Z:"+loc[2]+" SIZE:"+tblocks.size() + " ID:"+tblocks.get(i).isSolid());
			}
		
			for (int i = 0; i < tblocks.Count(); i++){
				int[] loc = tlocs[i];
				tblocks[i].postUpdate(loc[0], loc[1], loc[2], this);

				//System.out.println("X:"+loc[0]+" Y:"+loc[1]+" Z:"+loc[2]+" SIZE:"+updateBlocks.size() + " ID:"+tblocks.get(i).isSolid());
			}
		
			//Console.WriteLine(tblocks.Count());
		}

		public void GenerateMap() {
			MapGenerator mg = new MapGenerator(XSize, YSize, ZSize, this);
			blocks = mg.generateTerrain();
		}

		public void GenerateFlatMap(){
			blocks = new Block[XSize,YSize,ZSize];
			for (int x = 0; x < XSize; x++){
				for (int y = 0; y < YSize/2; y++){
					for (int z = 0; z < ZSize; z++){
						blocks[x, y, z] = new Block("Dirt");
					}
				}
			}
		}

		public Block blockAt(int x, int y, int z) {
			if (x < 0 || x >= XSize || y < 0 || y >= YSize || z < 0 || z >= ZSize) {
				return null;
			}

			return blocks[x, y, z];
		}

		public void ExposeBlocksAt(int x, int y, int z) {
			exposeBlock(x + 1, y, z);
			exposeBlock(x - 1, y, z);
			exposeBlock(x, y + 1, z);
			exposeBlock(x, y - 1, z);
			exposeBlock(x, y, z + 1);
			exposeBlock(x, y, z - 1);
		}

		public void removeBlock(int x, int y, int z) {
			if (blockAt(x, y, z) != null) {
				int remi = exposedBlocks.IndexOf(blocks[x, y, z]);
				if (remi != -1) {
					exposedBlocks.RemoveAt(remi);
					exposedLocations.RemoveAt(remi);
				}
				removeUpdate(x, y, z);

				blocks[x, y, z] = null;

				ExposeBlocksAt(x, y, z);

				refreshUpdateBlocks(x, y, z);
			}
		}

		public void addBlock(int x, int y, int z, Block b) {
			if (blockAt(x, y, z) == null) {
				int remi = exposedBlocks.IndexOf(blocks[x, y, z]);
				if (remi == -1) {
					exposedBlocks.Add(b);
					exposedLocations.Add(new int[] { x, y, z });
				}

				blocks[x, y, z] = b;
				checkExposure(x + 1, y, z);
				checkExposure(x - 1, y, z);
				checkExposure(x, y + 1, z);
				checkExposure(x, y - 1, z);
				checkExposure(x, y, z + 1);
				checkExposure(x, y, z - 1);

				//addUpdate(x,y,z);
				refreshUpdateBlocks(x, y, z);
			}
		}

		public void addBlock(int x, int y, int z) {
			addBlock(x, y, z, new Block("Water"));
		}

		public void checkExposure(int x, int y, int z) {
			if (blockAt(x, y, z) != null) {
				if (isExposed(x, y, z)) {
					if (!exposedBlocks.Contains(blocks[x, y, z])) {
						exposedBlocks.Add(blocks[x, y, z]);
						exposedLocations.Add(new int[] { x, y, z });
					}
				} else {
					int remi = exposedBlocks.IndexOf(blocks[x, y, z]);
					if (remi != -1) {
						exposedBlocks.RemoveAt(remi);
						exposedLocations.RemoveAt(remi);
					}
				}
			}
		}

		public void exposeBlock(int x, int y, int z) {
			if (blockAt(x, y, z) != null) {
				if (!exposedBlocks.Contains(blocks[x, y, z])) {
					exposedBlocks.Add(blocks[x, y, z]);
					exposedLocations.Add(new int[] { x, y, z });
				}
			}
		}

		public void refreshExposedBlocks() {
			exposedBlocks = new List<Block>();
			exposedLocations = new List<int[]>();
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					for (int z = 0; z < ZSize; z++) {
						if (isExposed(x, y, z)) {
							exposedBlocks.Add(blocks[x, y, z]);
							exposedLocations.Add(new int[] { x, y, z });
						}
					}
				}
			}
		}

		public void draw(Player p) {
			int ViewDistance = 100;
			for (int i = 0; i < exposedBlocks.Count(); i++) {
				int[] loc = exposedLocations[i];
				if (Math.Abs(p.xpos - loc[0]) < ViewDistance &&
						Math.Abs(p.ypos - loc[1]) < ViewDistance &&
						Math.Abs(p.zpos - loc[2]) < ViewDistance) {
					Vector3 box = new Vector3(loc[0] + 0.5f, loc[1] + 0.5f, loc[2] + 0.5f);
					if (p.frustum.pointInFrustum(box) != Frustum.OUTSIDE) {
						exposedBlocks[i].draw(loc[0], loc[1], loc[2], this);
						//Console.WriteLine("x:"+loc[0]+" y:"+loc[1]+" z:"+loc[2]);
					}
				}
			}
		}

		public bool isExposed(float x, float y, float z) {
			int xat = (int)x;
			int yat = (int)y;
			int zat = (int)z;

			if (xat >= XSize || xat < 0 || zat >= ZSize || zat < 0 || yat < 0 || yat >= YSize) {
				return false;
			}

			if (blocks[xat, yat, zat] == null) {
				return false;
			}

			//if (xat == XSize-1 || xat == 0 || zat == ZSize-1 || zat == 0 || yat == 0 || yat == YSize-1){
			//return false;
			//}


			if (!isSolid(xat, yat + 1, zat) || !isSolid(xat + 1, yat, zat) ||
					!isSolid(xat - 1, yat, zat) || !isSolid(xat, yat, zat + 1) ||
					!isSolid(xat, yat, zat - 1) || !isSolid(xat, yat - 1, zat)) {
				return true;
			} else {
				return false;
			}
		}

		public bool isSolid(double x, double y, double z) {
			int xat = (int)x;
			int yat = (int)y;
			int zat = (int)z;

			if (xat >= XSize || xat < 0 || yat >= YSize || yat < 0 || zat < 0) {
				return true;
			}

			if (zat >= ZSize) {
				return false;
			}


			if (blocks[xat, yat, zat] == null)
				return false;
			else {
				return blocks[xat, yat, zat].isSolid();
			}

		}

		public void SendWorldSize(NetConnection netConnection, NetServer Server) {
			NetOutgoingMessage nom = Server.CreateMessage();

			nom.Write("WorldSize");
			nom.Write(XSize);
			nom.Write(YSize);
			nom.Write(ZSize);

			Server.SendMessage(nom, netConnection, NetDeliveryMethod.ReliableUnordered);
		}

		public void SendBlockLayers(NetConnection netConnection, NetServer Server) {
			for (int x = 0; x < XSize; x++) {
				for (int y = 0; y < YSize; y++) {
					NetOutgoingMessage nom = Server.CreateMessage();
					nom.Write("BlockLayer");
					nom.Write(x);
					nom.Write(y);
					bool FoundOne = false;
					for (int z = 0; z < ZSize; z++) {
						Block b = blocks[x, y, z];
						if (b != null) {
							nom.Write(z);
							b.Write(nom);
							FoundOne = true;
						}
					}
					if (FoundOne) {
						Server.SendMessage(nom, netConnection, NetDeliveryMethod.ReliableUnordered);
					}
				}
			}
		}

		private void ReceiveWorldSize(NetIncomingMessage Message) {
			XSize = Message.ReadInt32();
			YSize = Message.ReadInt32();
			ZSize = Message.ReadInt32();

			blocks = new Block[XSize, YSize, ZSize];
			Current = State.SendingLayersRequest;
		}

		int LayerCount = 0;
		private void ReceiveBlockLayer(NetIncomingMessage Message) {
			int x = Message.ReadInt32();
			int y = Message.ReadInt32();
			while (Message.PositionInBytes < Message.LengthBytes) {
				Block b = new Block();
				int z = Message.ReadInt32();
				b.Read(Message);
				blocks[x, y, z] = b;
			}
			LayerCount++;

			if (LayerCount == XSize * YSize) {
				Current = State.Done;
				refreshExposedBlocks();
			}
		}

		enum State {
			SendingWorldSize, ReceivingWorldSize,
			SendingLayersRequest, ReceivingLayers,
			Done
		}

		State Current = State.SendingWorldSize;
		internal bool ReceiveClient(List<NetIncomingMessage> Messages, NetClient Client) {
			if (Current == State.SendingWorldSize){
				Current = State.ReceivingWorldSize;
				NetOutgoingMessage nom = Client.CreateMessage();
				nom.Write("Request");
				nom.Write("WorldSize");
				Client.SendMessage(nom, NetDeliveryMethod.ReliableUnordered);
			}

			if (Current == State.SendingLayersRequest) {
				Current = State.ReceivingLayers;
				NetOutgoingMessage nom = Client.CreateMessage();
				nom.Write("Request");
				nom.Write("BlockLayers");
				Client.SendMessage(nom, NetDeliveryMethod.ReliableUnordered);
			}

			if (Current == State.ReceivingWorldSize || Current == State.ReceivingLayers) {
				foreach (NetIncomingMessage Message in Messages) {
					if (Message.MessageType == NetIncomingMessageType.Data) {
						string Type = Message.ReadString();
						if (Type == "WorldSize") {
							ReceiveWorldSize(Message);
						} else if (Type == "BlockLayer") {
							ReceiveBlockLayer(Message);
						}
						Message.Position = 0;
					}
				}
			}

			return Current == State.Done;
		}

		
	}
}
