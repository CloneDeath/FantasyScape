using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FantasyScape.Server.CommandLine {
	class CommandQueue {
		Thread InputProcessor;
		List<string> Queue = new List<string>();
		public CommandQueue() {
			InputProcessor = new Thread(ListenForInput);
			InputProcessor.Start();
		}

		public void ListenForInput() {
			while (true) {
				string input = Console.ReadLine();
				lock (Queue) {
					Queue.Add(input);
				}
			}
		}

		~CommandQueue() {
			InputProcessor.Abort();
		}

		public bool LineAvailable {
			get {
				lock (Queue) {
					return Queue.Count != 0;
				}
			}
		}

		public string ReadLine() {
			if (LineAvailable) {
				string ret;
				lock (Queue) {
					ret = Queue[0];
					Queue.RemoveAt(0);
				}
				return ret;
			} else {
				return String.Empty;
			}
		}
	}
}
