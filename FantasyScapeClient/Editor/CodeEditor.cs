using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client.Editor {
	class CodeEditor : WindowControl {
		public CodeEditor(Base parent) : base(parent) {
			TextBox CodeArea = new TextBox(this);
			CodeArea.Multiline = true;
			CodeArea.SetSize(300, 300);
			
			this.SizeToChildren();
		}
	}
}
