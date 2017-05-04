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
	public class Operator :
		Token
	{
		public string Symbol { get; }
		public OperatorType Type { get; }
		public Operator(string symbol, Text.Fragment source = null) :
			this(symbol, OperatorType.Unknown, source)
		{ }
		public Operator(string symbol, OperatorType type, Text.Fragment source = null) :
			base(source)
		{
			this.Symbol = symbol;
			this.Type = type;
		}
		public override bool Equals(Token other)
		{
			return other is Operator && this.Symbol == (other as Operator).Symbol;
		}
		public override string ToString()
		{
			return this.Symbol;
		}
		public static bool IsOperator(char? c)
		{
			return c == '/' || c == '=' || c == '-' || c == '+' || c == '!' || c == '*' || c == '%' || c == '<' || c == '>' || c == '&' || c == '|' || c == '^' || c == '~' || c == '.' || c == '?' || c == ':';
		}
		public static async Tasks.Task<Token> Parse(IO.ITextReader reader)
		{
			Token result = null;
			if (Operator.IsOperator(await reader.Peek()))
			{
				var mark = reader.Mark();
				string r = await reader.ReadUpTo(c => !Operator.IsOperator(c));
				switch (r)
				{
					case "//":
						result = new Comment(r + await reader.ReadPast(c => c == '\n'), await mark.ToFragment());
						break;
					case "/*":
						int depth = 0;
						char previous = '\0';
						result = new Comment(r + await reader.ReadPast(c =>
						{
							if (previous == '/' && c == '*')
								depth++;
							return previous == '*' && (previous = c) == '/' && depth-- == 0;
						}), await mark.ToFragment());
						break;
					default:
						result = new Operator(r, await mark.ToFragment());
						break;
				}
			}
			return result;
		}
	}
}
