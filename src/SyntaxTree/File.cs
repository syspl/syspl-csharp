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

namespace SysPL.SyntaxTree
{
	public class File :
		Node
	{
		public Generic.IEnumerable<Statement> Statements { get; }
		public File(Generic.IEnumerable<Statement> statements, Generic.IEnumerable<Tokens.Token> source = null) :
			base(source)
		{
			this.Statements = statements;
		}
		public File(params Statement[] statements) :
			this((Generic.IEnumerable<Statement>)statements)
		{ }
		#region Static Parse
		public static async Tasks.Task<File> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			return new File(await Statement.Parse(tokens).WhenAll());
		}
		#endregion
	}
}
