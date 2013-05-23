using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.Client.Editor.Tools {
	class Fill : ITool {
		public string GetDisplayName() {
			return "Fill";
		}

		private class Coord {
			public int X;
			public int Y;
			public Coord(int X, int Y) {
				this.X = X;
				this.Y = Y;
			}

			public override bool Equals(object obj) {
				if (obj as Coord == null) return false;
				Coord other = obj as Coord;
				return this.X == other.X && this.Y == other.Y;
			}
		}
		public void UseTool(int X, int Y, PixelData Canvas, ref System.Drawing.Color CurrentColor) {
			//Canvas[X, Y] = CurrentColor;
			List<Coord> AdjacentCoords = new List<Coord>();
			GetAdjacentCoords(AdjacentCoords, Canvas, new Coord(X, Y), Canvas[X, Y]);

			foreach (Coord coord in AdjacentCoords) {
				Canvas[coord.X, coord.Y] = CurrentColor;
			}
		}

		private void GetAdjacentCoords(List<Coord> AdjacentCoords, PixelData Canvas, Coord coord, System.Drawing.Color SeekColor) {
			if (coord.X >= 0 && coord.X < Canvas.Width && coord.Y >= 0 && coord.Y < Canvas.Height && Canvas[coord.X, coord.Y] == SeekColor) {
				if (!AdjacentCoords.Contains(coord)) {
					AdjacentCoords.Add(coord);

					GetAdjacentCoords(AdjacentCoords, Canvas, new Coord(coord.X - 1, coord.Y), SeekColor);
					GetAdjacentCoords(AdjacentCoords, Canvas, new Coord(coord.X + 1, coord.Y), SeekColor);
					GetAdjacentCoords(AdjacentCoords, Canvas, new Coord(coord.X, coord.Y - 1), SeekColor);
					GetAdjacentCoords(AdjacentCoords, Canvas, new Coord(coord.X, coord.Y + 1), SeekColor);
				}
			}
		}
	}
}
