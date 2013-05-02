using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using Lidgren.Network;

namespace FantasyScape {
	class NetTexture : Texture {
		public NetTexture(string filename) : base(filename) {
			
		}


		internal void Send(NetConnection netConnection, NetServer Server) {
			NetOutgoingMessage nom = Server.CreateMessage();
			nom.Write("NetTexture");
			nom.Write(this.Name);
			nom.Write(GetBytes());
		}
	}
}
