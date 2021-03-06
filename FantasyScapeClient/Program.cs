﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Drawing;
using FantasyScape;
using OpenTK.Input;
using Lidgren.Network;
using FantasyScape.NetworkMessages;
using System.Threading;
using Gwen.Control;
using FantasyScape.Client.Editor;

namespace FantasyScape.Client {
	class Program {
		static MenuManager menu;
		static EscapeMenuManager escapemenu;
		
		static NetClient Client;

		static Camera2D Overlay;
		private static OpenTK.Vector2d OldPos;
		

		static void Main(string[] args){
			/* Set up Graphics Manager */
			GraphicsManager.EnableMipmap = false;
			GraphicsManager.UseExperimentalFullAlpha = true;
			
			GraphicsManager.SetTitle("FantasyScape");
			GraphicsManager.SetWindowState(OpenTK.WindowState.Maximized);
			GraphicsManager.SetBackground(Color.FromArgb(200, 200, 255));

			GraphicsManager.Update += Update;
			GraphicsManager.Render += Draw;

			Overlay = new Camera2D();
			Overlay.Layer = 1; //Draw behind everything
			Overlay.OnRender += Draw2D;

			/* Create Game World */
			menu = new MenuManager();
			escapemenu = new EscapeMenuManager();
			DevelopmentMenu.Hidden = true;

			/* Start Game */
			GraphicsManager.Start();

			/* Hack: Clean up, this should be done automatically - GWEN, I'm looking at you >:| */
			MainCanvas.Dispose();
		}


		static List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
		static void Update() {
			if (Client != null) {
				Client.ReadMessages(Messages);
				for (int i = 0; i < 10; i++) {
					if (Messages.Count != 0) {
						Message.Handle(Messages[0]);
						Messages.RemoveAt(0);
					} else {
						break;
					}
				}
			}

			Game.UpdateClient();

			menu.Connecting.Update();

			if (KeyboardManager.IsPressed(Key.Escape)) {
				escapemenu.Hidden = !escapemenu.Hidden;
			}

			if (KeyboardManager.IsPressed(Key.Tilde)) {
				DevelopmentMenu.Hidden = !DevelopmentMenu.Hidden;
			}

			bool LockMouse = Game.LockMouse;
			if (escapemenu.Hidden && DevelopmentMenu.Hidden) {
				LockMouse = true;
			} else {
				LockMouse = false;
			}
			if (LockMouse != Game.LockMouse) {
				if (LockMouse) {
					OldPos = MouseManager.GetMousePositionWindows();
					Game.CenterMouse();
				} else {
					MouseManager.SetMousePositionWindows((int)OldPos.X, (int)OldPos.Y);
				}
			}
			Game.LockMouse = LockMouse;

			//Toggle Full Screen
			if (KeyboardManager.IsPressed(Key.F11)) {
				if (GraphicsManager.windowstate != OpenTK.WindowState.Fullscreen) {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Fullscreen);
				} else {
					GraphicsManager.SetWindowState(OpenTK.WindowState.Normal);
				}
			}
		}

		static void Draw() {
			Game.Draw();
		}

		static Label lblDebug = new Label(MainCanvas.GetCanvas());
		static void Draw2D() {
			Game.Draw2D();

			if (!escapemenu.Hidden || !DevelopmentMenu.Hidden) {
				GraphicsManager.DrawRectangle(0, 0, GraphicsManager.WindowWidth, GraphicsManager.WindowHeight, Color.FromArgb(0, 0, 0, 250));
			}

			if (Game.Self != null) {
				lblDebug.Text = "X: " + Game.Self.xpos.ToString("0.##") + ", Y: " + Game.Self.ypos.ToString("0.##") + ", Z: " + Game.Self.zpos.ToString("0.##") + "\n";
				lblDebug.Text += "Render Chunks: " + Game.World.Renderer.ChunkCount() + "\n";
				lblDebug.Text += "Memory Chunks: " + Game.World.Realm.ChunkCount() + "\n";
				lblDebug.Text += "Outgoing Chunk Requests: " + Game.World.Requester.OutgoingChunkCount() + "\n";
				lblDebug.Text += "Incomming Chunk Responses: " + Game.World.Requester.IncommingChunkCount() + "\n";
				GraphicsManager.DrawRectangle(0, 0, 200, 75, Color.White);
			}
		}

		public static void Connect(string IPAddress, int Port) {
			Game.State = Game.GameState.Connecting;

			NetPeerConfiguration config = new NetPeerConfiguration("FantasyScape");
			Client = new NetClient(config);
			Client.Start();
			Client.Connect(IPAddress, Port);

			while (Client.ConnectionStatus != NetConnectionStatus.Connected) {
				Thread.Sleep(1000);
				Client.ReadMessages(new List<NetIncomingMessage>());
			}
			Console.WriteLine("Connected!");

			Message.RegisterClient(Client);
		}
	}
}
