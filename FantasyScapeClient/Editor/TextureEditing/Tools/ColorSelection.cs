using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.Client.Editor.Tools {
	class ColorSelection : ITool {
		public string GetDisplayName() {
			return "Color Picker";
		}

		public void UseTool(int X, int Y, PixelData Canvas, ref System.Drawing.Color CurrentColor) {
			CurrentColor = Canvas[X, Y];
		}
	}
}
