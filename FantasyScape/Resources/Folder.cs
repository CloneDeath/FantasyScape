using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FantasyScape.Resources {
	public class Folder : Resource {
		public List<Resource> Children = new List<Resource>();

		public Folder() {
			ID = Guid.NewGuid();
		}

		public override void Load(string path) {
			string[] dirs = path.Split(Path.DirectorySeparatorChar);
			this.Name = dirs.Last();
			this.LoadChildren(path);
		}

		public void LoadChildren(string dir) {
			string[] Files = Directory.GetFiles(dir);
			foreach (string file in Files) {
				string FileType = Path.GetExtension(file);
				Resource res = null;
				switch (FileType) {
					case ".tex":
						res = new FSTexture();
						break;

					case ".block":
						res = new BlockType();
						break;
				}
				if (res != null) {
					res.Load(file);
					Children.Add(res);
				}
			}

			string[] folders = Directory.GetDirectories(dir);
			foreach (string child in folders) {
				Folder f = new Folder();
				f.Load(child);
				Children.Add(f);
			}
		}
	}
}
