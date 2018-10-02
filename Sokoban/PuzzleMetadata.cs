using System;
using System.Collections.Generic;
using System.IO;

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
			var metadata = new Dictionary<string, string>();
			string line;
			while (! string.IsNullOrEmpty(line = reader.ReadLine())) {
				int colonIndex = line.IndexOf(':');
				if (colonIndex < 0) {
					throw new FormatException("invalid metadata format");
				}
				var key = line.Substring(0, colonIndex).Trim();
				var value = line.Substring(colonIndex + 1).Trim();
				if (key.Length == 0 || value.Length == 0) {
					throw new FormatException("invalid metadata format");
				}
				if (metadata.ContainsKey(key)) {
					throw new FormatException("invalid metadata format");
				}
				metadata.Add(key, value);
			}
			return FromDictionary(metadata);
		}
	}
}
