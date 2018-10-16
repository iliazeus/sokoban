using System;
using System.Collections.Generic;
using System.IO;

namespace Sokoban.Core
{
	public class Solution
	{
		public SolutionMetadata Metadata { get; private set; }
		public IList<Move> Moves { get; private set; }
		
		public Solution(SolutionMetadata metadata, IEnumerable<Move> moves)
		{
			Metadata = metadata;
			Moves = new List<Move>(moves);
		}
		
		private static List<Move> ParseMoveSequenceFrom(TextReader reader)
		{
			var result = new List<Move>();
			string line;
			while (! string.IsNullOrEmpty(line = reader.ReadLine())) {
				foreach (char c in line) {
					if (char.IsWhiteSpace(c)) continue;
					switch (c) {
						case 'U': result.Add(Move.Up); break;
						case 'D': result.Add(Move.Down); break;
						case 'L': result.Add(Move.Left); break;
						case 'R': result.Add(Move.Right); break;
						default:
							throw new FormatException("unexpected character");
					}
				}
			}
			return result;
		}
		
		private void PrintMoveSequenceTo(TextWriter writer)
		{
			int i = 0;
			while (i < Moves.Count) {
				for (int j = 0; i < Moves.Count && j < 70; i++, j++) {
					switch (Moves[i]) {
						case Move.Up: writer.Write('U'); break;
						case Move.Down: writer.Write('D'); break;
						case Move.Left: writer.Write('L'); break;
						case Move.Right: writer.Write('R'); break;
					}
				}
				writer.WriteLine();
			}
		}
		
		public static Solution ParseFrom(TextReader reader)
		{
			var metadata = SolutionMetadata.ParseFrom(reader);
			var moves = ParseMoveSequenceFrom(reader);
			return new Solution(metadata, moves);
		}
		
		public void PrintTo(TextWriter writer)
		{
			Metadata.PrintTo(writer);
			writer.WriteLine();
			PrintMoveSequenceTo(writer);
		}
	}
}
