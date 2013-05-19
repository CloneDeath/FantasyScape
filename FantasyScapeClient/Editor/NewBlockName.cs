using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;

namespace FantasyScape.Client.Editor {
	class NewBlockName : WindowControl {
		public NewBlockName(Base parent) : base(parent.Parent){
			this.SetSize(200, 100);

			TextBox Entry = new TextBox(this);
			Entry.SetPosition(10, 10);
			Entry.SetSize(180, 30);
			Entry.Text = "Block #" + BlockTypes.GetAll().Count;

			Button OK = new Button(this);
			OK.SetSize(50, 20);
			OK.SetPosition(130, 45);
			OK.Text = "OK";
			OK.Clicked += delegate(Base sender) {
				//Todo, call some function to get ready to add shit
				this.Close();
			};

			Button Cancel = new Button(this);
			Cancel.SetSize(50, 20);
			Cancel.SetPosition(10, 45);
			Cancel.Text = "Cancel";
			Cancel.Clicked += delegate(Base sender) {
				this.Close();
			};
		}
	}
}
