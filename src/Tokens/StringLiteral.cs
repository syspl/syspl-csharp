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
using Kean.Extension;
using Text = Kean.Text;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.Tokens
{
	public class StringLiteral :
		Literal
	{
		public string Value { get; }
		public StringLiteral(string value, Text.Fragment source = null) :
			base(source)
		{
			this.Value = value;
		}
		public override string ToString()
		{
			return "\"" + this.Value.Escape() + "\"";
		}
		public override bool Equals(Token other)
		{
			return other is StringLiteral && this.Value == (other as StringLiteral).Value;
		}
		public static async Tasks.Task<Token> Parse(IO.ITextReader reader)
		{
			Token result = null;
			if (await reader.Peek() == '"')
			{
				var mark = reader.Mark();
				await reader.Read();
				var skipNext = false;
				var value = await reader.ReadUpTo(c =>
				{
					var r = skipNext || c == '"';
					skipNext = c == '\\';
					return r;
				}
				);
				char? n;
				if ((n = await reader.Read('"')) != '"')
					new Exception.LexicalError("end of string literal", n.ToString(), await mark.ToFragment()).Throw();
				else
					result = new StringLiteral(value.Unescape(), await mark.ToFragment());
			}
			return result;
		}
	}
}
