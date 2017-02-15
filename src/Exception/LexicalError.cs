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

using Error = Kean.Error;
using Text = Kean.Text;

namespace SysPL.Exception
{
	public class LexicalError : Exception
	{
		public string Expected { get; }
		public string Found { get; }
		public new Text.Fragment Source { get; }
		protected internal LexicalError(string expected, string found, Text.Fragment source) :
			this(null, expected, found, source)
		{
		}
		protected internal LexicalError(System.Exception innerException, string expected, string found, Text.Fragment region) :
			base(innerException, Error.Level.Critical, "Lexical Error", "Expected {0}, but found {1}, at {2}.", expected, found, region.ToString())
		{
			this.Expected = expected;
			this.Found = found;
			this.Source = region;
		}
	}
}
