using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape {
	public class Vector3i {
		public int X, Y, Z;

		public Vector3i() {
			X = 0;
			Y = 0;
			Z = 0;
		}

		public Vector3i(int X, int Y, int Z) {
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		public override bool Equals(object obj) {
			Vector3i other = obj as Vector3i;
			if (other == null) {
				return false;
			} else {
				return (this.X == other.X) && (this.Y == other.Y) && (this.Z == other.Z);
			}
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}
}
