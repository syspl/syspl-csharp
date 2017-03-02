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
using Text = Kean.Text;
using IO = Kean.IO;
using Kean.IO.Extension;

namespace SysPL.SyntaxTree
{
	public class StringLiteral :
		Literal
	{
		public string Value { get; }
		public string Escaped {
			get { return this.Value.Fold((c, builder) => {
				switch (c)
				{
					case '\0': builder += "\\0"; break;
					case '\\': builder += "\\\\"; break;
					case '\t': builder += "\\t"; break;
					case '\n': builder += "\\n"; break;
					case '\r': builder += "\\r"; break;
					case '"': builder += "\\\""; break;
					case '\'': builder += "\\'"; break;
					default: builder += c; break;
				}
				return builder;
			}, new Text.Builder()); }
		}
		public StringLiteral(string value, string raw, Type.Expression type = null, Generic.IEnumerable<Tokens.Token> source = null) :
			base(raw, type, source)
		{
			this.Value = value;
		}
		public override async Tasks.Task<bool> Write(IO.ITextIndenter indenter)
		{
			return await indenter.Write("\"") &&
				await indenter.Write(this.Escaped) &&
				await indenter.Write("\"");
		}
	}
}
