using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor {
	class CodeEditor : WindowControl {
		public CodeEditor(CodeFile resource) : base(DevelopmentMenu.Instance) {
			TextBoxMultiline CodeArea = new TextBoxMultiline(this);
			CodeArea.SetSize(300, 300);
			
			this.SizeToChildren();
		}
	}
}
