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

namespace FantasyScape.Client {
	class Program {
		static MenuManager menu;
		static NetClient Client;

		static void Main(string[] args){
			/* Set up Graphics Manager */
			GraphicsManager.EnableMipmap = false;
			
			GraphicsManager.SetTitle("FantasyScape");
			GraphicsManager.SetResolution(640, 480);
			GraphicsManager.SetBackground(Color.FromArgb(200, 200, 255));

			GraphicsManager.Update += Update;
			GraphicsManager.Render += Draw;

			/* Create Game World */
			menu = new MenuManager();

			/* Start Game */
			GraphicsManager.Start();

			/* Hack: Clean up, this should be done automatically - GWEN, I'm looking at you >:| */
			MainCanvas.Dispose();
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
				MainCanvas.Dispose();
				Environment.Exit(0);
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
