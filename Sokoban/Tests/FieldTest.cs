using System;
using NUnit.Framework;

namespace Sokoban.Tests
{
	[TestFixture]
	public class FieldTest
	{
		private static Field.Cell[,] cells = new Field.Cell[,] {
			{ Field.Cell.Empty, Field.Cell.Target, Field.Cell.Wall },
			{ Field.Cell.Empty, Field.Cell.Target, Field.Cell.Wall },
		};
		
		private Field field = new Field(cells);
		
		[Test]
		public void TestBounds()
		{
			Assert.That(field.Width, Is.EqualTo(3));
			Assert.That(field.Height, Is.EqualTo(2));
			
			Assert.That(field.IsInBounds(new Coords(1, 1)));
			Assert.That(! field.IsInBounds(new Coords(1, 3)));
			Assert.That(! field.IsInBounds(new Coords(0, -1)));
		}
		
		[Test]
		public void TestAccess()
		{
			Assert.That(field.GetCellAt(new Coords(1,1)),
			            Is.EqualTo(Field.Cell.Target));
			Assert.Throws(typeof(IndexOutOfRangeException),
			              () => field.GetCellAt(new Coords(2,1)));
		}
	}
}
