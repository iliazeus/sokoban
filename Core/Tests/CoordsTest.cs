using System;
using NUnit.Framework;

namespace Sokoban.Core.Tests
{
	[TestFixture]
	public class CoordsTest
	{
		[Test]
		public void TestEquality()
		{
			var a1 = new Coords(1, 2);
			var a2 = new Coords(1, 2);
			var b = new Coords(2, 3);
			
			Assert.That(a1, Is.EqualTo(a2));
			Assert.That(a1, Is.Not.EqualTo(b));
			
			Assert.That(a1.GetHashCode(), Is.EqualTo(a2.GetHashCode()));
		}
	}
}
