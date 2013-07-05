using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using FantasyScape.Resources;

namespace FantasyScape.CodeCompilation {
	public class CodeManager {
		CompilerResults Assembly;
		internal void Compile(List<CodeFile> CodeFiles)
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
			Assembly = csc.CompileAssemblyFromSource(parameters, CodeSources.ToArray());

			foreach (CompilerError error in Assembly.Errors) {
				int CodeIndex = GetCodeIndex(error.FileName);
				CodeFiles[CodeIndex].AddError(error);
			}
		}

		private static int GetCodeIndex(string filename) {
			string[] parts = filename.Split('.');

			//Code index is 2nd to last part
			return Int32.Parse(parts[parts.Count() - 2]);
		}

		public void RunMain() {
			if (Assembly.Errors.HasErrors) {
				throw new Exception("Error! The assembly has errors!");
			}

			foreach (Module module in Assembly.CompiledAssembly.GetModules()) {
				foreach (Type mt in module.GetTypes()) {
					MethodInfo methInfo = mt.GetMethod("Main");
					if (methInfo != null) {
						methInfo.Invoke(null, new object[0]);
					}
				}
			}
		}

		public List<WorldGenerator> GetWorldGens() {
			List<WorldGenerator> ret = new List<WorldGenerator>();
			foreach (Module module in Assembly.CompiledAssembly.GetModules()) {
				foreach (Type mt in module.GetTypes()) {
					if (typeof(WorldGenerator).IsAssignableFrom(mt)) {
						ConstructorInfo info = mt.GetConstructor(new Type[0]);
						if (info != null) {
							ret.Add((WorldGenerator)info.Invoke(new object[0]));
						}
					}
				}
			}
			return ret;
		}
	}
}
