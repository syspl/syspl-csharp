// Copyright (C) 2014, 2017  Simon Mika <simon@mika.se>
//
// This file is part of SysPL.
//
// SysPL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SysPL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SysPL.  If not, see <http://www.gnu.org/licenses/>.
//

using Tasks = System.Threading.Tasks;
using IO = Kean.IO;
using Kean.IO.Extension;
using Text = Kean.Text;

namespace SysPL.Tokens
{
	public class Identifier :
		Token
	{
		public string Name { get; }
		public Identifier(string name, Text.Fragment source = null) :
			base(source)
		{
			this.Name = name;
		}
		public override string ToString() => this.Name;
		public override bool Equals(Token other) =>
			other is Identifier && this.Name == (other as Identifier).Name;
		public static bool StartsIdentifier(char? c) =>
			c.HasValue && (char.IsLetter(c.Value) || c == '_');
		public static bool IsWithinIdentifier(char? c) =>
			c.HasValue && (char.IsLetterOrDigit(c.Value) || c == '_');
		public static async Tasks.Task<Token> Parse(IO.ITextReader reader)
		{
			Token result = null;
			if (Identifier.StartsIdentifier(await reader.Peek())) // Keyword, Identifier, Boolean Literal or Null Literal
			{
				var mark = reader.Mark();
				result = Identifier.Parse(await reader.ReadUpTo(c => !Identifier.IsWithinIdentifier(c)), await mark.ToFragment());
			}
			return result;
		}
		public static Token Parse(string data, Text.Fragment source) =>
			Keyword.Parse(data, source) ?? new Identifier(data, source);
		}
}
