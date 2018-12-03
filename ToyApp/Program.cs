using System;
using System.IO;

using Sokoban.Core;

namespace Sokoban.ToyApp
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length != 1) {
				Console.WriteLine("Usage: ToyApp <puzzle_file>");
				return;
			}
			
			var puzzleFilePath = args[0];
			
			Puzzle puzzle;
			using (var reader = File.OpenText(puzzleFilePath)) {
				puzzle = Puzzle.ParseFrom(reader);
			}
			
			Console.Clear();
			puzzle.PrintTo(Console.Out);
			
			var currentState = puzzle.InitialState;
			while (! currentState.IsWinning) {
				Console.CursorTop -= currentState.Field.Height;
				Console.CursorLeft = 0;
				
				currentState.PrintTo(Console.Out);
				
				Move? move = null;
				while (move == null) {
					var key = Console.ReadKey(true);
					switch (key.Key) {
						case ConsoleKey.UpArrow: case ConsoleKey.K:
							move = Move.Up;
							break;
						case ConsoleKey.DownArrow: case ConsoleKey.J:
							move = Move.Down;
							break;
						case ConsoleKey.LeftArrow: case ConsoleKey.H:
							move = Move.Left;
							break;
						case ConsoleKey.RightArrow: case ConsoleKey.L:
							move = Move.Right;
							break;
						case ConsoleKey.Escape: case ConsoleKey.Q:
							return;
					}
					if (move != null && !currentState.ValidateMove(move.Value)) {
						move = null;
					}
				}
				
				currentState = currentState.ApplyMove(move.Value);
			}
			
			Console.WriteLine("The Winner Is You");
			return;
		}
	}
}