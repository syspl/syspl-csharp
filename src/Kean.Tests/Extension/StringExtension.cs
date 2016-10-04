// Copyright (C) 2010  Simon Mika <simon@mika.se>
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
using Kean.Extension;

namespace Kean.Test
{
	public class StringExtension
	{
		[Fact]
		public void PercentEncode()
		{
			Assert.Equal("string with spaces".PercentEncode(), "string%20with%20spaces");
			Assert.Equal("string\"with\"".PercentEncode(), "string%22with%22");
		}
		[Fact]
		public void PercentDecode()
		{
			Assert.Equal("string%20with%20spaces".PercentDecode(), "string with spaces");
			Assert.Equal("string%22with%22".PercentDecode(), "string\"with\"");
		}
	}
}
