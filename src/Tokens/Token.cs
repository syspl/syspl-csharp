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
using Kean.Extension;
using Text = Kean.Text;

namespace SysPL.Tokens
{
	public abstract class Token :
		IEquatable<Token>
	{
		public Text.Fragment Source { get; }
		protected Token(Text.Fragment source)
		{
			this.Source = source;
		}
		public override sealed int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
		public override sealed bool Equals(object other)
		{
			return other is Token && this.Equals(other);
		}
		public abstract bool Equals(Token token);
		public static bool operator ==(Token left, Token right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Token left, Token right)
		{
			return !(left == right);
		}
	}
}
