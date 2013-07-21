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
using FantasyScape.RealmManagement;
using FantasyScape.RealmManagement.RealmDrawing;
using FantasyScape.RealmManagement.ChunkRequester;

namespace FantasyScape {
	public enum HostType { Server, Client };
	
	public class Game {
		public static HostType Host;

		public static Realm Realm;
		public static RealmRenderer Renderer;
		public static RealmRequester Requester;
		public static MapGenerator MapGenerator;

		public static List<Player> Players;
		public static Player Self = null;

		public static ServerInfo ServerInfo = new ServerInfo();

		public enum GameState {
			NotReady, Connecting, Playing
		}

		public static bool Render = false;

		public static bool LockMouse = false;

		public static GameState State = GameState.NotReady;

		public static void CenterMouse() {
			MouseManager.SetMousePositionWindows(GraphicsManager.WindowWidth / 2, GraphicsManager.WindowHeight / 2);
		}

		static Game() {
			Realm = new Realm();
			Renderer = new RealmRenderer(Realm);
			MapGenerator = new MapGenerator(Realm);
			Requester = new RealmRequester(Realm);

			Players = new List<Player>();
		}

		static bool RequestedPlayer = false;
		public static void UpdateClient() {
			if (State == GameState.Playing) {
				Self.Update();

				//Make sure we have all nearby chunks
				for (int x = -3; x <= 3; x++){
					for (int y = -3; y <= 3; y++){
						for (int z = -3; z <= 3; z++){
							Vector3i Loc = new Vector3i(
										(int)(Self.xpos + (x * NetworkChunk.Size.X)),
										(int)(Self.xpos + (y * NetworkChunk.Size.Y)),
										(int)(Self.xpos + (z * NetworkChunk.Size.Z)));
							if (Realm.GetBlock(Loc) == null) {
								Requester.QueueRequest(Loc);
							}
						}
					}
				}
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
				}
			}
		}

		public static void UpdateServer() {
			//World.Update();
		}

		public static void Draw() {
			Game.Render = true;
			if (State == GameState.Playing) {
				Renderer.Draw();
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
	}
}
