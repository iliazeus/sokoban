using System;

namespace SokobanGui
{
	public enum TileType
	{
		EmptyTile, WallTile, TargetTile
	}
	
	public class TileObject : GameObject
	{
		private TileType type;
		public TileType Type
		{
			get { return type; }
			private set { type = value; NotifyPropertyChanged("Type"); }
		}
		
		public TileObject(Sokoban.State state, int row, int column)
			: base(state, row, column) {}
		
		public override Sokoban.State State
		{
			get { return base.State; }
			set
			{
				if (base.State == null || base.State.Field != value.Field) {
					switch (value.Field.GetCellAt(Row, Column)) {
						case Sokoban.Field.Cell.Empty:
							Type = TileType.EmptyTile;
							break;
						case Sokoban.Field.Cell.Target:
							Type = TileType.TargetTile;
							break;
						case Sokoban.Field.Cell.Wall:
							Type = TileType.WallTile;
							break;
					}
				}
				base.State = value;
			}
		}
	}
}
