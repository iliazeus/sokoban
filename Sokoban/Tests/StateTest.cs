using System;
using NUnit.Framework;

namespace Sokoban.Tests
{
	[TestFixture]
	public class StateTest
	{
		private Field field = new Field(
			new Field.Cell[,] {
				{ Field.Cell.Empty, Field.Cell.Target, Field.Cell.Wall },
				{ Field.Cell.Empty, Field.Cell.Target, Field.Cell.Wall },
			}
		);
		
		private Coords validPlayerCoords = new Coords(1, 1);
		private Coords invalidPlayerCoords = new Coords(0, 2);
		
		private Coords[] validBoxesCoords = new Coords[] {
			new Coords(0, 0), new Coords(0, 1),
		};
		private Coords[] invalidBoxesCoords = new Coords[] {
			new Coords(0, 0), new Coords(1, 2),
		};
		
		[Test]
		public void TestEquality()
		{
			var s1 = new State(field, validPlayerCoords, validBoxesCoords);
			var s2 = new State(field, validPlayerCoords, validBoxesCoords);
			var t1 = new State(field, invalidPlayerCoords, validBoxesCoords);
			var t2 = new State(field, validPlayerCoords, invalidBoxesCoords);
			
			Assert.That(s1, Is.EqualTo(s2));
			Assert.That(s1, Is.Not.EqualTo(t1));
			Assert.That(s1, Is.Not.EqualTo(t2));
			
			Assert.That(s1.GetHashCode(), Is.EqualTo(s2.GetHashCode()));
		}
		
		[Test]
		public void TestCloning()
		{
			var s1 = new State(field, validPlayerCoords, validBoxesCoords);
			var s2 = (State) s1.Clone();
			
			Assert.That(s1, Is.EqualTo(s2));
			Assert.That(s1, Is.Not.SameAs(s2));
			
			Assert.That(s1.Field, Is.SameAs(s2.Field));
		}
		
		[Test]
		public void TestValidation()
		{
			Assert.That(new State(field, validPlayerCoords, validBoxesCoords)
			            .Validate());
			
			Assert.That(! new State(field, invalidPlayerCoords, validBoxesCoords)
			            .Validate());
			Assert.That(! new State(field, validPlayerCoords, invalidBoxesCoords)
			            .Validate());
			
			Assert.That(! new State(field, validBoxesCoords[0], validBoxesCoords)
			            .Validate());
		}
	}
}
