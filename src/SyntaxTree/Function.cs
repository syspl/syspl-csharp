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

namespace SysPL.SyntaxTree {
	public class Function :
		Expression
	{
		public override int Precedence { get { return 500; } }
		public Symbol.Expression Arguments { get; }
		public Expression Expression { get; }
		public Function(Symbol.Expression arguments, Expression expression, Type.Expression type = null, Generic.IEnumerable<Tokens.Token> source = null) :
			base(type, source)
		{
			this.Arguments = arguments;
			this.Expression = expression;
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await this.Arguments.Write(indenter) &&
				await indenter.Write(" => ") &&
				await this.Expression.Write(indenter);
		}
	}
}
