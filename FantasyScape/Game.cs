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
			if (this.GameMode == FantasyScape.GameMode.Client) {
				MouseManager.SetMousePositionWindows(320, 240);
			}
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

		public void MountTextures() {
			Textures.ServerLoadTextures();
		}

		public void GenerateWorld() {
			InitWorld();
		}
	}
}
