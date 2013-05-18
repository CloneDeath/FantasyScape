using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;
using FantasyScape.Client.Editor.BlockTypesEditor;
using FantasyScape.Client.Editor;

namespace FantasyScape.Client {
	class EscapeMenuManager {
		WindowControl EscapeWindow;

		private bool _Hidden = true;
		public bool Hidden{
			get { return _Hidden; }
			set {
				_Hidden = value;
				if (_Hidden) {
					MainCanvas.GetCanvas().DeleteAllChildren();
				} else {
					CreateEscapeWindow();
				}
			}

		}

		private void CreateEscapeWindow() {
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

			Button Edit = new Button(EscapeWindow);
			Edit.SetPosition(10, 40);
			Edit.SetText("Open Editor");
			Edit.Clicked += delegate(Base sender) {
				new EditorWindow();
			};

			Button Quit = new Button(EscapeWindow);
			Quit.SetPosition(10, 70);
			Quit.SetText("Quit");
			Quit.Clicked += delegate(Base sender) {
				MainCanvas.Dispose();
				Environment.Exit(0);
			};
		}
	}
}
