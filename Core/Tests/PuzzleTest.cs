using System;
using System.IO;
using NUnit.Framework;

namespace Sokoban.Core.Tests
{
	[TestFixture]
	public class PuzzleTest
	{
		private string data =
			"Name: Test #01: Parsing\n" +
			"AuthorName: John Doe\n" +
			"AuthorEmail: jdoe@example.com\n" +
			"CreationDate: 2018-10-03 10:00\n" +
			"\n" +
			"########\n" +
			"#......#\n" +
			"#.@OB.#\n" +
			"#..X.#\n" +
			"#####";
		
		[Test]
		public void TestParsing()
		{
			var puzzle = Puzzle.ParseFrom(new StringReader(data));
			
			Assert.That(puzzle.Metadata.Name, Is.EqualTo("Test #01: Parsing"));
			Assert.That(puzzle.Metadata.AuthorName, Is.EqualTo("John Doe"));
			
			Assert.That(puzzle.InitialState.PlayerCoords,
			            Is.EqualTo(new Coords(2,2)));
		}
		
		[Test]
		public void TestPrinting()
		{
			var puzzle1 = Puzzle.ParseFrom(new StringReader(data));
			
			var writer1 = new StringWriter();
			puzzle1.PrintTo(writer1);
			var data1 = writer1.ToString();
			
			var puzzle2 = Puzzle.ParseFrom(new StringReader(data1));
			
			var writer2 = new StringWriter();
			puzzle2.PrintTo(writer2);
			var data2 = writer2.ToString();
			
			Assert.That(data1, Is.EqualTo(data2));
		}
	}
}
