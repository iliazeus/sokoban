using System;
using System.Collections.Generic;
using System.IO;

namespace Sokoban.Core
{
	public class State
	{
		public Field Field { get; private set; }
		
		public Coords PlayerCoords { get; private set; }
		public Coords[] BoxesCoords { get; private set; }
		
		public bool IsValid { get; private set; }
		public bool IsWinning { get; private set; }
		
		public State(Field field,
		             Coords playerCoords, Coords[] boxesCoords)
		{
			Field = field;
			PlayerCoords = playerCoords;
			BoxesCoords = boxesCoords.Clone() as Coords[];
			IsValid = Validate();
			IsWinning = CheckWinCondition();
		}
		private State() {}
		
		private bool Validate()
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
		
		private bool CheckWinCondition()
		{
			foreach (var boxCoords in BoxesCoords) {
				if (Field.GetCellAt(boxCoords) != Field.Cell.Target) {
					return false;
				}
			}
			return true;
		}
		
		public bool ValidateMove(Move move)
		{
			var newPlayerCoords = new Coords(PlayerCoords, move);
			if (! Field.IsInBounds(newPlayerCoords)) return false;
			if (Field.GetCellAt(newPlayerCoords) == Field.Cell.Wall) {
				return false;
			}
			
			foreach (var boxCoords in BoxesCoords) {
				if (boxCoords != newPlayerCoords) continue;
				var newBoxCoords = new Coords(boxCoords, move);
				if (! Field.IsInBounds(newBoxCoords)) return false;
				if (Field.GetCellAt(newBoxCoords) == Field.Cell.Wall) {
					return false;
				}
				foreach (var otherBoxCoords in BoxesCoords) {
					if (newBoxCoords == otherBoxCoords) return false;
				}
			}
			
			return true;
		}
		
		public bool ValidateMoveSequence(IEnumerable<Move> sequence)
		{
			var state = Clone();
			foreach (var move in sequence) {
				if (! state.ValidateMove(move)) return false;
				state = state.ApplyMove(move);
			}
			return true;
		}
		
		public State ApplyMove(Move move)
		{
			if (! ValidateMove(move)) {
				throw new InvalidOperationException("invalid move");
			}
			var result = Clone();
			result.PlayerCoords = new Coords(PlayerCoords, move);
			for (int i = 0; i < result.BoxesCoords.Length; i++) {
				if (result.BoxesCoords[i] != result.PlayerCoords) continue;
				result.BoxesCoords[i] = new Coords(result.BoxesCoords[i], move);
			}
			result.IsValid = result.Validate();
			result.IsWinning = result.CheckWinCondition();
			return result;
		}
		
		public State ApplyMoveSequence(IEnumerable<Move> sequence)
		{
			var state = Clone();
			foreach (var move in sequence) state = state.ApplyMove(move);
			return state;
		}
		
		public State Clone()
		{
			var result = new State();
			result.Field = this.Field;
			result.PlayerCoords = this.PlayerCoords;
			result.BoxesCoords = this.BoxesCoords.Clone() as Coords[];
			result.IsValid = this.IsValid;
			result.IsWinning = this.IsWinning;
			return result;
		}
		
		public bool Equals(State other)
		{
			if (other == null) return false;
			if (this.PlayerCoords != other.PlayerCoords) return false;
			if (this.BoxesCoords.Length != other.BoxesCoords.Length) return false;
			for (int i = 0; i < this.BoxesCoords.Length; i++) {
				if (this.BoxesCoords[i] != other.BoxesCoords[i]) return false;
			}
			return true;
		}
		public override bool Equals(object obj)
		{
			return Equals(obj as State);
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
						cells[row,col] = Field.Cell.Empty;
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
						case 'A':
							cells[row,col] = Field.Cell.Target;
							playerCoords.Add(new Coords(row, col));
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
		
		public void PrintTo(TextWriter writer)
		{
			char[][] lines = new char[Field.Height][];
			for (int row = 0; row < Field.Height; row++) {
				lines[row] = new char[Field.Width];
				for (int col = 0; col < Field.Width; col++) {
					switch (Field.GetCellAt(row, col)) {
						case Field.Cell.Empty: lines[row][col] = '.'; break;
						case Field.Cell.Wall: lines[row][col] = '#'; break;
						case Field.Cell.Target: lines[row][col] = 'X'; break;
					}
				}
			}
			if (Field.GetCellAt(PlayerCoords) == Field.Cell.Target) {
				lines[PlayerCoords.Row][PlayerCoords.Col] = 'A';
			} else {
				lines[PlayerCoords.Row][PlayerCoords.Col] = '@';
			}
			foreach (var boxCoords in BoxesCoords) {
				if (Field.GetCellAt(boxCoords) == Field.Cell.Target) {
					lines[boxCoords.Row][boxCoords.Col] = 'O';
				} else {
					lines[boxCoords.Row][boxCoords.Col] = 'B';
				}
			}
			foreach (char[] line in lines) writer.WriteLine(line);
		}
	}
}
