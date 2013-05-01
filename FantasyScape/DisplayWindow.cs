using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;

namespace FantasyScape {
	class DisplayWindow {
		bool fullscreen = false;
	
		public void toggleFullscreen() {
			fullscreen = !fullscreen;
			if (fullscreen) {
				GraphicsManager.SetWindowState(OpenTK.WindowState.Fullscreen);
			} else {
				GraphicsManager.SetWindowState(OpenTK.WindowState.Normal);
			}
		}
	}
}
