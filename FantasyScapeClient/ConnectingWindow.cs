using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client {
	class ConnectingWindow : WindowControl {
		ProgressBar bar;

		public ConnectingWindow() : base(MainCanvas.GetCanvas()) {
			this.SetSize(400, 100);
			this.IsClosable = false;
			this.DisableResizing();
			this.ClampMovement = true;
			Label message = new Label(this);
			message.Text = "Downloading Content";
			message.AutoSizeToContents = true;
			message.SetPosition(10, 10);
			
			bar = new ProgressBar(this);
			bar.SetPosition(10, 40);
			bar.SetSize(300, 20);
			bar.IsHorizontal = true;
			bar.AutoLabel = true;

			
		}

		public void Update() {
			//bar.Value = Game.GetProgress();
			//if (Game.GetProgress() >= 1.0f) {
				this.Hide();
			//}
		}
	}
}
