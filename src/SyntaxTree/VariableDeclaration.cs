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

namespace SysPL.SyntaxTree
{
	public class VariableDeclaration :
		SymbolDeclaration
	{
		public VariableDeclaration(Symbol.Expression symbol) : this(symbol, false, null) { }
		protected VariableDeclaration(Symbol.Expression symbol, bool immutable, Generic.IEnumerable<Tokens.Token> source) :
			base(symbol, immutable, source)
		{
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write("var ") && await this.Symbol.Write(indenter) && await indenter.WriteLine();
		}
		#region Static Parse
		internal static new async Tasks.Task<VariableDeclaration> Parse(Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens)
		{
			var current = await tokens.Current as Tokens.Keyword;
			VariableDeclaration result = null;
			bool immutable;
			if (current.NotNull() && ((immutable = current.Name == Tokens.Keywords.Let) || current.Name == Tokens.Keywords.Var))
			{
				Generic.IEnumerable<Tokens.Token> source = new[] { current };
				if (!tokens.MoveNext())
					new Exception.SyntaxError("symbol expression", tokens).Throw();
				var symbol = await SyntaxTree.Symbol.Expression.Parse(tokens);
				if ((await tokens.Current).Is<Tokens.InfixOperator>(t => t.Symbol == "="))
				{
					source = source.Append(await tokens.Current);
					if (!tokens.MoveNext())
						new Exception.SyntaxError("expression", tokens).Throw();
					result = new VariableDefinition(symbol, await Expression.Parse(tokens), immutable, source);
				}
				else
					result = new VariableDeclaration(symbol, immutable, source);
			}
			return result;
		}
		#endregion
	}
}
