// Copyright (C) 2017  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using Xunit;
using Generic = System.Collections.Generic;
using Kean.Extension;

namespace Kean.Tests.Extension
{
		public class EnumerableExtension
		{
			public static Generic.IEnumerable<object[]> SameOrEqualsData
			{
				get
				{
				var data = new string[] { "42", null, "1337", "There are 10 types of people, the ones that know binary and the ones don't.", "" };
				var copy = new string[] { "42", null, "1337", "There are 10 types of people, the ones that know binary and the ones don't.", "" };
				yield return new object[] { data, data };
				yield return new object[] { data, copy };
			}
			}
			[Theory]
			[MemberData("SameOrEqualsData")]
			public void Equal(Generic.IEnumerable<string> left, Generic.IEnumerable<string> right)
			{
				Assert.Equal(left, right);
			}
			[Theory]
			[MemberData("SameOrEqualsData")]
			public void SameOrEquals(Generic.IEnumerable<string> left, Generic.IEnumerable<string> right)
			{
				Assert.True(left.SameOrEquals(right));
			}
		}
}