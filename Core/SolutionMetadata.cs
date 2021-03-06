﻿using System;
using System.Collections.Generic;
using System.IO;

using Sokoban.Core.Extensions;

namespace Sokoban.Core
{
	public class SolutionMetadata
	{
		public string Name { get; private set; }
		public string AuthorName { get; private set; }
		public string AuthorEmail { get; private set; }
		public DateTimeOffset? CreationDate { get; private set; }
		
		public SolutionMetadata(string name = null,
		                        string authorName = null,
		                        string authorEmail = null,
		                        DateTimeOffset? creationDate = null)
		{
			Name = name;
			AuthorName = authorName;
			AuthorEmail = authorEmail;
			CreationDate = creationDate;
		}
		
		public SolutionMetadata Clone()
		{
			return new SolutionMetadata(Name,
			                            AuthorName,
			                            AuthorEmail,
			                            CreationDate);
		}
		
		private SolutionMetadata() {}
		
		public static SolutionMetadata FromDictionary(IDictionary<string, string> dict)
		{
			var result = new SolutionMetadata();
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
		
		public Dictionary<string, string> ToDictionary()
		{
			var result = new Dictionary<string, string>();
			if (Name != null) result.Add("Name", Name);
			if (AuthorName != null) result.Add("AuthorName", AuthorName);
			if (AuthorEmail != null) result.Add("AuthorEmail", AuthorEmail);
			if (CreationDate != null) result.Add("CreationDate", CreationDate.ToString());
			return result;
		}
		
		public static SolutionMetadata ParseFrom(TextReader reader)
		{
			return FromDictionary(DictionaryExtensions.ParseFrom(reader));
		}
		
		public void PrintTo(TextWriter writer)
		{
			ToDictionary().PrintTo(writer);
		}
	}
}
