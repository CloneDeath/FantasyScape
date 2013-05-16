using System;
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

namespace FantasyScape.Client {
	class Program {
		static MenuManager menu;
		static NetClient Client;

		static WindowControl EscapeWindow;

		static Camera2D Overlay;

		static void Main(string[] args){
			/* Set up Graphics Manager */
			GraphicsManager.EnableMipmap = false;
			GraphicsManager.UseExperimentalFullAlpha = true;
			
			GraphicsManager.SetTitle("FantasyScape");
			GraphicsManager.SetResolution(640, 480);
			GraphicsManager.SetBackground(Color.FromArgb(200, 200, 255));

			GraphicsManager.Update += Update;
			GraphicsManager.Render += Draw;

			Overlay = new Camera2D();
			Overlay.Layer = -1; //Draw behind everything
			Overlay.OnRender += Draw2D;

			/* Create Game World */
			menu = new MenuManager();
			CreateEscapeWindow();

			/* Start Game */
			GraphicsManager.Start();

			/* Hack: Clean up, this should be done automatically - GWEN, I'm looking at you >:| */
			MainCanvas.Dispose();
		}

		private static void CreateEscapeWindow() {
			EscapeWindow = new WindowControl(MainCanvas.GetCanvas());
			EscapeWindow.SetPosition(10, 10);
			EscapeWindow.SetSize(200, 200);

			Button Close = new Button(EscapeWindow);
			Close.SetPosition(10, 10);
			Close.SetText("Continue");
			Close.Clicked += delegate(Base sender) {
				EscapeWindow.Hide();
				Game.LockMouse = true;
			};

			Button Quit = new Button(EscapeWindow);
			Quit.SetPosition(10, 40);
			Quit.SetText("Quit");
			Quit.Clicked += delegate(Base sender) {
				MainCanvas.Dispose();
				Environment.Exit(0);
			};

			EscapeWindow.Hide();
		}

		static void Update() {
			if (Client != null) {
				List<NetIncomingMessage> Messages = new List<NetIncomingMessage>();
				Client.ReadMessages(Messages);
				foreach (NetIncomingMessage msg in Messages) {
					Message.Handle(msg);
				}
			}

			Game.Update();

			if (KeyboardManager.IsPressed(Key.Escape)) {
				if (EscapeWindow.IsHidden) {
					EscapeWindow.Show();
					Game.LockMouse = false;
				} else {
					EscapeWindow.Hide();
					Game.LockMouse = true;
				}
			}

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

		static void Draw2D() {
			Game.Draw2D();
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
