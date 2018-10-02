using System;
using System.Collections.Generic;
using System.IO;

using Sokoban.Extensions;

namespace Sokoban
{
	public class PuzzleMetadata
	{
		public string Name { get; set; }
		public string AuthorName { get; set; }
		public string AuthorEmail { get; set; }
		public DateTimeOffset? CreationDate { get; set; }
		
		public PuzzleMetadata(string name = null,
		                      string authorName = null,
		                      string authorEmail = null,
		                      DateTimeOffset? creationDate = null)
		{
			Name = name;
			AuthorName = authorName;
			AuthorEmail = authorEmail;
			CreationDate = creationDate;
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
				}
			}
			return result;
		}
		
		public static PuzzleMetadata ParseFrom(TextReader reader)
		{
			return FromDictionary(DictionaryExtensions.ParseFrom(reader));
		}
	}
}
