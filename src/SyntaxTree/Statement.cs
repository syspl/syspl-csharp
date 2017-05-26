// Copyright (C) 2014, 2016, 2017  Simon Mika <simon@mika.se>
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
using Kean;
using Kean.Extension;

namespace SysPL.SyntaxTree
{
	public abstract class Statement :
		Node
	{
		public Statement(Generic.IEnumerable<Tokens.Token> source) :
			base(source)
		{
		}
		#region Static Parse
		internal static IAsyncEnumerator<Statement> Parse(IAsyncEnumerator<Tokens.Token> tokens)
		{
			return AsyncEnumerator.Create(() => Statement.ParseNext(tokens), tokens as IDisposable);
		}
		static async Tasks.Task<Statement> ParseNext(IAsyncEnumerator<Tokens.Token> tokens)
		{
			return
				(Statement)await VariableDeclaration.Parse(tokens) ??
//				await FunctionDeclaration.Parse(tokens) ??
//				await StructureDeclaration.Parse(tokens) ??
//				await ClassDeclaration.Parse(tokens) ??
				await Expression.Parse(tokens);
		}
		#endregion
	}
}
