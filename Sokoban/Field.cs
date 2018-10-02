using System;

namespace Sokoban
{
	public class Field
	{
		public enum Cell { Empty, Wall, Target }
		
		private readonly Cell[,] cells;
		
		public Field(Cell[,] cells)
		{
			this.cells = (Cell[,]) cells.Clone();
		}
		
		public Cell GetCellAt(Coords coords)
		{
			return this.cells[coords.Row, coords.Col];
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
