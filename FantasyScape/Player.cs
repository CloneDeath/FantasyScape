using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using GLImp;
using OpenTK.Input;

namespace FantasyScape {
	class Player {
		public double xpos, ypos, zpos;
		double xrot, yrot;
		double xspeed, yspeed, zspeed;
	
		World world;
	
		float Speed = 6.0f/30.0f; //6 meters per second
		float PlayerHeight = 1.78f;
	
		public Frustum frustum;

		void setCamInternals(double angle, double ratio, double nearD, double farD) {
			frustum = new Frustum();
			frustum.setCamInternals(angle, ratio, nearD, farD);
		}
	
		public Player(float x, float z, World w){
			float Angle = 90;
			float Ratio = (float)GraphicsManager.WindowWidth / (float)GraphicsManager.WindowHeight;
			float nearD = 0.01f;
			float farD = 100.0f;
			Ratio = 0.6f;
			Angle = Angle / Ratio + 20f;

			setCamInternals(Angle, GraphicsManager.WindowWidth / GraphicsManager.WindowHeight, nearD, farD);
			ypos = 0;
			xpos = x;
			zpos = z;
			world = w;
		
			xspeed = 0;
			yspeed = 0;
			zspeed = 0;
		
			for (int i = w.YSize; i >= 0; i--){
				if (w.isSolid(x, i, z)){
					ypos = i+1;
					break;
				}
			}
		
		
			xrot = (float)Math.PI/2;
			yrot = 0;
		}
	
		private float LookingAtX(float s){
			return (float) ((Math.Cos(xrot) * Math.Cos(yrot) * s) + xpos);
		}

		private float LookingAtY(float s) {
			return (float)((-Math.Sin(xrot) * Math.Cos(yrot) * s) + ypos);
		}

		private float LookingAtZ(float s){
			return (float)(Math.Sin(yrot)*s + PlayerHeight + zpos);
		}
	
		
	
		public void updateCamera(){		
			if (KeyboardManager.IsDown(Key.F2)){
				//GLU.gluLookAt(xpos+20, ypos + 20, zpos+20, xpos, ypos, zpos, 0, 1, 0);
				GraphicsManager.SetCamera(new Vector3d(xpos + 20, ypos + 20, zpos + 20));
				GraphicsManager.SetLookAt(new Vector3d(xpos, ypos, zpos));
			} else {
				GraphicsManager.SetCamera(new Vector3d(xpos, ypos + PlayerHeight, zpos));
				GraphicsManager.SetLookAt(new Vector3d(LookingAtX(1), LookingAtY(1), LookingAtZ(1)));
			}
			Vector3 pos = new Vector3((float)(xpos + 3 * (xpos - LookingAtX(1))),
					(float)(ypos + PlayerHeight + 0 * (ypos - LookingAtY(1))),
					(float)(zpos + 3 * (zpos - LookingAtZ(1))));
			Vector3 lat = new Vector3(LookingAtX(1), LookingAtY(1), LookingAtZ(1));
			Vector3 up = new Vector3(0, 1, 0);
			frustum.setCamDef(pos,lat,up);
			if (KeyboardManager.IsDown(Key.F2)){
				//frustum.drawPlanes();
				frustum.drawLines();
				//frustum.drawNormals();
			}

		}
	
		public void update(){
			double newx = xpos;
			double newz = zpos;
		
			if (KeyboardManager.IsDown(Key.W)){
				newx += Math.Cos(xrot)*Speed;
				newz -= Math.Sin(xrot)*Speed;

			}
			if (KeyboardManager.IsDown(Key.S)) {
				newx -= Math.Cos(xrot)*Speed;
				newz += Math.Sin(xrot)*Speed;
			}
			if (KeyboardManager.IsDown(Key.A)) {
				newx += Math.Cos(xrot+(Math.PI/2))*Speed;
				newz -= Math.Sin(xrot+(Math.PI/2))*Speed;
			}
			if (KeyboardManager.IsDown(Key.D)) {
				newx += Math.Cos(xrot-(Math.PI/2))*Speed;
				newz -= Math.Sin(xrot-(Math.PI/2))*Speed;
			}
		
			if (!world.isSolid(newx, ypos+1.5f, newz) && !world.isSolid(newx, ypos+0.5f, newz))
			{
				xpos = newx;
				zpos = newz;
			} else {
				if (!world.isSolid(newx, ypos+1.5f, zpos) && !world.isSolid(newx, ypos+0.5f, zpos)){
					xpos = newx;
				} else if (!world.isSolid(xpos, ypos+1.5f, newz) && !world.isSolid(xpos, ypos+0.5f, newz)){
					zpos = newz;
				} 
			}
		
			xrot -= (MouseManager.GetMousePositionWindows().X - 320) / 150.0f;
			yrot += (MouseManager.GetMousePositionWindows().Y - 240) / 150.0f;
		
			if (yrot >= Math.PI/2){
				yrot = (float)Math.PI/2 - 0.001f;
			} else if (yrot <= -Math.PI/2){
				yrot = (float)-Math.PI/2 + 0.001f;
			}
		
			MouseManager.SetMousePositionWindows(320, 240);
		
			if (!world.isSolid(xpos, ypos+(yspeed), zpos)){
				yspeed -= (0.1);
			}
			if (yspeed < 0){
				if (world.isSolid(xpos, ypos+(yspeed), zpos)){
					ypos = (float)(Math.Floor(ypos+(yspeed)))+1.0f;
					yspeed = 0;
				}
			} else if (yspeed > 0) {
				if (world.isSolid(xpos, ypos+(yspeed)+PlayerHeight, zpos)){
					ypos = (float)(Math.Floor(ypos+(yspeed)+PlayerHeight))-1.0f;
					yspeed = 0;
				}
			}
		
			ypos += yspeed;
		
			if (KeyboardManager.IsDown(Key.Space) && 
					world.isSolid(xpos, ypos-1, zpos) && !world.isSolid(xpos, ypos+2, zpos)){
				yspeed = 1;
			}
		
			if (MouseManager.IsPressed(MouseButton.Left)){
				RemoveBlock();
			}

			if (MouseManager.IsPressed(MouseButton.Right)) {
				AddBlock();
			}
		}
	
