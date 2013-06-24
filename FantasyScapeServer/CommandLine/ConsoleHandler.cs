using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FantasyScape.Server.CommandLine;

namespace FantasyScape.Server {
	class ConsoleHandler {
		static CommandQueue Commands = new CommandQueue();
		public static void Update() {
			if (Commands.LineAvailable) {
				string Command = Commands.ReadLine();
				ProcessCommand(Command.ToLower());
			}
		}

		private static void ProcessCommand(string Command) {
			if (Command == "save") {
				Console.WriteLine("Saving Game...");
				ResourceManager.Save();
				Console.WriteLine("Game Saved!");
			}
		}
	}
}
