// Copyright (C) 2016  Simon Mika <simon@mika.se>
//
// This file is part of SysPL.
//
// SysPL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
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

namespace SysPL.SyntaxTree
{
	public class PrefixOperator : Operator
	{
		public override int Precedence { get { return 250; } }
		public Expression Expression { get; }
		public PrefixOperator(string symbol, Expression expression, Type.Expression type) : base(symbol, type)
		{
			this.Expression = expression;
		}
		#region Static Create
		public static PrefixOperator Create(string symbol, Expression expression, Type.Expression type = null)
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
					result = new PrefixOperator(symbol, expression, type);
					break;
			}
			return result;
		}
		#endregion
	}
}
