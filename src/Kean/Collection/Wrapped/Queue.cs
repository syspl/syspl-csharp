// Copyright (C) 2010  Simon Mika <simon@mika.se>
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

namespace Kean.Collection.Wrapped
{
	public class Queue<T> :
		IQueue<T>,
		IArrayCopyable<T>,
		IAsArray<T>
	{
		IList<T> backend;
		int head;
		int tail;
		int count;
		public Queue(IBlock<T> backend) : this(new List<T>(backend)) { }
		public Queue(int count, IBlock<T> backend) : this(new List<T>(count, backend)) { }
		public Queue(T[] backend) : this(new List<T>(backend)) { }
		public Queue(int count, T[] backend) : this(new List<T>(count, backend)) { }
		public Queue(IList<T> backend)
		{
			this.backend = backend;
			this.head = 0;
			this.tail = backend.Count - 1;
			this.count = backend.Count;
		}
		#region IQueue<T>
		public bool Empty { get { return this.count == 0; } }
		public int Count { get { return this.count; } }
		public IQueue<T> Enqueue(T item)
		{
			if (this.count == this.backend.Count)
			{
				this.backend.Insert(this.head, item);
				if (this.head > this.tail)
					this.head++;
				this.tail = (this.tail + 1) % this.backend.Count;
			}
			else
			{
				this.head = (this.head - 1 + this.backend.Count) % this.backend.Count;
				this.backend[this.head] = item;
			}
			this.count++;
			return this;
		}
		public T Peek()
		{
			return this.count == 0 ? default(T) : this.backend[this.tail];
		}
		public T Dequeue()
		{
			T result = this.Peek();
			// let garbage collector do its job
			if (this.backend.Count > 0)
			{
				this.backend[this.tail] = default(T);
				this.tail = (this.tail - 1 + this.backend.Count) % this.backend.Count;
				this.count--;
			}
			return result;
		}
		#endregion
		#region IArrayCopyable
		public bool CopyTo(T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			count = System.Math.Min(count == -1 ? target.Length : count, this.Count - sourceOffset);
			IBlock<T> backend;
			switch (this.head.CompareTo(this.tail)) {
				case -1:
					backend = this.backend.Slice(this.tail).Merge(this.backend.Slice(0, this.head + 1));
					break;
				default:
				case 0:
					backend = Collection.Block<T>.Empty;
					break;
				case 1:
					backend = this.backend.Slice(this.tail, this.Count);
					break;
			}
			return backend.CopyTo(target, targetOffset, count, sourceOffset);
		}
		#endregion
		#region IAsArray
		public T[] AsArray() {
			return this.backend is IAsArray<T> && this.head == 0 && this.tail == this.backend.Count - 1 ? ((IAsArray<T>) this.backend).AsArray() : this.ToArray();
		}
		#endregion
	}
}
