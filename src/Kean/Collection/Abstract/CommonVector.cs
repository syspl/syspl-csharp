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

using Kean.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Collection.Abstract
{
	public abstract class CommonVector<T> :
		Generic.IEnumerable<T>,
		System.IEquatable<IVector<T>>,
		System.IEquatable<IImmutableVector<T>>

	{
		public abstract int Count { get; }
		public CommonVector()
		{
		}
		protected abstract T Get(int index);
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
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
			return other is IVector<T> ? this.Equals(other as IVector<T>) :	other is IImmutableVector<T> && this.Equals(other as IImmutableVector<T>);
		}
		public override int GetHashCode ()
		{
			int result = 0;
			foreach (T item in this as Generic.IEnumerable<T>)
				result = unchecked(result * 31 + item?.GetHashCode() ?? 0);
			return result;
		}
		#endregion
		#region IEquatable<IVector<T>>
		public bool Equals(IVector<T> other)
		{
			bool result = other.NotNull() && this.Count == other.Count;
			for (int i = 0; result && i < this.Count; i++)
				result = this.Get(i).Equals(other[i]);
			return result;
		}
		#endregion
		#region IEquatable<IImmutableVector<T>>
		public bool Equals(IImmutableVector<T> other)
		{
			bool result = other.NotNull() && this.Count == other.Count;
			for (int i = 0; result && i < this.Count; i++)
				result = this.Get(i).Equals(other[i]);
			return result;
		}
		#endregion
	}
}

