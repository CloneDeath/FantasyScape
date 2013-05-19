using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client {
	class MenuManager {
		public enum MenuMode {
			MainMenu, JoiningGame, InGame
		}

		public MenuMode Mode = MenuMode.MainMenu;

		public ConnectingWindow Connecting;

		public MenuManager() {
			MainMenu = ConstructMainMenu();
			MainMenu.Show();

			FindGame = ConstructFindGame();
			FindGame.Hide();

			Connecting = new ConnectingWindow();
			Connecting.Hide();
		}

		Base ConstructMainMenu() {
			Base MainMenu = new Base(MainCanvas.GetCanvas());
			MainMenu.SetSize(300, 300);
			MainMenu.Dock = Gwen.Pos.Center;
			

			Button SinglePlayer = new Button(MainMenu);
			SinglePlayer.SetText("Single Player");
			SinglePlayer.SetPosition(10, 10);
			SinglePlayer.SetSize(220, 20);
			//SinglePlayer.Clicked += delegate(Base caller) {
			//    Mode = SINGLEPLAYER;
			//    MainMenu.Hide();
			//};
			SinglePlayer.Disable();

			Button JoinGame = new Button(MainMenu);
			JoinGame.SetText("Join Game");
			JoinGame.SetPosition(10, 70);
			JoinGame.SetSize(220, 20);
			JoinGame.Clicked += delegate(Base sender) {
				MainMenu.Hide();
				FindGame.Show();

				Mode = MenuMode.JoiningGame;
			};

			Button Quit = new Button(MainMenu);
			Quit.SetText("Quit");
			Quit.SetPosition(10, 100);
			Quit.SetSize(220, 20);
			Quit.Clicked += delegate(Base sender) {
				MainCanvas.Dispose();
				Environment.Exit(0);
			};

			return MainMenu;
		}
		Base MainMenu;

		Base ConstructFindGame() {
			Base FindGame = new Base(MainCanvas.GetCanvas());
			FindGame.SetSize(300, 300);

			Label EnterIP = new Label(FindGame);
			EnterIP.AutoSizeToContents = true;
			EnterIP.SetText("Enter an IP:");
			EnterIP.SetPosition(10, 10);

			TextBox IPAddress = new TextBox(FindGame);
			IPAddress.SetText("127.0.0.1");
			IPAddress.SetPosition(10, 40);
			IPAddress.SetSize(260, 20);

			TextBox Port = new TextBox(FindGame);
			Port.SetText("54987");
			Port.SetPosition(10, 70);
			Port.SetSize(260, 20);

			Button Connect = new Button(FindGame);
			Connect.SetText("Connect");
			Connect.SetPosition(10, 200);
			Connect.SetSize(200, 20);
			Connect.Clicked += delegate(Base sender) {
				Program.Connect(IPAddress.Text, Int32.Parse(Port.Text));
				MainMenu.Hide();
				FindGame.Hide();

				Connecting.Show();
			};

			Button Back = new Button(FindGame);
			Back.SetText("Back");
			Back.SetPosition(10, 225);
			Back.SetSize(200, 20);
			Back.Clicked += delegate(Base sender) {
				Mode = MenuMode.MainMenu;
				MainMenu.Show();
				FindGame.Hide();
			};

			return FindGame;
		}
		Base FindGame;
	}
}
