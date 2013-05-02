using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Gwen.Control;
using OpenTK.Input;

namespace FantasyScape {
	public class Game {
		World world;
		Player player;

		MenuManager Menu = new MenuManager();

		public Game() {
			Initialize();
			MouseManager.SetMousePositionWindows(320, 240);
		}

		private void Initialize() {
			LoadTextures();
			InitWorld();
		}

		private void LoadTextures() {
			Textures.Initialize();
			Console.WriteLine("Done Loading Textures");
		}

		private void InitWorld(){
			world = new World();
			player = new Player(world.XSize/2,world.ZSize/2, world);
			Console.WriteLine("World Generated");
			//DirectionalLightSource dls = new DirectionalLightSource(objRoot);
			//AmbientLightSource als = new AmbientLightSource(objRoot);
		}

		public void Update() {
			if (Menu.Mode == MenuManager.SINGLEPLAYER) {
				player.update();
				world.update();
			}

			if (KeyboardManager.IsPressed(Key.Escape)){
				MainCanvas.Dispose();
				Environment.Exit(0);
			}
			if (KeyboardManager.IsPressed(Key.F11)){
				//Toggle Full Screen
				if (GraphicsManager.windowstate != OpenTK.WindowState.Fullscreen) {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Fullscreen);
				} else {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Normal);
				}
			}
		}

		public void Draw() {
			if (Menu.Mode == MenuManager.SINGLEPLAYER || Menu.Mode == MenuManager.HOSTGAME || Menu.Mode == MenuManager.JOINGAME) {
				player.updateCamera();
				world.draw(player);
			}
		}
	}
}
