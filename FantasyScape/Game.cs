using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Gwen.Control;
using OpenTK.Input;
using Lidgren.Network;
using System.Threading;
using FantasyScape.NetworkMessages;

namespace FantasyScape {
	public class Game {
		public static Game Instance;

		public World world;
		public Player player;

		public enum GameState {
			NotReady, Connecting, Playing
		}

		public GameState State = GameState.NotReady;

		NetClient Client;

		public Game() {
			world = new World();
			Instance = this;
		}

		private void InitWorld(){
			world.GenerateMap();
			player = new Player(world.XSize/2,world.ZSize/2, world);
		}

		public void Update() {
			if (Client != null) {
				List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
				Client.ReadMessages(Messages);
				foreach (NetIncomingMessage msg in Messages) {
					Message.Handle(msg);
				}
			}

			if (State == GameState.Playing) {
				player.update();
				world.update();
			}

			if (State == GameState.Connecting) {
				bool Ready = true;

				Ready &= Textures.Ready();
				Ready &= BlockTypes.Ready();
				Ready &= world.Ready();

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
			}
			Console.WriteLine("Connected!");

			Message.RegisterClient(Client);
		}
	}
}
