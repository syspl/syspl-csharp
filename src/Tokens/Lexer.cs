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

using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean;
using Kean.Extension;
using IO = Kean.IO;
using System;
using System.Collections;

namespace SysPL.Tokens
{
	public class Lexer :
		IAsyncEnumerator<Token>,
		IDisposable
	{
		Tokenizer tokenizer;
		public Token Current { get; private set; }
		Lexer(Tokenizer tokenizer)
		{
			this.tokenizer = tokenizer;
		}
		public async Tasks.Task<bool> MoveNext()
		{
			return (this.Current = await this.tokenizer.Next()).NotNull() && !await this.tokenizer.Empty;
		}
		public void Dispose()
		{
			if (this.tokenizer.NotNull())
			{
				this.tokenizer.Close().Forget();
				this.tokenizer = null;
			}
		}
		#region Static Open
		static IAsyncEnumerator<Token> CreateEnumerator(Tokenizer tokenizer)
		{
			return tokenizer.NotNull() ? new Lexer(tokenizer) : null;
		}
		public static IAsyncEnumerator<Token> Tokenize(string content)
		{
			return Lexer.Open(IO.TextReader.From(content));
		}
		public static IAsyncEnumerator<Token> Open(IO.ITextReader reader)
		{
			return Lexer.Open(Tokenizer.Open(reader));
		}
		static IAsyncEnumerator<Token> Open(Tokenizer tokenizer)
		{
			return Lexer.CreateEnumerator(tokenizer); //.FilterTasks(token => !(token is WhiteSpace || token is Comment));
		}
		#endregion
	}
}
