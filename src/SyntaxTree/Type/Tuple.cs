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
	public class Tuple :
		Expression
	{
		public override int Precedence { get { return 30; } }
		public Generic.IEnumerable<Expression> Elements { get; }
		public Tuple(params Expression[] elements) :
			this((Generic.IEnumerable<Expression>)elements) { }
		public Tuple(Generic.IEnumerable<Expression> elements, Generic.IEnumerable<Tokens.Token> source = null) :
			base(source)
		{
			this.Elements = elements;
		}
		public override bool Equals(Expression other)
		{
			return other is Tuple && this.Elements.SameOrEquals(((Tuple)other).Elements);
		}
		public override int GetHashCode()
		{
			return this.Hash(this.Elements);
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write("(") &&
			await indenter.Join(this.Elements.Map(element => (Func<IO.ITextWriter, Tasks.Task<bool>>)(writer => element.Write(writer as IO.ITextIndenter, this.Precedence))), ", ") &&
			await indenter.Write(")");
		}
		#region Static Parse
		internal static new async Tasks.Task<Expression> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			Expression result = null;
			if (!(await tokens.Next() is Tokens.RightParenthesis))
			{
				Generic.IEnumerable<Expression> elements = null;
				Generic.IEnumerable<Tokens.Token> source = new[] { await tokens.Current };
				do
				{
					elements = elements.Append(await Expression.Parse(tokens));
					source = source.Append(await tokens.Current);
				}
				while (await tokens.Current is Tokens.Comma && tokens.MoveNext());
				if (!(await tokens.Current is Tokens.RightParenthesis))
					new Exception.SyntaxError("right parenthesis \")\"", tokens).Throw();
				source = source.Append(await tokens.Current);
				result = new Tuple(elements, source);
				tokens.MoveNext();
			}
			return result;
		}
		#endregion
	}
}
