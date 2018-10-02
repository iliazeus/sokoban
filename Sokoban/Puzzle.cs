using System;
using System.IO;

namespace Sokoban
{
	public class Puzzle
	{
		public PuzzleMetadata Metadata { get; private set; }
		public State InitialState { get; private set; }
		
		public Puzzle(PuzzleMetadata metadata, State initialState)
		{
			Metadata = metadata;
			InitialState = initialState;
		}
		
		public static Puzzle ParseFrom(TextReader reader)
		{
			var metadata = PuzzleMetadata.ParseFrom(reader);
			var initialState = State.ParseFrom(reader);
			return new Puzzle(metadata, initialState);
		}
		
		public void PrintTo(TextWriter writer)
		{
			Metadata.PrintTo(writer);
			writer.WriteLine();
			InitialState.PrintTo(writer);
		}
	}
}
