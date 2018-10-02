using System;

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
	}
}