		void RemoveBlock(){

			float bestDist = 100;
			int bestX = 0;
			int bestY = 0;
			int bestZ = 0;
			bool foundOne = false;
		
			int CDist = 5;
		
			Vector3 source = new Vector3();
			source.X = (float)xpos;
			source.Y = (float)(ypos + PlayerHeight);
			source.Z = (float)zpos;
		
			Vector3 dest = new Vector3();
			dest.X = LookingAtX(CDist);
			dest.Y = LookingAtY(CDist);
			dest.Z = LookingAtZ(CDist);
		
			for (int x = -CDist; x <= CDist; x++){
				for (int y = -CDist; y <= CDist; y++){
					for (int z = -CDist; z <= CDist; z++){
						if (world.isSolid(xpos + x, ypos + y + PlayerHeight, zpos + z)){
							Vector3 B1 = new Vector3();
							B1.X = (int)(xpos+x);
							B1.Y = (int)(ypos+y+PlayerHeight);
							B1.Z = (int)(zpos+z);
						
							Vector3 B2 = new Vector3();
							B2.X = B1.X + 1;
							B2.Y = B1.Y + 1;
							B2.Z = B1.Z + 1;
							if (CheckLineBox(B1, B2, source, dest) != NONE){
								foundOne = true;
								float length = (float)Math.Sqrt(x*x + y*y + z*z);
								if (length < bestDist){
									bestDist = length;
									bestX = (int)(xpos+x);
									bestY = (int)(ypos+y+PlayerHeight);
									bestZ = (int)(zpos+z);
								}
							}
						}
					}
				}
			}
		
			if (foundOne){
				//System.out.println("X:" + bestX + " Y:" + bestY + " Z:" + bestZ);
				world.removeBlock(bestX, bestY, bestZ);
			}
		}
	
	
		void AddBlock(){

			float bestDist = 100;
			int bestX = 0;
			int bestY = 0;
			int bestZ = 0;
			bool foundOne = false;
		
			int CDist = 5;
		
			int BestSide = NONE;
		
			Vector3 source = new Vector3();
			source.X = (float)xpos;
			source.Y = (float)(ypos + PlayerHeight);
			source.Z = (float)zpos;
		
			Vector3 dest = new Vector3();
			dest.X = LookingAtX(CDist);
			dest.Y = LookingAtY(CDist);
			dest.Z = LookingAtZ(CDist);
		
			for (int x = -CDist; x <= CDist; x++){
				for (int y = -CDist; y <= CDist; y++){
					for (int z = -CDist; z <= CDist; z++){
						if (world.isSolid(xpos + x, ypos + y + PlayerHeight, zpos + z)){
							Vector3 B1 = new Vector3();
							B1.X = (int)(xpos+x);
							B1.Y = (int)(ypos+y+PlayerHeight);
							B1.Z = (int)(zpos+z);
						
							Vector3 B2 = new Vector3();
							B2.X = B1.X + 1;
							B2.Y = B1.Y + 1;
							B2.Z = B1.Z + 1;
						
							int clb = CheckLineBox(B1, B2, source, dest);
							int[] temp = getBlockSide((int)(xpos+x), 
									(int)(ypos+y+PlayerHeight), 
									(int)(zpos+z), 
									clb);
							if (clb != NONE && clb != INSIDE && 
									world.blockAt(temp[0],temp[1], temp[2]) == null){
								foundOne = true;
								float length = (float)Math.Sqrt(x*x + y*y + z*z);
								if (length < bestDist){
									bestDist = length;
									bestX = (int)(xpos+x);
									bestY = (int)(ypos+y+PlayerHeight);
									bestZ = (int)(zpos+z);
									BestSide = clb;
								}
							}
						}
					}
				}
			}
		
			if (foundOne){
			
				int[] temp = getBlockSide(bestX, bestY, bestZ, BestSide);
				bestX = temp[0];
				bestY = temp[1];
				bestZ = temp[2];
			
				world.addBlock(bestX, bestY, bestZ);
			
				//System.out.println(BestSide);
				//System.out.println("X:" + bestX + " Y:" + bestY + " Z:" + bestZ);
			}
		}
	
