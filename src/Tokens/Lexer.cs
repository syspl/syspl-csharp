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

namespace SysPL.Tokens
{
	public class Lexer
	{
		Token current;
		public Token Current
		{
			get { return this.current; }
			private set
			{
				if (this.current.NotNull())
					this.Last = this.current;
				this.current = value;
			}
		}
		public Token Last { get; private set; }
		Generic.IEnumerator<Tasks.Task<Token>> backend;
		public bool Empty { get { return this.Current.IsNull(); } }
		public async Tasks.Task<Token> Next()
		{
			Token result = this.Current = (this.backend.MoveNext() ? await this.backend.Current : null);
			if (result is WhiteSpace || result is Comment)
				result = await this.Next();
			return result;
		}
		Lexer(Tokenizer tokenizer)
		{
			this.backend = this.CreateEnumerator(tokenizer);
		}
		Generic.IEnumerator<Tasks.Task<Token>> CreateEnumerator(Tokenizer tokenizer)
		{
				while (!tokenizer.Empty)
					yield return tokenizer.Next();
		}
		#region Static Open
		public static Lexer Open(IO.ITextReader reader)
		{
			return Lexer.Open(Tokenizer.Open(reader));
		}
		static Lexer Open(Tokenizer tokenizer)
		{
			return tokenizer.NotNull() ? new Lexer(tokenizer) : null;
		}
		#endregion
	}
}
