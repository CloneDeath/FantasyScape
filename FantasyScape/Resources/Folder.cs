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
			string FolderPath = Path.Combine(path, GetIDString());
			if (!Directory.Exists(FolderPath)) {
				Directory.CreateDirectory(FolderPath);
			}

			SaveFolderInfo(FolderPath);
			this.SaveChildren(FolderPath);
		}

		protected virtual void SaveFolderInfo(string FolderDir) {
			XDocument doc = new XDocument();
			{
				XElement Package = new XElement("Folder");
				{
					XElement Name = new XElement("Name", this.Name);
					Package.Add(Name);
				}
				doc.Add(Package);
			}
			doc.Save(Path.Combine(FolderDir, "folder.info"));
		}

		internal void SaveChildren(string dir) {
			foreach (Resource child in Children) {
				child.Save(dir);
			}
		}

		public override void Load(string path) {
			string[] dirs = path.Split(Path.DirectorySeparatorChar);
			if (!Guid.TryParse(dirs.Last(), out this.ID)) {
				throw new Exception("Failed to parse GUID: " + dirs.Last());
			}
			LoadFolderInfo(path);
			this.LoadChildren(path);
		}

		protected virtual void LoadFolderInfo(string dir) {
			XDocument doc = XDocument.Load(new StreamReader(Path.Combine(dir, "folder.info")));

			XElement Package = doc.Descendants("Folder").First();

			if (Package == null) {
				throw new Exception("Malformed folder. Expected 'Folder' element.");
			}

			List<XElement> PackageInfo = new List<XElement>(Package.Descendants());

			foreach (XElement info in PackageInfo) {
				switch (info.Name.ToString()) {
					case "Name":
						this.Name = info.Value;
						break;
					default:
						throw new Exception("Unknown element in package '" + info.Name + "'.");
				}
			}
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

					case ".code":
						res = new CodeFile();
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