		int[] getBlockSide(int x, int y, int z, int side){
			switch (side){
			case TOP:
				y += 1;break;
			case BOTTOM:
				y -= 1;break;
			case LEFT:
				x -= 1;break;
			case RIGHT:
				x += 1;break;
			case FRONT:
				z += 1;break;
			case BACK:
				z -= 1;break;
			}
		
			return new int[]{x,y,z};
		}
	
	
	
		Vector3 GetIntersection( float fDst1, float fDst2, Vector3 P1, Vector3 P2, out bool Valid) {
			Vector3 Hit = new Vector3();
			if ((fDst1 * fDst2) >= 0.0f) {
				Valid = false;
				return new Vector3();
			}
			if (fDst1 == fDst2) {
				Valid = false;
				return new Vector3();
			}
			Hit += (P2);
			Hit -= (P1);
			Hit *= (-fDst1/(fDst2-fDst1));
			Hit += (P1);
			Valid = true;
			return Hit;
		}

		bool InBox( Vector3 Hit, Vector3 B1, Vector3 B2, int Axis) {
			if ( Axis==1 && Hit.Z > B1.Z && Hit.Z < B2.Z && Hit.Y > B1.Y && Hit.Y < B2.Y) return true;
			if ( Axis==2 && Hit.Z > B1.Z && Hit.Z < B2.Z && Hit.X > B1.X && Hit.X < B2.X) return true;
			if ( Axis==3 && Hit.X > B1.X && Hit.X < B2.X && Hit.Y > B1.Y && Hit.Y < B2.Y) return true;
			return false;
		}

		const int NONE = -1;
		const int INSIDE = 0;
		const int TOP = 1;
		const int BOTTOM = 2;
		const int LEFT = 3;
		const int RIGHT = 4;
		const int BACK = 5;
		const int FRONT = 6;
			// returns true if line (L1, L2) intersects with the box (B1, B2)
			// returns intersection point in Hit
		int CheckLineBox( Vector3 B1, Vector3 B2, Vector3 L1, Vector3 L2){
			Vector3 Hit = new Vector3();
			if (L2.X < B1.X && L1.X < B1.X) return NONE;
			if (L2.X > B2.X && L1.X > B2.X) return NONE;
			if (L2.Y < B1.Y && L1.Y < B1.Y) return NONE;
			if (L2.Y > B2.Y && L1.Y > B2.Y) return NONE;
			if (L2.Z < B1.Z && L1.Z < B1.Z) return NONE;
			if (L2.Z > B2.Z && L1.Z > B2.Z) return NONE;
			if (L1.X > B1.X && L1.X < B2.X &&
				L1.Y > B1.Y && L1.Y < B2.Y &&
				L1.Z > B1.Z && L1.Z < B2.Z) 
				{Hit = L1; 
				return INSIDE;}
		
			float bestDist = 100;
			int bestSide = NONE;
			float d = 0;

			bool Valid;

			Hit = GetIntersection(L1.X - B1.X, L2.X - B1.X, L1, L2, out Valid);
			if (Valid && InBox(Hit, B1, B2, 1)) {
				d = distance(L1, Hit);
				if (d < bestDist){
					bestSide = LEFT;
					bestDist = d;
				}
			}

			Hit = GetIntersection(L1.X - B2.X, L2.X - B2.X, L1, L2, out Valid);
			if (Valid && InBox(Hit, B1, B2, 1)) {
				d = distance(L1, Hit);
				if (d < bestDist){
					bestSide = RIGHT;
					bestDist = d;
				}
			}

			Hit = GetIntersection(L1.Y - B1.Y, L2.Y - B1.Y, L1, L2, out Valid);
			if (Valid && InBox(Hit, B1, B2, 2)) {
				d = distance(L1, Hit);
				if (d < bestDist){
					bestSide = BOTTOM;
					bestDist = d;
				}
			}

			Hit = GetIntersection(L1.Y - B2.Y, L2.Y - B2.Y, L1, L2, out Valid);
			if (Valid && InBox(Hit, B1, B2, 2)) {
				d = distance(L1, Hit);
				if (d < bestDist){
					bestSide = TOP;
					bestDist = d;
				}
			}

			Hit = GetIntersection(L1.Z - B1.Z, L2.Z - B1.Z, L1, L2, out Valid);
			if (Valid && InBox(Hit, B1, B2, 3)) {
				d = distance(L1, Hit);
				if (d < bestDist){
					bestSide = BACK;
					bestDist = d;
				}
			}
		
			Hit = GetIntersection( L1.Z-B2.Z, L2.Z-B2.Z, L1, L2, out Valid);
			if (Valid && InBox(Hit, B1, B2, 3)) {
				d = distance(L1, Hit);
				if (d < bestDist){
					bestSide = FRONT;
					bestDist = d;
				}
			}

			return bestSide;
		}
	
		public float distance(Vector3 a, Vector3 b){
			return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
		}
	}
}
