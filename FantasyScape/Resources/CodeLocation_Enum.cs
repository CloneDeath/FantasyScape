using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FantasyScape.Resources {
	[Flags]
	public enum CodeLocation {
		Client = 0x01,
		Server = 0x02, 
		Shared = 0x03, //Both
	}
}
