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
	}
}
