using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;
using FantasyScape;

namespace FantasyScape.Client {
	class Program {
		static Game game;

		static void Main(string[] args){
			GraphicsManager.EnableMipmap = false;
			game = new Game();

			GraphicsManager.SetBackground(Color.FromArgb(200, 200, 255));
			
			GraphicsManager.SetTitle("FantasyScape");
			GraphicsManager.SetResolution(640, 480);

			GraphicsManager.Update += Update;
			GraphicsManager.Render += game.Draw;
			GraphicsManager.Start();
			MainCanvas.Dispose();
		}

		static void Update() {
			game.Update();
		}
	}
}
