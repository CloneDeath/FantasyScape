using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace FantasyScape {
	class Utility {
		public static Vector3 cross(Vector3 a, Vector3 b) {
			Vector3 ret = new Vector3();

			ret.X = a.Y * b.Z - a.Z * b.Y;
			ret.Y = a.Z * b.X - a.X * b.Z;
			ret.Z = a.X * b.Y - a.Y * b.X;

			return (ret);
		}

		public static float innerProduct(Vector3 a, Vector3 b) {
			return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
		}
	}
}
