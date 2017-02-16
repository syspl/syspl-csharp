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

using System;
using Tasks = System.Threading.Tasks;
using Kean.Extension;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.Tokens
{
	class Tokenizer :
		IDisposable
	{
		IO.ITextReader reader;
		Token last;
		public bool Empty { get; private set; }
		Tokenizer(IO.ITextReader reader)
		{
			this.reader = reader;
		}
		public async Tasks.Task<Token> Next()
		{
			if (this.last.IsNull())
				await this.reader.Next(); // only done first time
			Token result = null;
			this.Empty = await this.reader.Empty;
			var mark = this.reader.Mark();
			if (this.IsWhiteSpace(this.reader.Last)) // White Space
				result = new WhiteSpace(await this.reader.ReadFromCurrentUntil(c => !this.IsWhiteSpace(c)), mark);
			else if (this.IsSeparator(this.reader.Last)) // White Space
			{
				result = Separator.Parse(this.reader.Last, mark);
				this.Empty = !(await this.reader.Next());
			}
			else if (this.IsOperator(this.reader.Last))
			{
				string r = await this.reader.ReadFromCurrentUntil(c => !this.IsOperator(c));
				if (r == "//")
				{
					result = new Comment(r + this.reader.ReadFromCurrentUntil(c => (c == '\n' || c == '\r')), mark);
					this.Empty = !(await this.reader.Next());
				}
				else if (r == "/*")
				{
					int depth = 0;
					char previous = '\0';
					result = new Comment(r + this.reader.ReadFromCurrentUntil(c =>
					{
						if (previous == '/' && c == '*')
							depth++;
						return previous == '*' && (previous = c) == '/' && depth-- == 0;
					}) + "/", mark);
					this.Empty = !(await this.reader.Next());
				}
				else
					result =
						this.last is WhiteSpace && !(this.IsSeparator(this.reader.Last) || this.IsWhiteSpace(this.reader.Last)) ? (Operator)new PrefixOperator(r, mark) :
						!(this.last is WhiteSpace) && (this.IsSeparator(this.reader.Last) || this.IsWhiteSpace(this.reader.Last) || this.reader.Last == '.') ? (Operator)new PostfixOperator(r, mark) :
						new InfixOperator(r, mark);
			}
			else if (this.StartsNumber(this.reader.Last))
			{
				bool floatingPoint = false;
				string r = await this.reader.ReadFromCurrentUntil(c => !((floatingPoint = c == '.') || this.IsWithinNumber(c)));
				result = floatingPoint ? (Literal)FloatingPointLiteral.Parse(r, mark) : IntegerLiteral.Parse(r, mark);
			}
			else if (this.StartsIdentifier(this.reader.Last)) // Keyword, Identifier, Boolean Literal or Null Literal
				result = Identifier.Parse(await this.reader.ReadFromCurrentUntil(c => !this.IsWithinIdentifier(c)), mark);
			else
				new Exception.LexicalError("a valid token", "invalid character (\"" + this.reader.Last + "\" " + ((int)this.reader.Last).ToString("x") + ")", mark).Throw();
			if (result.NotNull())
				this.last = result;
			return result;
		}
		bool IsWhiteSpace(char c)
		{
			return char.IsWhiteSpace(c);
		}
		bool IsSeparator(char c)
		{
			return c == '(' || c == '[' || c == '{' || c == ')' || c == ']' || c == '}' || c == ',' || c == ';';
		}
		bool IsOperator(char c)
		{
			return c == '/' || c == '=' || c == '-' || c == '+' || c == '!' || c == '*' || c == '%' || c == '<' || c == '>' || c == '&' || c == '|' || c == '^' || c == '~' || c == '.' || c == '?' || c == ':';
		}
		bool StartsNumber(char c)
		{
			return char.IsDigit(c);
		}
		bool IsWithinNumber(char c)
		{
			return char.IsLetterOrDigit(c) || c == '_' || c == '.';
		}
		bool StartsIdentifier(char c)
		{
			return char.IsLetter(c) || c == '_';
		}
		bool IsWithinIdentifier(char c)
		{
			return char.IsLetterOrDigit(c) || c == '_';
		}
		public async Tasks.Task<bool> Close()
		{
			bool result = this.reader.NotNull();
			if (result && (result = await this.reader.Close()))
				this.reader = null;
			return result;
		}
		#region IDisposable implementation
		~Tokenizer ()
		{
			this.Close().Wait();
		}
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		#endregion
		public static Tokenizer Open(IO.ITextReader reader)
		{
			return reader.NotNull() ? new Tokenizer(reader) : null;
		}
	}
}
