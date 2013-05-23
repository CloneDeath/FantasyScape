using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Client.Editor.Tools;

namespace FantasyScape.Client.Editor {
	class ToolBox : RadioButtonGroup {
		List<ITool> Tools;
		ITool CurrentTool;

		public ToolBox(Base parent) : base(parent){
			InitializeTools();
			for (int i = 0; i < Tools.Count; i++) {
				LabeledRadioButton btn = this.AddOption(Tools[i].GetDisplayName());
				//btn.SetPosition(10, 10 + (i * 30));
				btn.UserData = Tools[i];
			}

			this.SelectionChanged += ToolChanged;
			this.SetSelection(0);
		}

		private void ToolChanged(Base sender) {
			CurrentTool = (ITool)this.Selected.UserData;
		}

		private void InitializeTools() {
			Tools = new List<ITool>();
			Tools.Add(new Brush());
			Tools.Add(new ColorSelection());
			Tools.Add(new Fill());
		}

		public void UseTool(int X, int Y, PixelData Data, ref System.Drawing.Color CurrentColor) {
			CurrentTool.UseTool(X, Y, Data, ref CurrentColor);
		}
	}
}
