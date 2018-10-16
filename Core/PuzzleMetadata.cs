using System;
using System.Collections.Generic;
using System.IO;

using Sokoban.Core.Extensions;

namespace Sokoban.Core
{
	public enum PuzzleDifficulty : int
	{
		Elementary = 1, Easy = 2, Medium = 3, Hard = 4, Insane = 5
	}
	
	public class PuzzleMetadata
	{
		public string Name { get; private set; }
		public string AuthorName { get; private set; }
		public string AuthorEmail { get; private set; }
		public DateTimeOffset? CreationDate { get; private set; }
		public PuzzleDifficulty? Difficulty { get; private set; }
		
		public PuzzleMetadata(string name = null,
		                      string authorName = null,
		                      string authorEmail = null,
		                      DateTimeOffset? creationDate = null,
		                      PuzzleDifficulty? difficulty = null)
		{
			Name = name;
			AuthorName = authorName;
			AuthorEmail = authorEmail;
			CreationDate = creationDate;
			Difficulty = difficulty;
		}
		
		public PuzzleMetadata Clone()
		{
			return new PuzzleMetadata(Name,
			                          AuthorName,
			                          AuthorEmail,
			                          CreationDate,
			                          Difficulty);
		}
		
		private PuzzleMetadata() {}
		
		public static PuzzleMetadata FromDictionary(IDictionary<string, string> dict)
		{
			var result = new PuzzleMetadata();
			foreach (var entry in dict) {
				switch (entry.Key) {
					case "Name":
						result.Name = entry.Value;
						break;
					case "AuthorName":
						result.AuthorName = entry.Value;
						break;
					case "AuthorEmail":
						result.AuthorEmail = entry.Value;
						break;
					case "CreationDate":
						result.CreationDate = DateTimeOffset.Parse(entry.Value);
						break;
					case "Difficulty":
						result.Difficulty =
							(PuzzleDifficulty) Enum.Parse(typeof(PuzzleDifficulty), entry.Value);
						break;
				}
			}
			return result;
		}
		
		public Dictionary<string, string> ToDictionary()
		{
			var result = new Dictionary<string, string>();
			if (Name != null) result.Add("Name", Name);
			if (AuthorName != null) result.Add("AuthorName", AuthorName);
			if (AuthorEmail != null) result.Add("AuthorEmail", AuthorEmail);
			if (CreationDate != null) result.Add("CreationDate", CreationDate.ToString());
			if (Difficulty != null) result.Add("Difficulty", Difficulty.ToString());
			return result;
		}
		
		public static PuzzleMetadata ParseFrom(TextReader reader)
		{
			return FromDictionary(DictionaryExtensions.ParseFrom(reader));
		}
		
		public void PrintTo(TextWriter writer)
		{
			ToDictionary().PrintTo(writer);
		}
	}
}
