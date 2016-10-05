// Copyright (C) 2011-2012  Simon Mika <simon@mika.se>
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
using Kean.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Collection.Abstract
{
	public abstract class CommonBlock<T> :
		IArrayCopyable<T>,
		IAsArray<T>,
		Generic.IEnumerable<T>,
		IEquatable<IBlock<T>>,
		IEquatable<IImmutableBlock<T>>
	{
		public abstract int Count { get; }
		protected CommonBlock()
		{ }
		protected abstract T Get(int index);
		public virtual bool CopyTo(T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			count = System.Math.Min(count == -1 ? target.Length : count, this.Count - sourceOffset);
			foreach (var element in this) {
				if (targetOffset >= count)
					break;
				if (sourceOffset > 0)
					sourceOffset--;
				else
					target[targetOffset++] = element;
			}
			return count > 0;
		}
		public virtual T[] AsArray()
		{
			return this.ToArray();
		}
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as Generic.IEnumerable<T>).GetEnumerator();
		}
		Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
				yield return this.Get(i);
		}
		#endregion
		#region Object override
		public override bool Equals(object other)
		{
			return other is IBlock<T> ? this.Equals(other as IBlock<T>) : other is IImmutableBlock<T> && this.Equals(other as IImmutableBlock<T>);
		}
		public override int GetHashCode()
		{
			int result = 0;
			foreach (T item in this as Generic.IEnumerable<T>)
				result = unchecked(result * 31 + item?.GetHashCode() ?? 0);
			return result;
		}
		#endregion
		#region IEquatable<IBlock<T>>
		public bool Equals(IBlock<T> other)
		{
			bool result = other.NotNull() && this.Count == other.Count;
			for (int i = 0; result && i < this.Count; i++)
				result = this.Get(i).Equals(other[i]);
			return result;
		}
		#endregion
		#region IEquatable<IImmutableBlock<T>>
		public bool Equals(IImmutableBlock<T> other)
		{
			bool result = other.NotNull() && this.Count == other.Count;
			for (int i = 0; result && i < this.Count; i++)
				result = this.Get(i).Equals(other[i]);
			return result;
		}
		#endregion
		#region Equality Operators
		public static bool operator ==(CommonBlock<T> left, IBlock<T> right) {
			return right.Equals(left);
		}
		public static bool operator !=(CommonBlock<T> left, IBlock<T> right) {
			return !right.Equals(left);
		}
		public static bool operator ==(IBlock<T> left, CommonBlock<T> right) {
			return right.Equals(left);
		}
		public static bool operator !=(IBlock<T> left, CommonBlock<T> right) {
			return !right.Equals(left);
		}
		#endregion
		#region Casts
		public static explicit operator T[](CommonBlock<T> block) {
			T[] result;
			return block.NotNull() && block.CopyTo(result = new T[block.Count]) ? result : null;
		}
		#endregion
	}
}

