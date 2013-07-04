using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using FantasyScape.Resources;

namespace FantasyScape.CodeCompilation {
	class CodeManager {
		internal static void Compile(List<CodeFile> CodeFiles)
		{
			if (CodeFiles.Count() == 0) return;

			CSharpCodeProvider csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
			CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "FantasyScape.dll" });
			parameters.GenerateExecutable = false;

			List<string> CodeSources = new List<string>();
			foreach (CodeFile code in CodeFiles) {
				code.ClearErrors();
				CodeSources.Add(code.Source);
			}
			CompilerResults results = csc.CompileAssemblyFromSource(parameters, CodeSources.ToArray());

			//if (results.Errors.Count == 0) {
			//    Module module = results.CompiledAssembly.GetModules()[0];

			//    foreach (Type mt in module.GetTypes()) {
			//        MethodInfo methInfo = mt.GetMethod("Main");
			//        if (methInfo != null) {
			//            methInfo.Invoke(null, new object[0]);
			//        }
			//    }
			//}

			foreach (CompilerError error in results.Errors) {
				int CodeIndex = GetCodeIndex(error.FileName);
				CodeFiles[CodeIndex].AddError(error);
			}
		}

		private static int GetCodeIndex(string filename) {
			string[] parts = filename.Split('.');

			//Code index is 2nd to last part
			return Int32.Parse(parts[parts.Count() - 2]);
		}
	}
}
