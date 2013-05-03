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
		static Game game;
		static MenuManager menu;

		static void Main(string[] args){
			GraphicsManager.EnableMipmap = false;
			game = new Game(GameMode.Client);
			menu = new MenuManager();

			GraphicsManager.SetBackground(Color.FromArgb(200, 200, 255));
			
			GraphicsManager.SetTitle("FantasyScape");
			GraphicsManager.SetResolution(640, 480);

			GraphicsManager.Update += Update;
			GraphicsManager.Render += Draw;
			GraphicsManager.Start();
			MainCanvas.Dispose();
		}

		static void Update() {
			if (menu.Mode == MenuManager.SINGLEPLAYER) {
				game.Update();
			}

			if (KeyboardManager.IsPressed(Key.Escape)) {
				MainCanvas.Dispose();
				Environment.Exit(0);
			}
			if (KeyboardManager.IsPressed(Key.F11)) {
				//Toggle Full Screen
				if (GraphicsManager.windowstate != OpenTK.WindowState.Fullscreen) {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Fullscreen);
				} else {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Normal);
				}
			}
		}

		static void Draw() {
			if (menu.Mode == MenuManager.SINGLEPLAYER) {
				game.Draw();
			}
		}
	}
}
