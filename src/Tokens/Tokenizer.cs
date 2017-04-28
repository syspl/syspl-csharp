// Copyright (C) 2014, 2017  Simon Mika <simon@mika.se>
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
using Tasks = System.Threading.Tasks;
using Kean.Extension;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.Tokens
{
	class Tokenizer :
		IDisposable
	{
		IO.ITextReader reader;
		public Tasks.Task<bool> Empty { get { return this.reader.Empty; } }
		Tokenizer(IO.ITextReader reader)
		{
			this.reader = reader;
		}
		public async Tasks.Task<Token> Next()
		{
			var mark = this.reader.Mark();
			return
				await WhiteSpace.Parse(reader) ??
				await Separator.Parse(reader) ??
				await Operator.Parse(reader) ??
				await NumberLiteral.Parse(reader) ??
				await Identifier.Parse(reader) ??
				new Exception.LexicalError("a valid token", "invalid character (\"" + await this.reader.Peek() + "\" " + ((int)await this.reader.Read()).ToString("x") + ")", await mark.ToFragment()).Throw<Token>();
		}
		public async Tasks.Task<bool> Close()
		{
			bool result = this.reader.NotNull();
			if (result && (result = await this.reader.Close()))
				this.reader = null;
			return result;
		}
		#region IDisposable implementation
		~Tokenizer ()
		{
			this.Close().Wait();
		}
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		#endregion
		public static Tokenizer Open(IO.ITextReader reader)
		{
			return reader.NotNull() ? new Tokenizer(reader) : null;
		}
	}
}
