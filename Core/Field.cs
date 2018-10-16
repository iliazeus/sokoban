using System;

namespace Sokoban.Core
{
	public class Field
	{
		public enum Cell { Empty, Wall, Target }
		
		private readonly Cell[,] cells;
		
		public Field(Cell[,] cells)
		{
			this.cells = (Cell[,]) cells.Clone();
		}
		
		public Cell GetCellAt(int row, int col)
		{
			return this.cells[row,col];
		}
		public Cell GetCellAt(Coords coords)
		{
			return GetCellAt(coords.Row, coords.Col);
		}
		public Cell this[int row, int col]
		{
			get { return GetCellAt(row, col); }
		}
		public Cell this[Coords coords]
		{
			get { return GetCellAt(coords); }
		}
		
		public int Height { get { return cells.GetLength(0); } }
		public int Width { get { return cells.GetLength(1); } }
		
		public bool IsInBounds(Coords coords)
		{
			if (coords.Row < 0) return false;
			if (coords.Row >= Height) return false;
			if (coords.Col < 0) return false;
			if (coords.Col >= Width) return false;
			return true;
		}
	}
}
