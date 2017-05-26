// Copyright (C) 2017  Simon Mika <simon@mika.se>
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

using Xunit;
using Generic = System.Collections.Generic;
using Kean.Extension;

namespace SysPL.Tokens
{
	public class LexerTest
	{
		public static Generic.IEnumerable<object[]> Data {
			get {
				yield return new object[] { new Token[] { new BooleanLiteral(true) }, "true" };
				yield return new object[] { new Token[] { new BooleanLiteral(false) }, "false" };
				yield return new object[] { new Token[] { new Comma() }, "," };
				yield return new object[] { new Token[] { new Comment("/* A wonderfully long comment */") }, "/* A wonderfully long comment */" };
				yield return new object[] { new Token[] { new Comment("// A one line comment\n") }, "// A one line comment\n" };
				yield return new object[] { new Token[] { new FloatingPointLiteral(13.37) }, "13.37" };
				yield return new object[] { new Token[] { new Identifier("Identifier") }, "Identifier" };
				yield return new object[] { new Token[] { new IntegerLiteral(42) }, "42" };
				yield return new object[] { new Token[] { new Keyword(Keywords.Let) }, "let" };
				yield return new object[] { new Token[] { new LeftBrace() }, "{" };
				yield return new object[] { new Token[] { new LeftBracket() }, "[" };
				yield return new object[] { new Token[] { new LeftParenthesis() }, "(" };
				yield return new object[] { new Token[] { new NullLiteral() }, "null" };
				yield return new object[] { new Token[] { new Operator("++") }, "++" };
				yield return new object[] { new Token[] { new Operator("*") }, "*" };
				yield return new object[] { new Token[] { new Operator("--") }, "--" };
				yield return new object[] { new Token[] { new RightBrace() }, "}" };
				yield return new object[] { new Token[] { new RightBracket() }, "]" };
				yield return new object[] { new Token[] { new RightParenthesis() }, ")" };
				yield return new object[] { new Token[] { new Semicolon() }, ";" };
				yield return new object[] { new Token[] { new StringLiteral("This is a string literal.") }, "\"This is a string literal.\"" };
				yield return new object[] { new Token[] { new WhiteSpace("   \n \t") }, "   \n \t" };
				yield return new object[] { new Token[] {
					new Keyword(Keywords.Var),
					new WhiteSpace(" "),
					new Identifier("beta")
				}, "var beta"};
				yield return new object[] { new Token[] {
					new FloatingPointLiteral(13.37),
					new WhiteSpace(" "),
					new Operator("+"),
					new WhiteSpace(" "),
					new IntegerLiteral(42)
				}, "13.37 + 42"};
				yield return new object[] { new Token[] {
					new Keyword(Keywords.Let),
					new WhiteSpace(" "),
					new Identifier("alpha"),
					new WhiteSpace(" "),
					new Operator("="),
					new WhiteSpace(" "),
					new FloatingPointLiteral(13.37),
					new WhiteSpace(" "),
					new Operator("+"),
					new WhiteSpace(" "),
					new IntegerLiteral(42)
				}, "let alpha = 13.37 + 42" };
			}
		}
		[Theory, MemberData("Data")]
		public void Tokenize(Token[] expected, string actual)
		{
			var result = Lexer.Tokenize(actual).ToArray().WaitFor();
			Assert.Equal(expected.Length, result.Length);
			for (int i = 0; i < expected.Length; i++)
				Assert.Equal(expected[i], result[i]);
		}
	}
}
