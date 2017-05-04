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

using Tasks = System.Threading.Tasks;
using IO = Kean.IO;
using Kean.IO.Extension;
using Text = Kean.Text;

namespace SysPL.Tokens
{
	public abstract class NumberLiteral :
		Literal
	{
		protected NumberLiteral(Text.Fragment source) :
			base(source)
		{
		}
		public static bool StartsNumber(char? c)
		{
			return c.HasValue && char.IsDigit(c.Value);
		}
		public static bool IsWithinNumber(char? c)
		{
			return c.HasValue && (char.IsDigit(c.Value) || c == '_' || c == '.');
		}
		public static async Tasks.Task<Token> Parse(IO.ITextReader reader)
		{
			Token result = null;
			if (NumberLiteral.StartsNumber(await reader.Peek()))
			{
				var floatingPoint = false;
				var mark = reader.Mark();
				string r = await reader.ReadUpTo(c => { if (c == '.') floatingPoint = true; return !NumberLiteral.IsWithinNumber(c); });
				result = floatingPoint ? (Literal)FloatingPointLiteral.Parse(r, await mark.ToFragment()) : IntegerLiteral.Parse(r, await mark.ToFragment());
			}
			return result;
		}
	}
}
