// Copyright (C) 2016  Simon Mika <simon@mika.se>
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
using Generic = System.Collections.Generic;

namespace Kean.Collection.Linked
{
	public class List<T> :
		List<Link<T>, T>
	{
		public List() { }
	}
	public class List<L, T> :
		Abstract.List<T>
		where L : class, ILink<L, T>, new()
	{
		public L Last { get; set; }
		public override int Count { get { return this.Last.Count<L, T>(); } }
		public override T this[int index]
		{
			get { return this.Last.Get<L, T>(this.Count - index - 1); }
			set { this.Last.Set<L, T>(this.Count - index - 1, value); }
		}
		#region Constructors
		public List() { }
		public List(params T[] items) :
			this(items as Generic.IEnumerable<T>)
		{ }
		public List(Generic.IEnumerable<T> items) :
			this()
		{
			Collection.Extension.ListExtension.Add(this, items);
		}
		#endregion
		public override Collection.IList<T> Add(T element)
		{
			this.Last = new L()
			{
				Head = element,
				Tail = this.Last,
			};
			return this;
		}
		public override T Remove()
		{
			T result = default(T);
			if (this.Last.NotNull())
			{
				result = this.Last.Head;
				this.Last = this.Last.Tail;
			}
			return result;
		}
		public override T Remove(int index)
		{
			T result;
			this.Last = this.Last.Remove(this.Count - index - 1, out result);
			return result;
		}
		public override Collection.IList<T> Insert(int index, T element)
		{
			this.Last = this.Last.Insert(this.Count - index, element);
			return this;
		}
	}
}
