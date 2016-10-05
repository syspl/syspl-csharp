// Copyright (C) 2011  Simon Mika <simon@mika.se>
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
	public class Queue<T> :
		IQueue<T>
	{
		IQueue<T> backend;
		#region Constructors
		public Queue() : this(new Wrapped.Queue<T>(new List<T>())) { }
		public Queue(int capacity) : this(new List<T>(capacity)) { }
		public Queue(params T[] backend) : this(new List<T>(backend)) { }
		public Queue(int count, params T[] backend) : this(new List<T>(count, backend)) { }
		public Queue(IBlock<T> backend) : this(new List<T>(backend)) { }
		public Queue(int count, IBlock<T> backend) : this(new List<T>(count, backend)) { }
		public Queue(IList<T> backend) : this(new Wrapped.Queue<T>(backend)) { }
		public Queue(IQueue<T> backend)
		{
			this.backend = backend;
		}
		#endregion

		#region IQueue<T> Members
		public bool Empty
		{
			get { return this.backend.Empty; }
		}
		public int Count
		{
			get { return this.backend.Count; }
		}
		public IQueue<T> Enqueue(T item)
		{
			return this.backend.Enqueue(item);
		}
		public T Peek()
		{
			return this.backend.Peek();
		}
		public T Dequeue()
		{
			return this.backend.Dequeue();
		}
		#endregion
	}
}