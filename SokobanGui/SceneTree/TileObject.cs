using System;

namespace SokobanGui.SceneTree
{
	public enum TileType
	{
		EmptyTile, WallTile, TargetTile
	}
	
	public class TileObject : SceneObject
	{
		public override ObjectType ObjectType { get { return ObjectType.Tile; }}
		
		private TileType tileType;
		public TileType TileType
		{
			get { return tileType; }
			private set { tileType = value; NotifyPropertyChanged("Type"); }
		}
		
		public TileObject(GameSession session, int row, int column) : base(session)
		{
			this.Row = row;
			this.Column = column;
			switch (session.CurrentState.Field.GetCellAt(Row, Column)) {
				case Sokoban.Field.Cell.Empty:
					TileType = TileType.EmptyTile;
					break;
				case Sokoban.Field.Cell.Target:
					TileType = TileType.TargetTile;
					break;
				case Sokoban.Field.Cell.Wall:
					TileType = TileType.WallTile;
					break;
			}
		}
	}
}
