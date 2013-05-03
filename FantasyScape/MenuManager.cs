using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape {
	class MenuManager {
		public const int MAINMENU = 0;
		public const int SINGLEPLAYER = 1;
		public const int HOSTGAME = 2;
		public const int JOINGAME = 3;

		public const int FINDGAME = 4;
		public const int LOBBY = 5;

		public int Mode = MAINMENU;

		bool Multiplayer = false;
		bool Host = true;

		public MenuManager() {
			MainMenu = ConstructMainMenu();
			MainMenu.Show();

			FindGame = ConstructFindGame();
			FindGame.Hide();
		}

		WindowControl ConstructMainMenu() {
			WindowControl MainMenu = new WindowControl(MainCanvas.GetCanvas());
			MainMenu.SetSize(300, 300);

			Button SinglePlayer = new Button(MainMenu);
			SinglePlayer.SetText("Single Player");
			SinglePlayer.SetPosition(10, 10);
			SinglePlayer.SetSize(220, 20);
			SinglePlayer.Clicked += delegate(Base caller) {
				Mode = SINGLEPLAYER;
				MainMenu.Hide();
				Host = true;
				Multiplayer = false;
			};

			Button JoinGame = new Button(MainMenu);
			JoinGame.SetText("Join Game");
			JoinGame.SetPosition(10, 70);
			JoinGame.SetSize(220, 20);
			JoinGame.Clicked += delegate(Base sender) {
				Multiplayer = true;
				Host = false;
				//IPAddress.selected = true;

				MainMenu.Hide();
				FindGame.Show();

				Mode = FINDGAME;
			};

			Button Quit = new Button(MainMenu);
			Quit.SetText("Quit");
			Quit.SetPosition(10, 100);
			Quit.SetSize(220, 20);
			Quit.Clicked += delegate(Base sender) {
				MainCanvas.Dispose();
				Environment.Exit(0);
			};

			MainMenu.SetPosition(10, 10);

			return MainMenu;
		}
		WindowControl MainMenu;

		WindowControl ConstructFindGame() {
			WindowControl FindGame = new WindowControl(MainCanvas.GetCanvas());
			FindGame.SetPosition(10, 10);
			FindGame.SetSize(300, 300);

			Button Connect = new Button(FindGame);
			Connect.SetText("Connect");
			Connect.SetPosition(10, 420);
			Connect.SetSize(200, 20);
			/*if (Connect.isClicked()) {
				client = new GameClient(IPAddress.text, Port.text);
			}*/

			Label EnterIP = new Label(FindGame);
			EnterIP.SetText("Enter an IP:");
			EnterIP.SetPosition(10, 10);

			TextBox IPAddress = new TextBox(FindGame);
			IPAddress.SetText("127.0.0.1");
			IPAddress.SetPosition(10, 40);
			IPAddress.SetSize(260, 20);

			TextBox Port = new TextBox(FindGame);
			Port.SetText("4444");
			Port.SetPosition(10, 70);
			Port.SetSize(260, 20);

			Button Back = new Button(FindGame);
			Back.SetText("Back");
			Back.SetPosition(10, 450);
			Back.SetSize(200, 20);
			Back.Clicked += delegate(Base sender) {
				Mode = MAINMENU;
				MainMenu.Show();
				FindGame.Hide();
			};

			return FindGame;
		}
		WindowControl FindGame;
	}
}
