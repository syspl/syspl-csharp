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

namespace SysPL.SyntaxTree
{
	public abstract class Expression :
		Statement
	{
		public abstract int Precedence { get; }
		public Type.Expression AssignedType { get; }
		protected Expression(Type.Expression assignedType, Generic.IEnumerable<Tokens.Token> source) :
			base(source)
		{
			this.AssignedType = assignedType;		}
		internal static new async Tasks.Task<Expression> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			return null;
		}
	}
}
