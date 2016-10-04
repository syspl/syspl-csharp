// Copyright (C) 2010-2012  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Kean.Extension;
using System.Text.RegularExpressions;

namespace Kean.Uri
{
	public class PathLink :
		Collection.ILink<PathLink, string>,
		IEquatable<PathLink>
	{
		#region ILink<PathLink, string> Members
		public string Head { get; set; }
		public PathLink Tail { get; set; }
		#endregion
		public PathLink()
		{
		}
		public PathLink(string head, PathLink tail) :
			this()
		{
			this.Head = head;
			this.Tail = tail;
		}
		static readonly System.Collections.Generic.Dictionary<string, string> defaultFormats = new System.Collections.Generic.Dictionary<string, string> {
			{ "Time", "HH-mm-ss-fff" },
		};
		/// <summary>
		/// Recursively resolves a given variable in a PathLink, using a format specified as "$(variable:format)" in the PathLink string.
		/// If the variable has no format specifier i.e. "$(variable)", a default format, specified in defaultFormats above, is used.
		/// If no default format specifier has been defined, the variable is left untouched because we don't know what to do with it.
		/// </summary>
		/// <param name="variable">The name of variable we want to resolve.</param>
		/// <param name="format">A function that returns the variable formatted as specified in its input.</param>
		/// <returns>A new PathLink where the variable has been resolved, or perhaps not.</returns>
		public PathLink ResolveVariable(string variable, Func<string, string> format)
		{
			string head = this.Head;
			MatchCollection matches = Regex.Matches(head, @"(.*)(\$\(" + variable + @"\))(.*)"); // Variable without format specifier
			if (matches.Count == 1 && defaultFormats.Keys.Contains(variable))
				head = matches[0].Groups[1].Value + format(defaultFormats[variable]) + matches[0].Groups[3].Value;
			else
			{
				matches = Regex.Matches(head, @"(.*)(\$\(" + variable + @":)([^\)]+)(\))(.*)"); // Variable with format specifier
				if (matches.Count == 1)
					head = matches[0].Groups[1].Value + format(matches[0].Groups[3].Value) + matches[0].Groups[5].Value;
			}
			return new PathLink(head, this.Tail.NotNull() ? this.Tail.ResolveVariable(variable, format) : null);
		}
		PathLink RebuildHelper()
		{
			PathLink result;
			if (this.Head == "." || this.Head.IsEmpty())
				result = this.Tail.NotNull() ? this.Tail.RebuildHelper() : this;
			else if (this.Head == ".." && this.Tail.NotNull())
			{
				result = this.Tail.RebuildHelper();
				if (result.NotNull() && result.Head.NotEmpty() && result.Head != "." && result.Head != "..")
					result = result.Tail;
				else
					result = new PathLink("..", result);
			}
			else
				result = new PathLink(this.Head, this.Tail.NotNull() ? this.Tail.RebuildHelper() : null);
			return result;
		}
		public PathLink Rebuild()
		{
			return this.Head.NotEmpty() ? this.RebuildHelper() :
				this.Tail.NotNull() ? new PathLink(this.Head, this.Tail.RebuildHelper()) :
				this;
		}
		#region IEquatable<PathLink> Members
		public bool Equals(PathLink other)
		{
			return other.NotNull() && this.Head == other.Head && this.Tail == other.Tail;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is PathLink && this.Equals(other as PathLink);
		}
		public override int GetHashCode()
		{
			return this.Head.Hash() ^ this.Tail.Hash();
		}
		public override string ToString()
		{
			return this.Tail + "/" + this.Head;
		}
		#endregion
		#region Equality Operators
		public static bool operator ==(PathLink left, PathLink right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(PathLink left, PathLink right)
		{
			return !(left == right);
		}
		#endregion
	}
}