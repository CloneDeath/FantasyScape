using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using FantasyScape.NetworkMessages.Code;
using GLImp;
using System.Drawing;

namespace FantasyScape.Client.Editor {
	class CodeEditor : WindowControl {
		CodeFile Resource;

		MultilineTextBox CodeArea;
		Base TopBar;
		Button Run;
		ListBox ErrorList;


		public CodeEditor(CodeFile resource) : base(DevelopmentMenu.Instance) {
			this.Resource = resource;
			this.SetSize(600, 400);
			this.SetPosition((int)MouseManager.GetMousePositionWindows().X, (int)MouseManager.GetMousePositionWindows().Y);

			CodeArea = new MultilineTextBox(this);
			CodeArea.AcceptTabs = true;
			CodeArea.SetSize(400, 400);
			CodeArea.Text = Resource.Source;
			CodeArea.Font = new Gwen.Font(MainCanvas.Renderer, "Consolas");
			CodeArea.Dock = Gwen.Pos.Fill;
			CodeArea.TextChanged +=new GwenEventHandler<EventArgs>(CodeArea_TextChanged);

			TopBar = new Base(this);
			TopBar.Dock = Gwen.Pos.Top;
			TopBar.SetSize(0, 25);
			{
				Run = new Button(TopBar);
				Run.Text = "Run";
				Run.Clicked += new GwenEventHandler<ClickedEventArgs>(Run_Clicked);
			}

			ErrorList = new ListBox(this);
			ErrorList.Dock = Gwen.Pos.Bottom;
			ErrorList.Height = 50;
		}

		void CodeArea_TextChanged(Base control, EventArgs args) {
			Resource.Source = CodeArea.Text;
			new UpdateCode(Resource).Send();
		}

		void Run_Clicked(Base control, ClickedEventArgs args) {
			Package.RecompilePackages();

			ErrorList.RemoveAllRows();
			foreach (CompilerError error in Resource.Errors) {
				ListBoxRow row = ErrorList.AddRow(error.ErrorText);
				row.UserData = error;
				row.DoubleClicked += new GwenEventHandler<ClickedEventArgs>(row_Clicked);
			}
		}

		void row_Clicked(Base sender, ClickedEventArgs arguments) {
			CompilerError error = sender.UserData as CompilerError;
			CodeArea.CursorPosition = new Point(error.Column - 1, error.Line - 1);
			CodeArea.CursorEnd = new Point(CodeArea.GetTextLine(error.Line - 1).Length, error.Line - 1);
			CodeArea.Focus();
		}
	}
}
