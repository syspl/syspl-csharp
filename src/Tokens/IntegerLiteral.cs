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

using Text = Kean.Text;

namespace SysPL.Tokens
{
	public class IntegerLiteral :
		NumberLiteral
	{
		public readonly long Value;
		public IntegerLiteral(long value, Text.Fragment source = null) :
			base(source)
		{
			this.Value = value;
		}
		public override string ToString()
		{
			return this.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
		}
		public override bool Equals(Token other)
		{
			return other is IntegerLiteral && this.Value == (other as IntegerLiteral).Value;
		}
		public static IntegerLiteral Parse(string raw, Text.Fragment source)
		{
			IntegerLiteral result = null;
			long v;
			if (long.TryParse(raw, out v))
				result = new IntegerLiteral(v, source);
			return result;
		}

	}
}
