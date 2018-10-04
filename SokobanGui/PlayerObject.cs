using System;

namespace SokobanGui
{
	public class PlayerObject : GameObject
	{
		public override Sokoban.State State
		{
			get { return base.State; }
			set
			{
				base.State = value;
				Row = value.PlayerCoords.Row;
				Column = value.PlayerCoords.Col;
			}
		}
		
		public PlayerObject(Sokoban.State state) : base(state) {}
	}
}
