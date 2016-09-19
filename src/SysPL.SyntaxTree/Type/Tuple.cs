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

namespace SysPL.SyntaxTree.Type
{
	public class Tuple : Expression
	{
		readonly Generic.IEnumerable<Expression> elements;
		public Generic.IEnumerable<Expression> Elements { get { return this.elements; } }
		public Tuple(params Expression[] elements) : this((Generic.IEnumerable<Expression>)elements) { }
		public Tuple(Generic.IEnumerable<Expression> elements)
		{
			this.elements = elements;
		}
	}
}
