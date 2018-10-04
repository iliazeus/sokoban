using System;

namespace SokobanGui
{
	public class BoxObject : GameObject
	{
		private int index;
		
		public override Sokoban.State State
		{
			get { return base.State; }
			set
			{
				base.State = value;
				Row = value.BoxesCoords[index].Row;
				Column = value.BoxesCoords[index].Col;
			}
		}
		
		public BoxObject(Sokoban.State state, int index) : base(state)
		{
			this.index = index;
		}
	}
}
