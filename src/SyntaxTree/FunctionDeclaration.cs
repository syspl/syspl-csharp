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
	public class FunctionDeclaration :
		SymbolDeclaration
	{
		public Type.Expression ReturnType { get; }
		public Function Function { get; }
		public FunctionDeclaration(Symbol.Identifier symbol, Type.Expression returnType, Function function, Generic.IEnumerable<Tokens.Token> source = null) :
			base(symbol, true, source)
		{
			this.ReturnType = returnType;
			this.Function = function;
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write("func ") &&
				await this.Symbol.Write(indenter) &&
				await this.Function.Arguments.Write(indenter) &&
				(this.ReturnType.IsNull() || await indenter.Write(" : ") && await this.ReturnType.Write(indenter)) &&
				this.Function.Expression is Block ? await this.Function.Expression.Write(indenter) :
				this.Function.Expression.IsNull() ? await indenter.Write(" { }") :
				await indenter.Write(" { ") && indenter.Increase() && await this.Function.Expression.Write(indenter) && indenter.Decrease() && await indenter.WriteLine(" }");
		}
	}
}
