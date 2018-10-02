using System;
using System.Collections.Generic;
using System.IO;

namespace Sokoban
{
	public class State : ICloneable
	{
		public Field Field { get; set; }
		
		public Coords PlayerCoords { get; set; }
		public Coords[] BoxesCoords { get; set; }
		
		public State(Field field,
		             Coords playerCoords, Coords[] boxesCoords)
		{
			Field = field;
			PlayerCoords = playerCoords;
			BoxesCoords = (Coords[]) boxesCoords.Clone();
		}
		
		public bool Validate()
		{
			if (! Field.IsInBounds(PlayerCoords)) return false;
			if (Field.GetCellAt(PlayerCoords) == Field.Cell.Wall) {
				return false;
			}
			
			for (int i = 0; i < BoxesCoords.Length; i++) {
				if (! Field.IsInBounds(BoxesCoords[i])) return false;
				if (Field.GetCellAt(BoxesCoords[i]) == Field.Cell.Wall) {
					return false;
				}
				if (BoxesCoords[i] == PlayerCoords) return false;
				for (int j = i + 1; j < BoxesCoords.Length; j++) {
					if (BoxesCoords[i] == BoxesCoords[j]) return false;
				}
			}
			
			return true;
		}
		
		public object Clone()
		{
			return new State(Field,
			                 PlayerCoords, (Coords[])BoxesCoords.Clone());
		}
		
		public override bool Equals(object obj)
		{
			State other = obj as State;
			if (other == null) return false;
			
			if (this.PlayerCoords != other.PlayerCoords) return false;
			if (this.BoxesCoords.Length != other.BoxesCoords.Length) return false;
			for (int i = 0; i < this.BoxesCoords.Length; i++) {
				if (this.BoxesCoords[i] != other.BoxesCoords[i]) return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * PlayerCoords.GetHashCode();
				if (BoxesCoords != null) {
					foreach (var boxCoords in BoxesCoords) {
						hashCode = 1000000009*hashCode + boxCoords.GetHashCode();
					}
				}
			}
			return hashCode;
		}
		
		public static State ParseFrom(TextReader reader)
		{
			var lines = new List<string>();
			int maxLineLength = 0;
			
			string line;
			while (! string.IsNullOrEmpty(line = reader.ReadLine())) {
				lines.Add(line);
				if (maxLineLength < line.Length) maxLineLength = line.Length;
			}
			
			int height = lines.Count, width = maxLineLength;
			var cells = new Field.Cell[height, width];
			var playerCoords = new List<Coords>();
			var boxesCoords = new List<Coords>();
			
			for (int row = 0; row < height; row++) {
				for (int col = 0; col < width; col++) {
					if (col >= lines[row].Length) {
						cells[row,col] = Field.Cell.Wall;
						continue;
					}
					switch (lines[row][col]) {
						case '.':
							cells[row,col] = Field.Cell.Empty;
							break;
						case '#':
							cells[row,col] = Field.Cell.Wall;
							break;
						case 'X':
							cells[row,col] = Field.Cell.Target;
							break;
						case '@':
							cells[row,col] = Field.Cell.Empty;
							playerCoords.Add(new Coords(row, col));
							break;
						case 'B':
							cells[row,col] = Field.Cell.Empty;
							boxesCoords.Add(new Coords(row, col));
							break;
						case 'O':
							cells[row,col] = Field.Cell.Target;
							boxesCoords.Add(new Coords(row, col));
							break;
						default:
							throw new FormatException("unexpected character");
					}
				}
			}
			
			if (playerCoords.Count == 0) {
				throw new FormatException("no starting position specified");
			}
			if (playerCoords.Count > 1) {
				throw new FormatException("multiple starting positions specified");
			}
			
			var field = new Field(cells);
			var state = new State(field, playerCoords[0], boxesCoords.ToArray());
			
			return state;
		}
	}
}
