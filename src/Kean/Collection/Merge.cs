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

using Kean.Collection.Extension;

namespace Kean.Collection
{
	public class Merge<T> :
		Abstract.Block<T>
	{
		IBlock<T> left;
		IBlock<T> right;
		public override int Count { get { return this.left.Count + this.right.Count; } }
		public override T this[int index]
		{
			get
			{
				int leftCount = this.left.Count;
				return index < leftCount ? this.left[index] : this.right[index - leftCount];
			}
			set
			{
				int leftCount = this.left.Count;
				if (index < leftCount)
					this.left[index] = value;
				else
					this.right[index - leftCount] = value;
			}
		}
		public Merge(T[] left, T[] right) :
			this((Block<T>)left, (Block<T>)right)
		{ }
		public Merge(IBlock<T> left) :
			this(left, new Block<T>(0))
		{ }
		public Merge(IBlock<T> left, IBlock<T> right)
		{
			this.left = left ?? new Block<T>(0);
			this.right = right ?? new Block<T>(0);
		}
		public static implicit operator Merge<T>(T[] value)
		{
			return new Merge<T>(value, new T[0]);
		}
		public static Merge<T> operator +(Merge<T> left, IBlock<T> right)
		{
			return new Merge<T>(left, right);
		}
		public static Merge<T> operator +(IBlock<T> left, Merge<T> right)
		{
			return new Merge<T>(left, right);
		}
		public override bool CopyTo(T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			bool result = false;
			count = System.Math.Min(count == -1 ? target.Length : count, this.Count - sourceOffset);
			var leftCount = System.Math.Min(count, this.left.Count - sourceOffset);
			if (leftCount > 0)
			{
				result = this.left.CopyTo(target, targetOffset, leftCount, sourceOffset);
				targetOffset += leftCount;
				sourceOffset = 0;
			}
			else
				sourceOffset += leftCount;
			var rightCount = count - leftCount;
			if (rightCount > 0)
				result = this.right.CopyTo(target, targetOffset, rightCount, sourceOffset);
			return result;
		}
	}
}
