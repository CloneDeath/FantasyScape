using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using System.Drawing;
using GLImp;
using FantasyScape.NetworkMessages;
using FantasyScape.Resources;

namespace FantasyScape.Client.Editor {
	class TextureEditor : WindowControl {
		DrawingArea Canvas;
		ToolBox ToolSelect;
		HSVColorPicker colorpicker;

		public TextureEditor(FSTexture texture) : base(DevelopmentMenu.Instance) {
            this.Title = "Texture Editor";
			this.SetSize(600, 400);
            this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);

            Canvas = new DrawingArea(this);
			Canvas.SetPosition(10, 10);
			Canvas.SetSize(300, 300);

            colorpicker = new HSVColorPicker(this);
			colorpicker.SetPosition(320, 10);
			colorpicker.ColorChanged += delegate(Base sender, EventArgs args) {
				Canvas.SetColor(colorpicker.SelectedColor);
			};

            ToolSelect = new ToolBox(this);
			ToolSelect.Text = "Current Tool";
			ToolSelect.SetPosition(320, 150);
			ToolSelect.SetSize(200, 200);

			Canvas.OnDraw += DrawEventHandler;

            Canvas.LoadTexture(texture);
		}

		private void DrawEventHandler(int X, int Y, PixelData Data, ref Color CurrentColor) {
			ToolSelect.UseTool(X, Y, Data, ref CurrentColor);
			colorpicker.SetColor(CurrentColor);
		}
	}
}
