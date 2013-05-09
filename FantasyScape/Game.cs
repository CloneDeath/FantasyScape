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
		public static World World;
		public static Player Player;

		public enum GameState {
			NotReady, Connecting, Playing
		}

		public static GameState State = GameState.NotReady;

		static Game() {
			World = new World();
		}

		public static void Update() {
			if (State == GameState.Playing) {
				Player.update();
				World.update();
			}

			if (State == GameState.Connecting) {
				bool Ready = true;

				Ready &= Textures.Ready();
				Ready &= BlockTypes.Ready();
				Ready &= World.Ready();

				if (Ready) {
					Player = new Player(World.XSize / 2, World.ZSize / 2);
					MouseManager.SetMousePositionWindows(320, 240);
					State = GameState.Playing;
				}
			}
		}

		public static void Draw() {
			if (State == GameState.Playing) {
				World.draw(Player);
				Player.updateCamera();
			}
		}

		public static void GenerateWorld() {
			World.GenerateMap();
			Player = new Player(World.XSize / 2, World.ZSize / 2);
		}
	}
}
