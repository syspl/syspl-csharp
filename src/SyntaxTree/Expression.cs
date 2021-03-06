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
using Kean;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.SyntaxTree
{
	public abstract class Expression :
		Statement
	{
		public abstract int Precedence { get; }
		public Type.Expression AssignedType { get; }
		protected Expression(Type.Expression assignedType, Generic.IEnumerable<Tokens.Token> source) :
			base(source)
		{
			this.AssignedType = assignedType;
		}
		public async Tasks.Task<bool> Write(IO.ITextIndenter indenter, int parentPrecedance) {
			return this.Precedence < parentPrecedance ?
				await indenter.Write("(") && await this.Write(indenter) && await indenter.Write(")") :
				await this.Write(indenter);
		}
		internal static new Tasks.Task<Expression> Parse(IAsyncEnumerator<Tokens.Token> tokens)
		{
			return Tasks.Task.FromResult<Expression>(null);
		}
	}
}
