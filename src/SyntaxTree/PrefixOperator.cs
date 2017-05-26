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
	public class PrefixOperator : Operator
	{
		public override int Precedence { get { return 250; } }
		public Expression Expression { get; }
		public PrefixOperator(string symbol, Expression expression, Type.Expression type, Generic.IEnumerable<Tokens.Token> source) :
			base(symbol, type, source)
		{
			this.Expression = expression;
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write(this.Symbol) &&
				await this.Expression.Write(indenter, this.Precedence);
		}
		#region Static Create
		public static PrefixOperator Create(string symbol, Expression expression, Type.Expression type = null, Generic.IEnumerable<Tokens.Token> source = null)
		{
			PrefixOperator result;
			switch (symbol)
			{
				default:
					result = null;
					break;
				case "++":
				case "--":
				case "!":
				case "~":
				case "+":
				case "-":
					result = new PrefixOperator(symbol, expression, type, source);
					break;
			}
			return result;
		}
		#endregion
	}
}
