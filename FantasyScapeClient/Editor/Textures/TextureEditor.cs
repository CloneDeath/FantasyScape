﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using System.Drawing;

namespace FantasyScape.Client.Editor {
	class TextureEditor : WindowControl {
		public TextureEditor(Base parent) : base(parent) {
			this.SetPosition(10, 10);
			this.SetSize(400, 400);
		}

		private void Save() {
			throw new NotImplementedException();
		}
	}
}