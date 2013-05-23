using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FantasyScape.Client.Editor.Tools {
	class Brush : ITool {
		public string GetDisplayName() {
			return "Brush";
		}

		public void UseTool(int X, int Y, PixelData Canvas, ref Color CurrentColor) {
			Canvas[X, Y] = CurrentColor;
		}
	}
}
