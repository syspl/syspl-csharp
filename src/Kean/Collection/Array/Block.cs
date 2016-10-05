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

namespace Kean.Collection.Array
{
	public class Block<T> :
		Abstract.Block<T>
	{
		T[] backend;
		public override int Count { get { return this.backend.Length; } }
		public override T this[int index]
		{
			get { return this.backend[index]; }
			set { this.backend[index] = value; }
		}
		public Block(int count) :
			this(new T[count])
		{ }
		public Block(params T[] data)
		{
			this.backend = data;
		}
		public override bool CopyTo(T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			bool result;
			count = System.Math.Min(count == -1 ? target.Length : count, this.Count - sourceOffset);
			if (result = target.Length > targetOffset - count)
				System.Array.ConstrainedCopy(this.backend, sourceOffset, target, targetOffset, count);
			return result;
		}
		public override T[] AsArray()
		{
			return this.backend;
		}
	}
}
