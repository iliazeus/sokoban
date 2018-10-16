using System;
using System.IO;
using NUnit.Framework;

namespace Sokoban.Core.Tests
{
	[TestFixture]
	public class SolutionTest
	{
		const string data =
			"Name: Test #01: Parsing\n" +
			"AuthorName: John Doe\n" +
			"AuthorEmail: jdoe@example.com\n" +
			"CreationDate: 2018-10-03 10:00\n" +
			"\n" +
			"UDLR\n" +
			"DD\n";
		
		private Move[] moveSequence = new Move[] {
			Move.Up, Move.Down, Move.Left, Move.Right,
			Move.Down, Move.Down,
		};
		
		[Test]
		public void TestParsing()
		{
			var solution = Solution.ParseFrom(new StringReader(data));
			
			Assert.That(solution.Metadata.Name, Is.EqualTo("Test #01: Parsing"));
			Assert.That(solution.Metadata.AuthorName, Is.EqualTo("John Doe"));
			
			Assert.That(solution.Moves, Is.EquivalentTo(moveSequence));
		}
		
		[Test]
		public void TestPrinting()
		{
			var solution1 = Solution.ParseFrom(new StringReader(data));
			
			var writer1 = new StringWriter();
			solution1.PrintTo(writer1);
			var data1 = writer1.ToString();
			
			var solution2 = Solution.ParseFrom(new StringReader(data1));
			
			var writer2 = new StringWriter();
			solution2.PrintTo(writer2);
			var data2 = writer2.ToString();
			
			Assert.That(data1, Is.EqualTo(data2));
		}
	}
}
