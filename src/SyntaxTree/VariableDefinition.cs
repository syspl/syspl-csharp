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
using Kean.IO.Extension;

namespace SysPL.SyntaxTree
{
	public class VariableDefinition :
		VariableDeclaration
	{
		public Expression Value { get; }
		public VariableDefinition(Symbol.Expression symbol, Expression value, bool immutable = false, Generic.IEnumerable<Tokens.Token> source = null) :
			base(symbol, immutable, source)
		{
			this.Value = value;
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write(this.Immutable ? "let " : "var ") &&
				await this.Symbol.Write(indenter) &&
				await indenter.Write(" = ") &&
				await this.Value.Write(indenter) &&
				await indenter.WriteLine();
		}
	}
}
