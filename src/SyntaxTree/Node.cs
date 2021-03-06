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
using IO = Kean.IO;

namespace SysPL.SyntaxTree
{
	public abstract class Node
	{
		Generic.IEnumerable<Tokens.Token> Source { get; }
		protected Node(Generic.IEnumerable<Tokens.Token> source)
		{
			this.Source = source;
		}
		public abstract Tasks.Task<bool> Write(IO.ITextIndenter indenter);
		public override string ToString()
		{
			string result = null;
			using (var indenter = IO.TextIndenter.Open(r => result = r))
				this.Write(indenter).Wait();
			return result;
		}
	}
}
