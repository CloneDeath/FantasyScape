using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using FantasyScape.NetworkMessages;

namespace FantasyScape.Client.Editor {
	class DeleteBlockWindow : WindowControl {
		public DeleteBlockWindow() : base(MainCanvas.GetCanvas()) {
			this.SetSize(200, 200);
			this.SetPosition(50, 50);

			ComboBox types = new ComboBox(this);
			types.SetPosition(10, 10);
			foreach (BlockType b in BlockTypes.GetAll()) {
				types.AddItem(b.Name, "", b.Name);
			}

			Button delete = new Button(this);
			delete.SetPosition(10, 40);
			delete.Clicked += delegate(Base sender) {
				BlockTypes.Remove((string)types.UserData);

				BlockTypeRemove btr = new BlockTypeRemove((string)types.UserData);
				btr.Send();
			};

		}
	}
}
