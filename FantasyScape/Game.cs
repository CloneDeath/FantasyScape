using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Gwen.Control;
using OpenTK.Input;
using Lidgren.Network;
using System.Threading;

namespace FantasyScape {
	public class Game {
		public World world;
		Player player;

		public enum GameState {
			NotReady, Connecting, Playing
		}

		public GameState State = GameState.NotReady;

		NetClient Client;

		public Game() {
			world = new World();
		}

		private void InitWorld(){
			world.GenerateMap();
			player = new Player(world.XSize/2,world.ZSize/2, world);
		}

		public void Update() {
			if (State == GameState.Playing) {
				player.update();
				world.update();
			}

			if (State == GameState.Connecting) {
				bool Ready = true;
				List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
				Client.ReadMessages(Messages);

				Ready &= Textures.ReceiveClient(Messages, Client);
				//Ready &= BlockTypes.ReceiveClient(Messages, Client);
				//Ready &= world.ReceiveClient(Messages, Client);

				if (Ready) {
					player = new Player(world.XSize / 2, world.ZSize / 2, world);
					MouseManager.SetMousePositionWindows(320, 240);
					State = GameState.Playing;
				}
			}
		}

		public void Draw() {
			if (State == GameState.Playing) {
				world.draw(player);
				player.updateCamera();
			}
		}

		public void GenerateWorld() {
			InitWorld();
		}

		public void Connect(string IPAddress, int Port) {
			State = GameState.Connecting;

			NetPeerConfiguration config = new NetPeerConfiguration("FantasyScape");
			Client = new NetClient(config);
			Client.Start();
			Client.Connect(IPAddress, Port);

			while (Client.ConnectionStatus != NetConnectionStatus.Connected) {
				Thread.Sleep(1000);
				Client.ReadMessages(new List<NetIncomingMessage>());
				Console.WriteLine(Client.ConnectionStatus);
			}
		}
	}
}
