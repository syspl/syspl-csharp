// Copyright (C) 2016  Simon Mika <simon@mika.se>
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
using Tasks = System.Threading.Tasks;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.SyntaxTree
{
	public class InfixOperator :
		Operator
	{
		public override int Precedence { get; }
		public Associativity Associativity { get; }
		public Expression Left { get; }
		public Expression Right { get; }
		protected InfixOperator(Expression left, string symbol, Expression right, Type.Expression type, Generic.IEnumerable<Tokens.Token> source) :
			base(symbol, type, source)
		{
			this.Left = left;
			this.Right = right;
			switch (symbol)
			{
				default:
					break;
			// Resolving
				case ".":
				case ".?":
					this.Precedence = 300;
					this.Associativity = Associativity.None;
					break;
			// Bitwise Shifting
				case "<<":
				case ">>":
					this.Precedence = 160;
					this.Associativity = Associativity.None;
					break;
			// Multiplicative
				case "*":
				case "/":
				case "%":
				case "&*":
				case "&":
					this.Precedence = 150;
					this.Associativity = Associativity.Left;
					break;
			// Additive
				case "+":
				case "-":
				case "&+":
				case "&-":
				case "|":
				case "^":
					this.Precedence = 140;
					this.Associativity = Associativity.Left;
					break;
			// Range Formation
				case "..<":
				case "...":
					this.Precedence = 135;
					this.Associativity = Associativity.None;
					break;
			// Casting TODO: Letters in operator? Does currently not work!
				case "is":
				case "as":
				case "as?":
				case "as!":
					this.Precedence = 132;
					this.Associativity = Associativity.Left;
					break;
			// Null coalescing
				case "??":
					this.Precedence = 132; // TODO: Correct precedence for ?? operator?
					this.Associativity = Associativity.Right;
					break;
			// Comparative
				case "<":
				case "<=":
				case ">":
				case ">=":
				case "==":
				case "!=":
				case "===":
				case "!==":
				case "~=":
					this.Precedence = 130;
					this.Associativity = Associativity.None;
					break;
			// Conjuctive
				case "&&":
					this.Precedence = 120;
					this.Associativity = Associativity.Left;
					break;
			// Disjunctive
				case "||":
					this.Precedence = 110;
					this.Associativity = Associativity.Left;
					break;
			// Assigning
				case "=":
				case "*=":
				case "/=":
				case "+=":
				case "-=":
				case "<<=":
				case ">>=":
				case "&=":
				case "^=":
				case "|=":
				case "&&=":
				case "||=":
					this.Precedence = 90;
					this.Associativity = Associativity.Right;
					break;
			}
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await this.Left.Write(indenter, this.Precedence + this.Associativity == Associativity.Right ? -1 : 0) &&
				(this.Precedence < 300 || await indenter.Write(" ")) &&
				await indenter.Write(this.Symbol) &&
				(this.Precedence < 300 || await indenter.Write(" ")) &&
				await this.Right.Write(indenter, this.Precedence + this.Associativity == Associativity.Left ? -1 : 0);
		}
	}
}
