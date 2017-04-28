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
	public abstract class Separator :
		Token
	{
		protected Separator(Text.Fragment source) :
			base(source)
		{
		}
		public static bool IsSeparator(char c)
		{
			return c == '(' || c == '[' || c == '{' || c == ')' || c == ']' || c == '}' || c == ',' || c == ';';
		}
		public static bool IsSeparator(char? c)
		{
			return c.HasValue && Separator.IsSeparator(c.Value);
		}
		public static async Tasks.Task<Token> Parse(IO.ITextReader reader)
		{
			Token result = null;
			if (Separator.IsSeparator(await reader.Peek()))
			{
				var mark = reader.Mark();
				result = Separator.Parse((await reader.Read()).Value, await (Tasks.Task<Text.Fragment>)mark);
			}
			return result;
		}
		public static Separator Parse(char data, Text.Fragment source)
		{
			Separator result = null;
			switch (data)
			{
				case '(':
					result = new LeftParenthesis(source);
					break;
				case ')':
					result = new RightParenthesis(source);
					break;
				case '[':
					result = new LeftBracket(source);
					break;
				case ']':
					result = new RightBracket(source);
					break;
				case '{':
					result = new LeftBrace(source);
					break;
				case '}':
					result = new RightBrace(source);
					break;
				case ',':
					result = new Comma(source);
					break;
				case ';':
					result = new Semicolon(source);
					break;
			}
			return result;
		}
	}
}
