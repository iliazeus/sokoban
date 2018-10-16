using System;
using System.IO;
using NUnit.Framework;

namespace Sokoban.Core.Tests
{
	[TestFixture]
	public class PuzzleMetadataTest
	{
		[Test]
		public void TestParsing()
		{
			var str =
				"Name: Test #01: FooBar\n" +
				"AuthorName : John Doe\n" +
				"AuthorEmail : jdoe@example.com\n" +
				"UnknownKeyFooBarBaz: foobar\n" +
				"CreationDate: 2018-10-03 10:00\n" +
				"Difficulty: Easy\n" +
				"\n" +
				"this is not a part of metadata\n" +
				"Name : foobar";
			var metadata = PuzzleMetadata.ParseFrom(new StringReader(str));
			
			Assert.That(metadata.Name, Is.EqualTo("Test #01: FooBar"));
			Assert.That(metadata.AuthorName, Is.EqualTo("John Doe"));
			Assert.That(metadata.AuthorEmail, Is.EqualTo("jdoe@example.com"));
			Assert.That(metadata.CreationDate,
			            Is.EqualTo(new DateTimeOffset(
			            	new DateTime(2018, 10, 3, 10, 0, 0))));
			Assert.That(metadata.Difficulty, Is.EqualTo(PuzzleDifficulty.Easy));
		}
		
		[Test]
		public void TestParsingExceptions()
		{
			var noColon = new StringReader("foobar");
			Assert.Throws(typeof(FormatException),
			              () => PuzzleMetadata.ParseFrom(noColon));
			
			var emptyKey = new StringReader("  : bar");
			Assert.Throws(typeof(FormatException),
			              () => PuzzleMetadata.ParseFrom(emptyKey));
			
			var emptyValue = new StringReader("foo :  ");
			Assert.Throws(typeof(FormatException),
			              () => PuzzleMetadata.ParseFrom(emptyValue));
			
			var duplicateKey = new StringReader("foo: bar\nfoo: bar");
			Assert.Throws(typeof(FormatException),
			              () => PuzzleMetadata.ParseFrom(duplicateKey));
			
			var invalidCreationDate = new StringReader("CreationDate: foobar");
			Assert.Throws(typeof(FormatException),
			              () => PuzzleMetadata.ParseFrom(invalidCreationDate));
		}
		
		[Test]
		public void TestPrinting()
		{
			string data0 =
				"Name: Test #01: FooBar\n" +
				"AuthorName : John Doe\n" +
				"AuthorEmail : jdoe@example.com\n" +
				"CreationDate: 2018-10-03 10:00";
			
			var meta1 = PuzzleMetadata.ParseFrom(new StringReader(data0));
			
			var writer1 = new StringWriter();
			meta1.PrintTo(writer1);
			var data1 = writer1.ToString();
			
			var meta2 = PuzzleMetadata.ParseFrom(new StringReader(data1));
			
			var writer2 = new StringWriter();
			meta2.PrintTo(writer2);
			var data2 = writer2.ToString();
			
			Assert.That(data1, Is.EqualTo(data2));
		}
	}
}
