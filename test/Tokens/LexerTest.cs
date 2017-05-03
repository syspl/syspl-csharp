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
				yield return new object[] { new Token[] { new Identifier("alfa") }, "alfa" };
				yield return new object[] { new Token[] { new Keyword(Keywords.Let) }, "let" };
				yield return new object[] { new Token[] {
					new Keyword(Keywords.Var),
					new Identifier("beta")
				}, "var beta"};
				/*yield return new object[] { new Token[] {
					new Keyword(Keywords.Let),
					new Identifier("alfa"),
					new InfixOperator("="),
					new FloatingPointLiteral(13.37),
					new InfixOperator("+"),
					new IntegerLiteral(42)
				}, "let alfa = 13.37 + 42" };*/
			}
		}
		[Theory, MemberData("Data")]
		public void Tokenize(Generic.IEnumerable<Token> expected, string actual)
		{
			var a = Lexer.Tokenize(actual);
			var b = a.ToArray();
			var result = b.WaitFor();
			//var result = Lexer.Tokenize(actual).ToArray().WhenAll().WaitFor().ToArray();
			//Assert.Equal(expected, (Generic.IEnumerable<Token>)result);
		}
	}
}
