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

namespace FantasyScape {
	public class Game {
		public static World World;
		public static List<Player> Players;
		public static Player Self = null;

		public enum GameState {
			NotReady, Connecting, Playing
		}

		public static bool LockMouse = false;

		public static GameState State = GameState.NotReady;

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

				Ready &= Textures.Ready();
				Ready &= BlockTypes.Ready();
				Ready &= World.Ready();
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

		public static float GetProgress() {
			return (float)Game.World.ChunkCount / ((float)World.XSize * (float)World.YSize * (float)World.ZSize);
		}

		public static void Draw() {
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

		public static void GenerateWorld() {
			MapGenerator mg = new MapGenerator(World);
			mg.GenerateTerrain();
		}

		internal static Player AddNewPlayer() {
			Player p = new Player(World.XSize / 2, World.YSize / 2);
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
	}
}
