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
using Kean.Extension;
using IO = Kean.IO;
using System;
using System.Collections;

namespace SysPL.Tokens
{
	public class Lexer :
		Generic.IEnumerator<Tasks.Task<Token>>
	{
		Tokenizer tokenizer;
		public Tasks.Task<Token> Current { get; private set; }
		object IEnumerator.Current => this.Current;
		Lexer(Tokenizer tokenizer)
		{
			this.tokenizer = tokenizer;
		}
		public bool MoveNext()
		{
			return (this.Current = this.tokenizer.Next()).NotNull();
		}
		public void Reset()
		{
			throw new NotImplementedException();
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
		static Generic.IEnumerator<Tasks.Task<Token>> CreateEnumerator(Tokenizer tokenizer)
		{
			return tokenizer.NotNull() ? new Lexer(tokenizer) : null;
		}
		public static Generic.IEnumerator<Tasks.Task<Token>> Tokenize(string content)
		{
			return Lexer.Open(IO.TextReader.From(content));
		}
		public static Generic.IEnumerator<Tasks.Task<Token>> Open(IO.ITextReader reader)
		{
			return Lexer.Open(Tokenizer.Open(reader));
		}
		static Generic.IEnumerator<Tasks.Task<Token>> Open(Tokenizer tokenizer)
		{
			return Lexer.CreateEnumerator(tokenizer); //.FilterTasks(token => !(token is WhiteSpace || token is Comment));
		}
		#endregion
	}
}
