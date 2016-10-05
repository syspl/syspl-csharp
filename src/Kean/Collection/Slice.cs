// Copyright (C) 2009-2011  Simon Mika <simon@mika.se>
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

using System;

namespace Kean.Collection
{
	public class Slice<T> :
		Abstract.Block<T>
	{
		int offset;
		int count;
		IBlock<T> backend;
		public override int Count { get { return this.count; } }
		public override T this [int index]
		{
			get
			{
				if (index >= count)
					throw new IndexOutOfRangeException();
				return this.backend[this.offset + index];
			}
			set
			{
				if (index >= count)
					throw new IndexOutOfRangeException();
				this.backend[this.offset + index] = value;
			}
		}
		public Slice(T[] data, int offset) :
			this(data, offset, data.Length - offset)
		{	}
		public Slice(T[] data, int offset, int count) :
			this((Block<T>)data, offset, count)
		{	}
		public Slice(IBlock<T> data, int offset) :
			this(data, offset, data.Count - offset)
		{	}
		public Slice(IBlock<T> data, int offset, int count)
		{
			if (count < 0)
				count = data.Count + count;
			if (data.Count < offset + count)
				throw new IndexOutOfRangeException();
			this.offset = offset;
			this.count = count;
			this.backend = data;
		}
		public override bool CopyTo(T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			count = System.Math.Min(count == -1 ? target.Length : count, sourceOffset + this.Count);
			sourceOffset += this.offset;
			return this.backend is IArrayCopyable<T> && ((IArrayCopyable<T>) this.backend).CopyTo(target, targetOffset, count, sourceOffset);
		}
	}
}
