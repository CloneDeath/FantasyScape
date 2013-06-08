using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape {
	public class Vector2i {
		public int X, Y;

		public Vector2i() {
			X = 0;
			Y = 0;
		}

		public Vector2i(int X, int Y) {
			this.X = X;
			this.Y = Y;
		}

		public override bool Equals(object obj) {
			Vector2i other = obj as Vector2i;
			if (other == null) {
				return false;
			} else {
				return (this.X == other.X) && (this.Y == other.Y);
			}
		}

		public override int GetHashCode() {
			return (X << 16) & (Y);
		}
	}
}
