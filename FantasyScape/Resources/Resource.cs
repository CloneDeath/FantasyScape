using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.Resources {
	public abstract class Resource {
		public Guid ID;
		public string Name = "[New Resource]";
		public abstract void Load(string path);
	}
}
