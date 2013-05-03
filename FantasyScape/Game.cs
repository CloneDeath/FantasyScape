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

		GameMode GameMode;

		public Game(GameMode mode) {
			this.GameMode = mode;
			Initialize();
			MouseManager.SetMousePositionWindows(320, 240);
		}

		private void Initialize() {
			if (GameMode == GameMode.Server) {
				LoadTextures();
			}
			InitWorld();
		}

		private void LoadTextures() {
			Textures.ServerLoadTextures();
			Console.WriteLine("Done Loading Textures");
		}

		private void InitWorld(){
			world = new World();
			player = new Player(world.XSize/2,world.ZSize/2, world);
			Console.WriteLine("World Generated");
		}

		public void Update() {
			player.update();
			world.update();
		}

		public void Draw() {
			player.updateCamera();
			world.draw(player);
		}
	}
}
