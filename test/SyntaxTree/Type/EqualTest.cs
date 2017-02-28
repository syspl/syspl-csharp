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

using Xunit;
using Generic = System.Collections.Generic;

namespace SysPL.SyntaxTree.Type
{
	public class EqualTest
	{
		public static Generic.IEnumerable<object[]> EqualData {
			get {
				var identifier = new Identifier("int");
				yield return new object[] { identifier, identifier };
				yield return new object[] { new Identifier("int"), new Identifier("int") };
				yield return new object[] { new Identifier("string"), new Identifier("string") };
				yield return new object[] { new Tuple(new Identifier("string"), new Identifier("int")), new Tuple(new Identifier("string"), new Identifier("int")) };
			}
		}
		public static Generic.IEnumerable<object[]> NotEqualData {
			get {
				yield return new object[] { new Identifier("int"), new Identifier("string") };
				yield return new object[] { new Identifier("string"), new Identifier("int") };
				yield return new object[] { new Tuple(new Identifier("string")), new Identifier("string") };
				yield return new object[] { new Tuple(new Identifier("string"), new Identifier("string")), new Identifier("string") };
				yield return new object[] { new Tuple(new Identifier("string"), new Identifier("string")), new Tuple(new Identifier("string"), new Identifier("int")) };
				yield return new object[] { new Tuple(new Identifier("string"), new Identifier("string")), new Tuple(new Identifier("int"), new Identifier("string")) };
			}
		}
		[Theory, MemberData("EqualData")]
		public void Equal(Expression expected, Expression actual)
		{
			Assert.Equal(expected, actual);
		}
		[Theory, MemberData("NotEqualData")]
		public void NotEqual(Expression expected, Expression actual)
		{
			Assert.NotEqual(expected, actual);
		}
	}
}
