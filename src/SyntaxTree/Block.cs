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

using System;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.Extension;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.SyntaxTree
{
	public class Block :
		Expression
	{
		public override int Precedence { get { return int.MaxValue; } }
		public Generic.IEnumerable<Statement> Statements { get; }
		public Block(params Statement[] statements) :
			this((Generic.IEnumerable<Statement>)statements)
		{
		}
		public Block(Generic.IEnumerable<Statement> statements, Generic.IEnumerable<Tokens.Token> source = null) :
			base(Block.GetType(statements.Last()), source)
		{
			this.Statements = statements;
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.WriteLine("{") && indenter.Increase() &&
				await indenter.Join(this.Statements.Map(statement => (Func<IO.ITextWriter, Tasks.Task<bool>>)(writer => statement.Write((IO.ITextIndenter)writer))), (Func<IO.ITextWriter, Tasks.Task<bool>>)(writer => writer.WriteLine())) &&
				indenter.Decrease() && await indenter.WriteLine("}");

		}
		static Type.Expression GetType(Statement statement)
		{
			return statement is Expression ? (statement as Expression).AssignedType : new Type.Identifier("void");
		}
/*		internal static async Generic.IEnumerable<Statement> ParseCodeBlock(IAsyncEnumerator<Tokens.Token> tokens)
		{
			var current = await tokens.Current;
			if (!(current is Tokens.LeftBrace))
				new Exception.SyntaxError("function body starting with \"{\"", tokens).Throw();
			await tokens.Next();
			foreach (var result in Statement.ParseStatements(tokens))
				yield return result;
			if (!(await tokens.Current is Tokens.RightBrace))
				new Exception.SyntaxError("function body ending with \"}\"", tokens).Throw();
			await tokens.Next();
		}
*/	}
}
