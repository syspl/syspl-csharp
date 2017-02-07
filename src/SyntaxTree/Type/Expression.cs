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
using Kean.Extension;

namespace SysPL.SyntaxTree.Type
{
	public abstract class Expression :
			IEquatable<Expression>
	{
		public abstract int Precedence { get; }

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
		public string ToString(int parentPrecedance) {
			var result = this.ToString();
			if (this.Precedence < parentPrecedance)
				result = "(" + result + ")";
			return result;
		}
	}
}
