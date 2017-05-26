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
	public class EqualTest
	{
		public static Generic.IEnumerable<object[]> EqualData {
			get {
				return TestData.Tokens.Zip(TestData.Tokens, (left, right) => new object[] { left, right });
			}
		}
		public static Generic.IEnumerable<object[]> NotEqualData {
			get {
				var i = 0;
				foreach (var t in TestData.Tokens)
				{
					var j = 0;
					foreach (var s in TestData.Tokens)
					{
						if (i != j)
							yield return new object[] { t, s };
						j++;
					}
					i++;
				}
			}
		}
		[Theory, MemberData("EqualData")]
		public void Equal(Tokens.Token expected, Tokens.Token actual)
		{
			Assert.Equal(expected, actual);
		}
		[Theory, MemberData("NotEqualData")]
		public void NotEqual(Tokens.Token expected, Tokens.Token actual)
		{
			Assert.NotEqual(expected, actual);
		}
	}
}
