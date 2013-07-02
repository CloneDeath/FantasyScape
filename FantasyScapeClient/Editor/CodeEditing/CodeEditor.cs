using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.Resources;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace FantasyScape.Client.Editor {
	class CodeEditor : WindowControl {
		CodeFile Resource;

		MultilineTextBox CodeArea;
		Base TopBar;
		Button Run;


		public CodeEditor(CodeFile resource) : base(DevelopmentMenu.Instance) {
			this.Resource = resource;
			this.SetSize(400, 400);

			CodeArea = new MultilineTextBox(this);
			CodeArea.AcceptTabs = true;
			CodeArea.SetSize(400, 400);
			CodeArea.Text = Resource.Code;
			CodeArea.Font = new Gwen.Font(MainCanvas.Renderer, "Consolas");
			CodeArea.Dock = Gwen.Pos.Fill;

			TopBar = new Base(this);
			TopBar.Dock = Gwen.Pos.Top;
			TopBar.SetSize(0, 25);
			{
				Run = new Button(TopBar);
				Run.Text = "Run";
				Run.Clicked += new GwenEventHandler(Run_Clicked);
			}			
		}

		void Run_Clicked(Base control) {
			var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
			var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" });
			parameters.GenerateExecutable = false;
			CompilerResults results = csc.CompileAssemblyFromSource(parameters, CodeArea.Text);
			results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));

			if (results.Errors.Count == 0) {
				Module module = results.CompiledAssembly.GetModules()[0];

				foreach (Type mt in module.GetTypes()){
					MethodInfo methInfo = mt.GetMethod("Main");
					if (methInfo != null) {
						methInfo.Invoke(null, new object[0]);
					}
				}
			}
		}
	}
}
