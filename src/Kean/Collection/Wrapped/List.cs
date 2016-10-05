// Copyright (C) 2009, 2016  Simon Mika <simon@mika.se>
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

namespace Kean.Collection.Wrapped
{
	public class List<T> :
		Abstract.List<T>
	{
		IBlock<T> backend;
		public int Capacity
		{
			get { return this.backend.Count; }
			set
			{
				if (value < this.Count)
					throw new ArgumentOutOfRangeException("Capacity can't be set to {0}, because collection contains {1} elements.".Format(value, this.Count));
				this.backend = this.backend.Resize(value);
			}
		}
		#region Constructors
		public List(params T[] backend) : this(backend.Length, backend) { }
		public List(int count, params T[] backend) : this(count, new Block<T>(backend)) { }
		public List(IBlock<T> backend) : this(backend.Count, backend) { }
		public List(int count, IBlock<T> backend)
		{
			this.count = count;
			this.backend = backend;
		}
		#endregion
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
		#region IBlock<T>
		int count;
		public override int Count { get { return this.count; } }
		public override T this [int index]
		{
			get	{ return this.backend[this.VerifyIndex(index)]; }
			set { this.backend[this.VerifyIndex(index)] = value; }
		}
		#endregion
		#region IList<T>
		public override IList<T> Add (T item)
		{
			if (this.Capacity <= this.Count + 1)
				this.Capacity = this.CapacityCeiling(this.Count + 1);
			this.count++;
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
			if (index == this.Count - 1)
				this.backend[index] = default(T); // Let the garbage collector work.
			else // index < this.Count - 1) && index >= 0 (First line of function will verify that index is within the valid range.)
				this.backend = this.backend.Slice(0, index).Merge(this.backend.Slice(index));
			this.count--;
			return result;
		}
		public override IList<T> Insert (int index, T item)
		{
			if (index == this.Count)
				this.Add(item);
			else
			{
				this.backend = this.backend.Slice(0, index).Merge(new T[] { item }).Merge(this.Slice(index));
				this.count++;
				this[index] = item;
			}
			return this;
		}
		#endregion
		#region IArrayCopyable
		public override bool CopyTo(T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			return this.backend.CopyTo(target, targetOffset, count, sourceOffset);
		}
		#endregion
	}
}
