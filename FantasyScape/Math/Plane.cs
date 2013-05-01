using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace FantasyScape {
	class Plane {
		public Vector3 normal, point;
		float d;

		public Plane() {
		}

		public Plane(Vector3 v1, Vector3 v2, Vector3 v3) {
			set3Points(v1, v2, v3);
		}

		public void set3Points(Vector3 v1, Vector3 v2, Vector3 v3) {
			Vector3 aux1, aux2;

			aux1 = v1 - v2;
			aux2 = v3 - v2;

			normal = Utility.cross(aux2, aux1);
			normal.Normalize();

			point = v2;

			d = -Utility.innerProduct(normal, point);
		}

		void setNormalAndPoint(Vector3 normal, Vector3 point) {

			this.normal = normal;
			this.normal.Normalize();
			d = -(Utility.innerProduct(this.normal, point));
		}

		void setCoefficients(float a, float b, float c, float d) {
			normal = new Vector3(a, b, c);
			float l = normal.Length;
			normal = new Vector3(a / l, b / l, c / l);
			this.d = d / l;
		}

		public float distance(Vector3 p) {
			return (d + Utility.innerProduct(normal, p));
		}

		void print(){
			Console.WriteLine("Plane: Normal: " + normal.ToString() + " D: " + d);
		}
	}
}
