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
using System.Diagnostics;
using FantasyScape.Resources;

namespace FantasyScape {
	public class Game {
		public static World World;
		public static List<Player> Players;
		public static Player Self = null;

		public enum GameState {
			NotReady, Connecting, Playing
		}

		public static bool Render = false;

		public static bool LockMouse = false;

		public static GameState State = GameState.NotReady;

		private static MapGenerator WorldGen = new MapGenerator();

		public static void CenterMouse() {
			MouseManager.SetMousePositionWindows(GraphicsManager.WindowWidth / 2, GraphicsManager.WindowHeight / 2);
		}

		static Game() {
			World = new World();
			Players = new List<Player>();
		}

		static bool RequestedPlayer = false;
		public static void UpdateClient() {
			if (State == GameState.Playing) {
				Self.Update();
			}

			if (State == GameState.Connecting) {
				bool Ready = true;

				Ready &= Package.Ready();
				Ready &= (Self != null);

				if (!RequestedPlayer) {
					RequestedPlayer = true;
					new RequestMessage(RequestType.NewPlayer).Send();
				}

				if (Ready) {
					Game.CenterMouse();
					State = GameState.Playing;
					Stopwatch.StartNew();
					World.RefreshExposedBlocks();
				}
			}
		}

		public static void UpdateServer() {
			World.Update();
		}

		public static void Draw() {
			Game.Render = true;
			if (State == GameState.Playing) {
				World.Draw(Self);
				Self.updateCamera();

				foreach (Player p in Players) {
					if (p != Self || KeyboardManager.IsDown(Key.F2)) {
						p.DrawWorld();
					}
				}
			}
		}

		public static void Draw2D() {
			if (State == GameState.Playing) {
				Self.Draw2D();
			}
		}

		internal static Player AddNewPlayer() {
			Player p = new Player(0, 0);
			p.PlayerID = Players.Count;
			Players.Add(p);
			return p;
		}

		internal static void AddPlayer(Player p) {
			Players.Add(p);
		}

		internal static void SetSelf(Player p) {
			Self = p;
		}

		public static Player FindPlayer(int PlayerID) {
			foreach (Player p in Players) {
				if (p.PlayerID == PlayerID) {
					return p;
				}
			}
			return null;
		}

		public static void GenerateChunk(Vector3i Location) {
			Game.World.Chunks[Location] = WorldGen.GenerateTerrain(Location.X, Location.Y, Location.Z);
		}
	}
}
