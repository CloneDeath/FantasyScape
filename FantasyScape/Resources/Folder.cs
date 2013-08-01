using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lidgren.Network;
using System.Xml.Linq;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Resources {
	public class Folder : Resource {
		private List<Resource> Children = new List<Resource>();

		public Folder() {
			ID = Guid.NewGuid();
		}

		public override void Save(string path) {
			base.Save(Path.Combine(path, "folder.info"));
			this.SaveChildren(path);
		}

		internal void SaveChildren(string dir) {
			foreach (Resource child in Children) {
				child.Save(dir);
			}
		}

		public static new Folder Load(string path) {
			Folder dir = (Folder)Resource.Load(Path.Combine(path, "folder.info"), typeof(Folder));
			dir.LoadChildren(path);
			return dir;
		}

		public void LoadChildren(string dir) {
			foreach (string file in Directory.GetFiles(dir)) {
				Type restype;
				switch (Path.GetExtension(file)) {
					case ".block":
						restype = typeof(BlockType);
						break;
					case ".tex":
						FSTexture resource = (FSTexture)FSTexture.Load(file, typeof(FSTexture));
						resource.Load(new System.Drawing.Bitmap(file.Replace("tex", "png")));
						resource.ID = Guid.Parse(Path.GetFileNameWithoutExtension(file));
						Children.Add(resource);
						continue;
					case ".code":
						restype = typeof(CodeFile);
						break;
					default:
						continue;
				}
				Resource res = Resource.Load(file, restype);
				res.ID = Guid.Parse(Path.GetFileNameWithoutExtension(file));
				Children.Add(res);
			}

			foreach (string child in Directory.GetDirectories(dir)) {
				Folder f = Folder.Load(child);
				f.ID = Guid.Parse(Path.GetFileName(child));
				Children.Add(f);
			}
		}

		internal override Resource GetResource(Guid ResourceID) {
			if (this.ID == ResourceID) {
				return this;
			} else {
				foreach (Resource Child in Children) {
					Resource ret = Child.GetResource(ResourceID);
					if (ret != null) {
						return ret;
					}
				}
				return null;
			}
		}

		public void Add(Resource res) {
			Children.Add(res);

			TriggerUpdateEvent(this);
			Package.TriggerOnChangeEvent();
		}

		public IEnumerable<Resource> GetChildren() {
			return Children;
		}

		public override void SendUpdate() {
			new UpdateFolder(this).Send();
		}

		public IEnumerable<Resource> GetAllChildren() {
			List<Resource> resources = new List<Resource>(GetChildren());
			foreach (Resource child in GetChildren()) {
				if (child is Folder) {
					Folder fld = child as Folder;
					resources.AddRange(fld.GetChildren());
				}
			}
			return resources;
		}
	}
}
