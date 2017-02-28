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
using Kean.Extension;

namespace SysPL.SyntaxTree.Symbol
{
	public class Discard :
		Expression
	{
		public Discard(Type.Expression type = null) :
			this(null, type)
		{}
		public Discard(Tokens.Identifier source, Type.Expression type = null) :
			base(type, new [] { source })
		{}
		internal static async new Tasks.Task<Discard> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			var current = await tokens.Current as Tokens.Identifier;
			return current.NotNull() && current.Name == "_" ? new Discard(current, tokens.MoveNext() ? await Type.Expression.TryParse(tokens) : null) : null;
		}
	}
}
