// Copyright (C) 2017  Simon Mika <simon@mika.se>
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

using Xunit;
using Generic = System.Collections.Generic;

namespace SysPL.SyntaxTree.Type
{
	public class StringTest
	{
		public static Generic.IEnumerable<object[]> Data {
			get {
				yield return new object[] { "int", new Identifier("int") };
				yield return new object[] { "string", new Identifier("string") };
				yield return new object[] { "string * int", new Tuple(new Identifier("string"), new Identifier("int")) };
			}
		}
		[Theory, MemberData("Data")]
		public void Stringify(string expected, Expression actual)
		{
			Assert.Equal(expected, actual.ToString());
		}
		[Theory, MemberData("Data")]
		public void Parse(string actual, Expression expected)
		{
// TODO: Is this how we do and test parsing?
//			Assert.Equal(expected, Expression.Parse(actual));
		}
	}
}