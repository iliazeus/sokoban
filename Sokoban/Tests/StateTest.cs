using System;
using System.IO;
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
		
		[Test]
		public void TestWinCondition()
		{
			var targetCoords = new Coords[] {
				new Coords(0, 1), new Coords(1, 1)
			};
			var winState = new State(field, new Coords(0, 0), targetCoords);
			var justState = new State(field, validPlayerCoords, validBoxesCoords);
			var invalidState = new State(field, invalidPlayerCoords, invalidBoxesCoords);
			
			Assert.That(winState.CheckWinCondition());
			Assert.That(! justState.CheckWinCondition());
			Assert.That(! invalidState.CheckWinCondition());
		}
		
		[Test]
		public void TestParsing()
		{
			string data =
				"########\n" +
				"#..@..#\n" + 
				"#XBO.#\n" +
				"#####";
			var state = State.ParseFrom(new StringReader(data));
			
			Assert.That(state.PlayerCoords,
			            Is.EqualTo(new Coords(1, 3)));
			
			var expectedBoxesCoords = new Coords[] {
				new Coords(2, 2), new Coords(2, 3)
			};
			Assert.That(state.BoxesCoords,
			            Is.EquivalentTo(expectedBoxesCoords));
			
			Assert.That(state.Field.GetCellAt(new Coords(2, 0)),
			            Is.EqualTo(Field.Cell.Wall));
			Assert.That(state.Field.GetCellAt(new Coords(2, 1)),
			            Is.EqualTo(Field.Cell.Target));
			Assert.That(state.Field.GetCellAt(new Coords(2, 2)),
			            Is.EqualTo(Field.Cell.Empty));
			
			Assert.That(state.Validate());
		}
		
		[Test]
		public void TestParsingExceptions()
		{
			Assert.Throws(typeof(FormatException),
			              () => State.ParseFrom(new StringReader("@@@")));
			Assert.Throws(typeof(FormatException),
			              () => State.ParseFrom(new StringReader("##FooBar")));
		}
	}
}
