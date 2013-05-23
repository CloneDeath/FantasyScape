using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FantasyScape.Client.Editor.Tools {
	interface ITool {
		string GetDisplayName();
		void UseTool(int X, int Y, PixelData Canvas, ref Color CurrentColor);
	}
}
