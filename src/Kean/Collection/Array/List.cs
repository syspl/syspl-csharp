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

using System;
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Collection.Array
{
	public class List<T> :
		Abstract.List<T>
	{
		T[] items;
		static readonly T[] emptyArray = new T[0];
		public int Capacity
		{
			get { return this.items.Length; }
			set
			{
				if (value < this.Count)
					throw new ArgumentOutOfRangeException("Capacity can't be set to {0}, because collection contains {1} elements.".Format(value, this.Count));
				System.Array.Resize<T>(ref this.items, value);
			}
		}
		#region Constructors
		public List()
		{
			this.items = List<T>.emptyArray;
		}
		public List(int capacity) : this()
		{
			this.Capacity = capacity;
		}
		public List(params T[] items) :
			this(items.Length)
		{
			this.Add(items);
		}
		#endregion
		void Decrease ()
		{
			if (this.count > 0)
			{
				this[this.Count - 1] = default(T); // let garbage collection work
				this.count--;
			}
		}
		void Increase ()
		{
			if (this.Capacity <= this.Count + 1)
				this.Capacity = this.Count + 1;
			this.count++;
		}
		public void Trim ()
		{
			this.Capacity = this.Count;
		}
		int VerifyIndex (int index)
		{
			if ((uint)index >= (uint)this.Count)
				throw new IndexOutOfRangeException();
			return index;
		}
		#region IVector<T>
		int count;
		public override int Count { get { return this.count; } }
		public override T this [int index]
		{
			get	{ return this.items[this.VerifyIndex(index)]; }
			set { this.items[this.VerifyIndex(index)] = value; }
		}
		#endregion
		#region IList<T>
		public override IList<T> Add (T item)
		{
			this.Increase();
			this[this.Count - 1] = item;
			return this;
		}
		public override T Remove ()
		{
			return this.Remove(this.Count - 1);
		}
		public override T Remove (int index)
		{
			T result = this[index];
			if (0 <= index && index < this.Count - 1)
				System.Array.Copy(this.items, index + 1, this.items, index, this.Count - index - 1);
			this.Decrease();
			return result;
		}
		public override IList<T> Insert (int index, T item)
		{
			if (index == this.Count)
				this.Add(item);
			else
			{
				this.Increase();
				System.Array.Copy(this.items, index, this.items, index + 1, this.Count - index - 1);
				this[index] = item;
			}
			return this;
		}
		#endregion
		#region Operators
		public static implicit operator List<T> (T[] data)
		{
			return new List<T>(data);
		}
		public static explicit operator T[] (List<T> data)
		{
			return data.items;
		}
		#endregion
	}
}
