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

using Generic = System.Collections.Generic;

namespace SysPL.Tokens
{
	public class TestData
	{
		public static Generic.IEnumerable<Tokens.Token> Tokens {
			get {
				yield return new BooleanLiteral(true);
				yield return new BooleanLiteral(false);
				yield return new Comma();
				yield return new Comment(" A wonderfully long comment ");
				yield return new FloatingPointLiteral(13.37);
				yield return new Identifier("Identifier");
				yield return new IntegerLiteral(42);
				yield return new Keyword(Keywords.Let);
				yield return new LeftBrace();
				yield return new LeftBracket();
				yield return new LeftParenthesis();
				yield return new NullLiteral();
				yield return new Operator("++", OperatorType.Prefix);
				yield return new Operator("*", OperatorType.Infix);
				yield return new Operator("--", OperatorType.Postfix);
				yield return new RightBrace();
				yield return new RightBracket();
				yield return new RightParenthesis();
				yield return new Semicolon();
				yield return new StringLiteral("A string.");
				yield return new WhiteSpace("   \n \t");
			}
		}
		public static Generic.IEnumerable<string> Strings {
			get {
				yield return "true";
				yield return "false";
				yield return ",";
				yield return "/* A wonderfully long comment */";
				yield return "13.37";
				yield return "Identifier";
				yield return "42";
				yield return "let";
				yield return "{";
				yield return "[";
				yield return "(";
				yield return "null";
				yield return "++";
				yield return "*";
				yield return "--";
				yield return "}";
				yield return "]";
				yield return ")";
				yield return ";";
				yield return "\"A string.\"";
				yield return "   \n \t";
			}
		}
	}
}
