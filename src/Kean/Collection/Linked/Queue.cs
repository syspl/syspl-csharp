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

using Kean.Extension;
using Kean.Collection.Linked.Extension;

namespace Kean.Collection.Linked
{
	public class Queue<T> :
		Queue<Link<T>, T>
	{
		public Queue() { }
	}
	public class Queue<L, T> :
		IQueue<T>
		where L : class, ILink<L, T>, new()
	{
		L first;
		L last;
		public bool Empty { get { return this.first == null; } }
		public int Count { get { return this.first.Count<L, T>(); } }
		public Queue() { }
		public Collection.IQueue<T> Enqueue(T item)
		{
			L link = new L() { Head = item };
			if (this.last.IsNull())
				this.first = link;
			else
				this.last.Tail = link;
			this.last = link;
			return this;
		}
		public T Peek()
		{
			return this.first.NotNull() ? this.first.Head : default(T);
		}
		public T Dequeue()
		{
			T result;
			if (this.first.IsNull())
				result = default(T);
			else
			{
				result = this.first.Head;
				this.first = this.first.Tail;
				if (this.first.IsNull())
					this.last = null;
			}
			return result;
		}
	}
}
