namespace Sokoban
{
	public struct Coords
	{
		public readonly int Row, Col;
		
		public Coords(int row, int col)
		{
			Row = row; Col = col;
		}
		
		public override bool Equals(object obj)
		{
			return (obj is Coords) && Equals((Coords)obj);
		}

		public bool Equals(Coords other)
		{
			return this.Row == other.Row && this.Col == other.Col;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * Row.GetHashCode();
				hashCode += 1000000009 * Col.GetHashCode();
			}
			return hashCode;
		}

		public static bool operator ==(Coords lhs, Coords rhs) {
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Coords lhs, Coords rhs) {
			return !(lhs == rhs);
		}
		
		public override string ToString()
		{
			return string.Format("Coords({0}, {1})", Row, Col);
		}

	}
}
