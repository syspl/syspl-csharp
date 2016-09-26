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

namespace Kean.Collection.Linked
{
	public class Dictionary<TKey, TValue> :
		Dictionary<Link<KeyValue<TKey, TValue>>, TKey, TValue>
	{
		public Dictionary() { }
	}
	public class Dictionary<L, TKey, TValue> :
		IDictionary<TKey, TValue>
		where L : class, ILink<L, KeyValue<TKey, TValue>>, new()
	{
		protected L Head { get; set; }
		public Dictionary() { }

		#region IDictionary<TKey,TValue> Members
		public TValue this[TKey key]
		{
			get { return this.Head.Find(item => item.Key.SameOrEquals(key)).Value; }
			set
			{
				this.Head = this.Replace(this.Head, KeyValue.Create(key, value));
			}
		}
		L Replace(L head, KeyValue<TKey, TValue> item)
		{
			return
				head.IsNull() ? new L() { Head = item } :
				head.Head.Key.SameOrEquals(item.Key) ? new L() { Head = item, Tail = head.Tail } :
				new L() { Head = head.Head, Tail = this.Replace(head.Tail, item) };
		}
		public bool Contains(TKey key)
		{
			return this.Head.Exists(item => item.Key.SameOrEquals(key));
		}
		public bool Remove(TKey key)
		{
			bool result = false;
			this.Head = this.Head.Remove(item =>
			{
				bool r = item.Key.SameOrEquals(key);
				result |= r;
				return r;
			});
			return result;
		}
		#endregion

		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
		{
			return this.Head.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		#region IEquatable<IDictionary<TKey,TValue>> Members
		public bool Equals(IDictionary<TKey, TValue> other)
		{
			bool result = other.NotNull();
			int count = this.Head.Count();
			if (result)
				foreach (KeyValue<TKey, TValue> pair in other)
					if (!(result = count-- == 0 || this[pair.Key].Equals(pair.Value)))
						break;
			return result && count == 0;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is IDictionary<TKey, TValue>) && this.Equals(other as IDictionary<TKey, TValue>);
		}
		public override string ToString()
		{
			return this.Head.ToString();
		}
		public override int GetHashCode()
		{
			return this.Head.GetHashCode();
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Dictionary<L, TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Dictionary<L, TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return !(left == right);
		}
		#endregion
	}
}
