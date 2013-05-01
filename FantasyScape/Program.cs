using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Gwen.Control;

namespace FantasyScape {
	class Program {
		static void Main(string[] args) {
			GraphicsManager.EnableMipmap = false;
			Game game = new Game();

			GraphicsManager.SetTitle("FantasyScape");
			GraphicsManager.SetResolution(640, 480);

			GraphicsManager.Update += game.Update;
			GraphicsManager.Render += game.Draw;
			GraphicsManager.Start();
			MainCanvas.Dispose();
		}
	}
}
