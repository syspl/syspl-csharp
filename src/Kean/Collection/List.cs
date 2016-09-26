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

namespace Kean.Collection
{
	public class List<T> :
		IList<T>
	{
		Array.List<T> data;
		#region Constructors
		public List() :
			this(0)
		{ }
		public List(int count)
		{
			this.data = new Array.List<T>(count);
		}
		public List(params T[] items)
		{
			this.data = new Array.List<T>(items);
		}
		#endregion
		#region IList<T>
		public int Count { get { return this.data.Count; } }
		public T this[int index]
		{
			get { return this.data[index]; }
			set { this.data[index] = value; }
		}
		public Collection.IList<T> Add(T item)
		{
			return this.data.Add(item);
		}
		public T Remove()
		{
			return this.data.Remove();
		}
		public T Remove(int index)
		{
			return this.data.Remove(index);
		}
		public Collection.IList<T> Insert(int index, T item)
		{
			return this.data.Insert(index, item);
		}
		#endregion
		#region System.Collections.IEnumerable
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this.data as System.Collections.IEnumerable).GetEnumerator();
		}
		#endregion
		#region System.Collections.Generic.IEnumerable<T>
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			return (this.data as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		#endregion
		#region System.IEquatable<Interface.IVector<T>>
		public bool Equals(IVector<T> other)
		{
			return this.data.Equals(other);
		}
		#endregion
		#region System.Object
		public override bool Equals(object other)
		{
			return this.data.Equals(other);
		}
		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}
		#endregion
	}
}
