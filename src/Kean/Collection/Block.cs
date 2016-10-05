// Copyright (C) 2009  Simon Mika <simon@mika.se>
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

using Kean.Collection.Extension;

namespace Kean.Collection
{
	public class Block<T> :
		Abstract.Block<T>,
		IArrayCopyable<T>
	{
		IBlock<T> backend;
		public override int Count { get { return this.backend.Count; } }
		public override T this[int index]
		{
			get { return this.backend[index]; }
			set { this.backend[index] = value; }
		}
		public Block(int count) :
			this(new T[count])
		{ }
		public Block(params T[] data) :
			this(new Array.Block<T>(data))
		{ }
		public Block(IBlock<T> backend)
		{
			this.backend = backend;
		}
		public override bool CopyTo(T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			return this.backend.CopyTo(target, targetOffset, count, sourceOffset);
		}
		public override T[] AsArray()
		{
			return this.backend.AsArray();
		}
		public static Block<T> Empty { get; } = new Block<T>(0);
		public static implicit operator Block<T>(T[] array)
		{
			return new Block<T>(array);
		}
		public static explicit operator T[](Block<T> block)
		{
			return block.AsArray();
		}
	}
}
