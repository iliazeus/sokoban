using System;
using System.Collections.Generic;
using System.IO;

namespace Sokoban.Core.Extensions
{
	internal static class DictionaryExtensions
	{
		public static Dictionary<string, string> ParseFrom(TextReader reader)
		{
			var result = new Dictionary<string, string>();
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
				if (result.ContainsKey(key)) {
					throw new FormatException("invalid metadata format");
				}
				result.Add(key, value);
			}
			return result;
		}
		
		public static void PrintTo(this Dictionary<string, string> self,
		                           TextWriter writer)
		{
			foreach (var entry in self) {
				writer.WriteLine("{0}: {1}", entry.Key, entry.Value);
			}
		}
	}
}
