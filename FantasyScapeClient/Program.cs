using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;
using FantasyScape;
using OpenTK.Input;

namespace FantasyScape.Client {
	class Program {
		public static Game game;
		static MenuManager menu;

		static void Main(string[] args){
			/* Set up Graphics Manager */
			GraphicsManager.EnableMipmap = false;
			
			GraphicsManager.SetTitle("FantasyScape");
			GraphicsManager.SetResolution(640, 480);
			GraphicsManager.SetBackground(Color.FromArgb(200, 200, 255));

			GraphicsManager.Update += Update;
			GraphicsManager.Render += Draw;

			/* Create Game World */
			game = new Game(GameMode.Client);
			menu = new MenuManager();

			/* Start Game */
			GraphicsManager.Start();

			/* Hack: Clean up, this should be done automatically - GWEN, I'm looking at you >:| */
			MainCanvas.Dispose();
		}

		static void Update() {
			if (menu.Mode == MenuManager.MenuMode.InGame) {
				game.Update();
			}

			if (KeyboardManager.IsPressed(Key.Escape)) {
				MainCanvas.Dispose();
				Environment.Exit(0);
			}

			//Toggle Full Screen
			if (KeyboardManager.IsPressed(Key.F11)) {
				if (GraphicsManager.windowstate != OpenTK.WindowState.Fullscreen) {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Fullscreen);
				} else {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Normal);
				}
			}
		}

		static void Draw() {
			if (menu.Mode == MenuManager.MenuMode.InGame) {
				game.Draw();
			}
		}

		internal static void Connect(string IPAddress, int Port) {
			throw new NotImplementedException();
		}
	}
}
