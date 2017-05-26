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
using Kean.Extension;

namespace SysPL.Tokens
{
	public class ArrayEqualTest
	{
		public static Generic.IEnumerable<object[]> ArrayEqualData
		{
			get
			{
				yield return new object[] { TestData.Tokens.ToArray(), TestData.Tokens.ToArray() };
			}
		}
		[Theory, MemberData("ArrayEqualData")]
		public void SameOrEquals(Tokens.Token[] expected, Tokens.Token[] actual)
		{
			Assert.Equal(expected.Length, actual.Length);
			for (int i = 0; i < expected.Length; i++)
				Assert.Equal(expected[i], actual[i]);
		}
		// TODO: Why does this test crash the test runner?
//		[Theory, MemberData("ArrayEqualData")]
//		public void Equal(Tokens.Token[] expected, Tokens.Token[] actual)
//		{
//			Assert.Equal(expected, actual);
//		}
	}
}
