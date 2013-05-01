using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FantasyScape {
	class Frustum {
		private int TOP = 0;
		private int BOTTOM = 1;
		private int LEFT = 2;
		private int RIGHT = 3;
		private int NEARP = 4;
		private int FARP = 5;


		public static int OUTSIDE = 0;
		public static int INTERSECT = 1;
		public static int INSIDE = 2;

		private Plane[] pl = new Plane[6];

		Vector3 ntl, ntr, nbl, nbr, ftl, ftr, fbl, fbr;
		double nearD, farD, ratio, angle, tang;
		double nw, nh, fw, fh;

		private double ANG2RAD = 3.14159265358979323846 / 180.0;

		public void setCamInternals(double angle, double ratio, double nearD, double farD) {
			this.ratio = ratio;
			this.angle = angle;
			this.nearD = nearD;
			this.farD = farD;

			tang = (float)Math.Tan(angle * ANG2RAD * 0.5);
			nh = nearD * tang;
			nw = nh * ratio;
			fh = farD * tang;
			fw = fh * ratio;

			for (int i = 0; i < 6; i++) {
				pl[i] = new Plane();
			}
		}

		public Vector3 Mult(Vector3 a, double scale) {
			float b = (float)scale;
			return new Vector3(a.X * b, a.Y * b, a.Z * b);
		}


		public void setCamDef(Vector3 p, Vector3 l, Vector3 u) {

			Vector3 nc, fc, X, Y, Z;

			Z = p - l;
			Z.Normalize();

			X = Utility.cross(u, Z);
			X.Normalize();

			Y = Utility.cross(Z, X);

			nc = p - Mult(Z, (nearD));
			fc = p - Mult(Z, (farD));

			ntl = (nc + Mult(Y, (nh))) - Mult(X, (nw));
			ntr = (nc + Mult(Y, (nh))) + Mult(X, (nw));
			nbl = (nc - Mult(Y, (nh))) - Mult(X, (nw));
			nbr = (nc - Mult(Y, (nh))) + Mult(X, (nw));

			ftl = fc + Mult(Y, (fh)) - Mult(X, (fw));
			ftr = fc + Mult(Y, (fh)) + Mult(X, (fw));
			fbl = fc - Mult(Y, (fh)) - Mult(X, (fw));
			fbr = fc - Mult(Y, (fh)) + Mult(X, (fw));

			pl[TOP].set3Points(ntr, ntl, ftl);
			pl[BOTTOM].set3Points(nbl, nbr, fbr);
			pl[LEFT].set3Points(ntl, nbl, fbl);
			pl[RIGHT].set3Points(nbr, ntr, fbr);
			pl[NEARP].set3Points(ntl, ntr, nbr);
			pl[FARP].set3Points(ftr, ftl, fbl);
		}


		public int pointInFrustum(Vector3 p) {
			int result = INSIDE;
			for (int i = TOP; i <= FARP; i++) {

				if (pl[i].distance(p) < 0)
					return OUTSIDE;
			}
			return (result);
		}


		int sphereInFrustum(Vector3 p, float raio) {

			int result = INSIDE;
			float distance;

			for (int i = 0; i < 6; i++) {
				distance = pl[i].distance(p);
				if (distance < -raio)
					return OUTSIDE;
				else if (distance < raio)
					result = INTERSECT;
			}
			return (result);

		}

		private Vector3 getVertexP(Vector3 normal, Vector3 pos) {

			Vector3 res = pos;

			if (normal.X > 0)
				res.X += 1.0f;

			if (normal.Y > 0)
				res.Y += 1.0f;

			if (normal.Z > 0)
				res.Z += 1.0f;

			return (res);
		}



		private Vector3 getVertexN(Vector3 normal, Vector3 pos) {

			Vector3 res = pos;

			if (normal.X < 0)
				res.X += 1.0f;

			if (normal.Y < 0)
				res.Y += 1.0f;

			if (normal.Z < 0)
				res.Z += 1.0f;

			return (res);
		}

		int boxInFrustum(Vector3 box) {

			int result = INSIDE;
			for (int i = 0; i < 6; i++) {

				if (pl[i].distance(getVertexP(pl[i].normal, box)) < 0)
					return OUTSIDE;
				else if (pl[i].distance(getVertexN(pl[i].normal, box)) < 0)
					result = INTERSECT;
			}
			return (result);

		}


		void drawPoints() {
			GL.Begin(BeginMode.Points);

			GL.Vertex3(ntl.X, ntl.Y, ntl.Z);
			GL.Vertex3(ntr.X, ntr.Y, ntr.Z);
			GL.Vertex3(nbl.X, nbl.Y, nbl.Z);
			GL.Vertex3(nbr.X, nbr.Y, nbr.Z);

			GL.Vertex3(ftl.X, ftl.Y, ftl.Z);
			GL.Vertex3(ftr.X, ftr.Y, ftr.Z);
			GL.Vertex3(fbl.X, fbl.Y, fbl.Z);
			GL.Vertex3(fbr.X, fbr.Y, fbr.Z);

			GL.End();
		}

		public void drawLines() {
			GL.Color3(1.0f, 0.0f, 0.0f);
			GL.Begin(BeginMode.LineLoop);
			//near plane
			GL.Vertex3(ntl.X, ntl.Y, ntl.Z);
			GL.Vertex3(ntr.X, ntr.Y, ntr.Z);
			GL.Vertex3(nbr.X, nbr.Y, nbr.Z);
			GL.Vertex3(nbl.X, nbl.Y, nbl.Z);
			GL.End();

			GL.Begin(BeginMode.LineLoop);
			//far plane
			GL.Vertex3(ftr.X, ftr.Y, ftr.Z);
			GL.Vertex3(ftl.X, ftl.Y, ftl.Z);
			GL.Vertex3(fbl.X, fbl.Y, fbl.Z);
			GL.Vertex3(fbr.X, fbr.Y, fbr.Z);
			GL.End();

			GL.Begin(BeginMode.LineLoop);
			//bottom plane
			GL.Vertex3(nbl.X, nbl.Y, nbl.Z);
			GL.Vertex3(nbr.X, nbr.Y, nbr.Z);
			GL.Vertex3(fbr.X, fbr.Y, fbr.Z);
			GL.Vertex3(fbl.X, fbl.Y, fbl.Z);
			GL.End();

			GL.Begin(BeginMode.LineLoop);
			//top plane
			GL.Vertex3(ntr.X, ntr.Y, ntr.Z);
			GL.Vertex3(ntl.X, ntl.Y, ntl.Z);
			GL.Vertex3(ftl.X, ftl.Y, ftl.Z);
			GL.Vertex3(ftr.X, ftr.Y, ftr.Z);
			GL.End();

			GL.Begin(BeginMode.LineLoop);
			//left plane
			GL.Vertex3(ntl.X, ntl.Y, ntl.Z);
			GL.Vertex3(nbl.X, nbl.Y, nbl.Z);
			GL.Vertex3(fbl.X, fbl.Y, fbl.Z);
			GL.Vertex3(ftl.X, ftl.Y, ftl.Z);
			GL.End();

			GL.Begin(BeginMode.LineLoop);
			// right plane
			GL.Vertex3(nbr.X, nbr.Y, nbr.Z);
			GL.Vertex3(ntr.X, ntr.Y, ntr.Z);
			GL.Vertex3(ftr.X, ftr.Y, ftr.Z);
			GL.Vertex3(fbr.X, fbr.Y, fbr.Z);

			GL.End();
		}


		void drawPlanes() {
			GL.Begin(BeginMode.Quads);

			//near plane
			GL.Color3(0.0f, 1.0f, 0.0f);//Green
			GL.Vertex3(ntl.X, ntl.Y, ntl.Z);
			GL.Vertex3(ntr.X, ntr.Y, ntr.Z);
			GL.Vertex3(nbr.X, nbr.Y, nbr.Z);
			GL.Vertex3(nbl.X, nbl.Y, nbl.Z);

			//far plane
			GL.Color3(0.0f, 1.0f, 0.0f); //Green
			GL.Vertex3(ftr.X, ftr.Y, ftr.Z);
			GL.Vertex3(ftl.X, ftl.Y, ftl.Z);
			GL.Vertex3(fbl.X, fbl.Y, fbl.Z);
			GL.Vertex3(fbr.X, fbr.Y, fbr.Z);

			//bottom plane
			GL.Color3(0.0f, 0.0f, 1.0f);//Blue
			GL.Vertex3(nbl.X, nbl.Y, nbl.Z);
			GL.Vertex3(nbr.X, nbr.Y, nbr.Z);
			GL.Vertex3(fbr.X, fbr.Y, fbr.Z);
			GL.Vertex3(fbl.X, fbl.Y, fbl.Z);

			//top plane
			GL.Color3(0.0f, 0.0f, 1.0f);//Blue
			GL.Vertex3(ntr.X, ntr.Y, ntr.Z);
			GL.Vertex3(ntl.X, ntl.Y, ntl.Z);
			GL.Vertex3(ftl.X, ftl.Y, ftl.Z);
			GL.Vertex3(ftr.X, ftr.Y, ftr.Z);

			//left plane
			GL.Color3(1.0f, 0.0f, 1.0f);//Purple
			GL.Vertex3(ntl.X, ntl.Y, ntl.Z);
			GL.Vertex3(nbl.X, nbl.Y, nbl.Z);
			GL.Vertex3(fbl.X, fbl.Y, fbl.Z);
			GL.Vertex3(ftl.X, ftl.Y, ftl.Z);

			// right plane
			GL.Color3(1.0f, 0.0f, 1.0f);//Purple
			GL.Vertex3(nbr.X, nbr.Y, nbr.Z);
			GL.Vertex3(ntr.X, ntr.Y, ntr.Z);
			GL.Vertex3(ftr.X, ftr.Y, ftr.Z);
			GL.Vertex3(fbr.X, fbr.Y, fbr.Z);

			GL.End();

		}

		void drawNormals() {

			Vector3 a, b;

			GL.Begin(BeginMode.Lines);

			// near
			a = (ntr + (ntl) + (nbr) + (nbl)) * (0.25f);
			b = a + (pl[NEARP].normal);
			GL.Vertex3(a.X, a.Y, a.Z);
			GL.Vertex3(b.X, b.Y, b.Z);

			// far
			a = (ftr + (ftl) + (fbr) + (fbl)) * (0.25f);
			b = a + (pl[FARP].normal);
			GL.Vertex3(a.X, a.Y, a.Z);
			GL.Vertex3(b.X, b.Y, b.Z);

			// left
			a = (ftl + (fbl) + (nbl) + (ntl)) * (0.25f);
			b = a + (pl[LEFT].normal);
			GL.Vertex3(a.X, a.Y, a.Z);
			GL.Vertex3(b.X, b.Y, b.Z);

			// right
			a = (ftr + (nbr) + (fbr) + (ntr)) * (0.25f);
			b = a + (pl[RIGHT].normal);
			GL.Vertex3(a.X, a.Y, a.Z);
			GL.Vertex3(b.X, b.Y, b.Z);

			// top
			a = (ftr + (ftl) + (ntr) + (ntl)) * (0.25f);
			b = a + (pl[TOP].normal);
			GL.Vertex3(a.X, a.Y, a.Z);
			GL.Vertex3(b.X, b.Y, b.Z);

			// bottom
			a = (fbr + (fbl) + (nbr) + (nbl)) * (0.25f);
			b = a + (pl[BOTTOM].normal);
			GL.Vertex3(a.X, a.Y, a.Z);
			GL.Vertex3(b.X, b.Y, b.Z);

			GL.End();
		}
	}
}
