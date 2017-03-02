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

using System;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.Extension;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.SyntaxTree.Type
{
	public abstract class Expression :
		Node,
		IEquatable<Expression>
	{
		public abstract int Precedence { get; }
		protected Expression(Generic.IEnumerable<Tokens.Token> source) :
			base(source)
		{ }
		public abstract bool Equals(Expression other);
		// override object.Equals
		public override bool Equals(object other)
		{
			return other is Expression && this.Equals((Expression)other);
		}
		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
		public static bool operator ==(Expression left, Expression right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Expression left, Expression right)
		{
			return !(left == right);
		}
		public async Tasks.Task<bool> Write(IO.ITextIndenter indenter, int parentPrecedance) {
			return this.Precedence < parentPrecedance ?
				await indenter.Write("(") && await this.Write(indenter) && await indenter.Write(")") :
				await this.Write(indenter);
		}
		#region Static Parse
		public static Expression Parse(string data)
		{
			return Expression.Parse(Tokens.Lexer.Tokenize(data)).WaitFor();
		}
		internal static async Tasks.Task<Expression> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			return (await Function.Parse((await Tuple.Parse(tokens)) ?? (await Identifier.Parse(tokens)), tokens)) ?? new Exception.SyntaxError("type expression", tokens).Throw<Expression>();
		}
		internal static async Tasks.Task<Expression> TryParse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			return (await tokens.Current).Is<Tokens.Operator>(token => token.Symbol == ":") && tokens.MoveNext() ? await Expression.Parse(tokens) : null;
		}
		#endregion
	}
}
