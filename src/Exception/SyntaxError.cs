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
using Error = Kean.Error;
using Text = Kean.Text;

namespace SysPL.Exception
{
	public class SyntaxError :
		Exception
	{
		public string Expected { get; }
		public string Found { get; }
		public new Text.Fragment Source { get; }
		internal SyntaxError(string expected, Generic.IEnumerator<Tasks.Task<Tokens.Token>> tokens) :
			this(expected, "\"" + tokens.Current?.WaitFor()?.Source ?? "nothing" + "\"", tokens.Current?.WaitFor()?.Source)
		{
		}
		internal SyntaxError(string expected, string found, Text.Fragment source) :
			this(null, expected, found, source)
		{
		}
		internal SyntaxError(System.Exception innerException, string expected, string found, Text.Fragment source) :
			base(innerException, Error.Level.Critical, "Syntax Error", "Expected {0}, but found {1}, at {2}.", expected, found, source.ToString())
		{
			this.Expected = expected;
			this.Found = found;
			this.Source = source;
		}
	}
}
