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
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.SyntaxTree.Symbol
{
	public class Identifier :
		Expression
	{
		public string Name { get; }
		public Identifier(string name, Type.Expression type = null, Generic.IEnumerable<Tokens.Token> source = null) :
			base(type, source)
		{
			this.Name = name;
		}
		Identifier(Tokens.Identifier source, Type.Expression type = null) :
			this(source.Name, type, new [] { source })
		{}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write(this.Name);
		}
		internal static async new Tasks.Task<Identifier> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			var current = await tokens.Current as Tokens.Identifier;
			return current.NotNull() ? new Identifier(current, tokens.MoveNext() ? await Type.Expression.TryParse(tokens) : null) : null;
		}
	}
}
