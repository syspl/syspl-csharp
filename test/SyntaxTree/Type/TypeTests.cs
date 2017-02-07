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
using Kean.Extension;

namespace SysPL.SyntaxTree.Tests
{
	public class TypeTests
	{
		[Fact]
		public void Identifier()
		{
			var a = new Type.Identifier("string");
			Assert.Equal(a.Name, "string");
			Assert.Equal(a.ToString(), "string");
			var b = new Type.Identifier("int");
			Assert.Equal(b.Name, "int");
			Assert.Equal(b.ToString(), "int");
			Assert.NotEqual(a, b);
			var c = new Type.Identifier("int");
			Assert.Equal(c.ToString(), "int");
			Assert.Equal(b, c);
		}
		[Fact]
		public void Tuple()
		{
			var a = new Type.Tuple(new Type.Identifier("string"), new Type.Identifier("int"));
			Assert.Equal(a.Elements.Get(0), new Type.Identifier("string"));
			Assert.Equal(a.Elements.Get(1), new Type.Identifier("int"));
			Assert.Equal(a.ToString(), "string * int");
			var b = new Type.Tuple(new Type.Identifier("string"), new Type.Identifier("string"));
			Assert.Equal(b.Elements.Get(0), new Type.Identifier("string"));
			Assert.Equal(b.Elements.Get(1), new Type.Identifier("string"));
			Assert.Equal(b.ToString(), "string * string");
			Assert.NotEqual(a, b);
			var c = new Type.Tuple(new Type.Identifier("string"), new Type.Identifier("int"));
			Assert.Equal(c.ToString(), "string * int");
			Assert.Equal(a, c);
		}
	}
}
