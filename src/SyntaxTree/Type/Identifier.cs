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

namespace SysPL.SyntaxTree.Type
{
	public class Identifier :
		Expression
	{
		public override int Precedence { get { return 50; } }
		public string Name { get; }
		public Identifier(Tokens.Identifier source) :
			this(source.Name, source)
		{ }
		public Identifier(string name, Tokens.Identifier source = null) :
			base(new [] { source })
		{
			this.Name = name;
		}
		public override bool Equals(Expression other)
		{
			return other is Identifier && this.Name == ((Identifier)other).Name;
		}
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write(this.Name);
		}
		internal static new async Tasks.Task<Expression> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			var current = await tokens.Current as Tokens.Identifier;
			tokens.MoveNext();
			return current.NotNull() ? new Identifier(current) : null;
		}
	}
}
