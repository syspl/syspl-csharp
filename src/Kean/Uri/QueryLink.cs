// Copyright (C) 2010-2011  Simon Mika <simon@mika.se>
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
using Kean.Collection.Linked.Extension;

namespace Kean.Uri
{
	public class QueryLink :
		Collection.ILink<QueryLink, KeyValue<string, string>>,
		IEquatable<QueryLink>
	{
		public string this[string key]
		{
			get { return this.Head.Key == key ? this.Head.Value : (this.Tail.NotNull() ? this.Tail[key] : null); }
			set
			{
				if (this.Head.Key == key)
					this.Head = KeyValue.Create(key, value);
				else if (this.Tail.NotNull())
					this.Tail[key] = value;
				else
					this.Tail = new QueryLink(key, value);
			}
		}
		public QueryLink()
		{
		}
		public QueryLink(KeyValue<string, string> head) :
			this()
		{
			this.Head = head;
		}
		public QueryLink(string key, string value) :
			this(KeyValue.Create(key, value))
		{ }
		public QueryLink(KeyValue<string, string> head, QueryLink tail) :
			this(head)
		{
			this.Tail = tail;
		}
		public QueryLink(string key, string value, QueryLink tail) :
			this(KeyValue.Create(key, value), tail)
		{ }
		public QueryLink Copy()
		{
			return new QueryLink(this.Head, this.Tail.NotNull() ? this.Tail.Copy() : null);
		}
		public QueryLink Add(string key, string value)
		{
			QueryLink result;
			KeyValue<string, string> item = KeyValue.Create(key, value);
			if (this.head.HasValue)
				result = this.Add(item);
			else
				result = new QueryLink(item);
			return result;
		}
		public QueryLink Remove(params string[] keys)
		{
			return this.Remove((KeyValue<string, string> q) =>
			{
				bool result = false;
				foreach (string key in keys)
					result |= q.Key == key;
				return result;
			});
		}
		public QueryLink Keep(params string[] keys)
		{
			return this.Remove((KeyValue<string, string> q) =>
			{
				bool result = false;
				foreach (string key in keys)
					result |= q.Key == key;
				return !result;
			});
		}

		#region ILink<Query, KeyValue<string, string>> Members
		KeyValue<string, string>? head;
		public KeyValue<string, string> Head
		{
			get { return this.head.HasValue ? this.head.Value : default(KeyValue<string, string>); }
			set { this.head = value; }
		}
		public QueryLink Tail { get; set; }
		#endregion
		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder(this.Head.Key);
				if (this.Head.Value.NotNull())
				{
					result.Append("=");
					result.Append(this.Head.Value);
				}
				if (this.Tail != null)
				{
					result.Append("&");
					result.Append(this.Tail.String);
				}
				return result.ToString();
			}
			set
			{
				if (value.IsEmpty())
				{
					this.Head = KeyValue.Create<string, string>(null, null);
					this.Tail = null;
				}
				else
				{
					string[] splitted = value.Split(new char[] { '&', ';' }, 2);
					string[] keyValue = splitted[0].Split(new char[] { '=' }, 2);
					this.Head = KeyValue.Create(keyValue[0], keyValue.Length > 1 ? keyValue[1] : null);
					this.Tail = splitted.Length > 1 ? new QueryLink() { String = splitted[1] } : null;
				}
			}
		}
		#endregion
		#region IEquatable<Query> Members
		public bool Equals(QueryLink other)
		{
			return other.NotNull() && this.Head == other.Head && this.Tail == other.Tail;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is QueryLink && this.Equals(other as QueryLink);
		}
		public override int GetHashCode()
		{
			return this.Head.Hash() ^ this.Tail.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		#region Operators
		public static bool operator ==(QueryLink left, QueryLink right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(QueryLink left, QueryLink right)
		{
			return !(left == right);
		}
		public static implicit operator string(QueryLink query)
		{
			return query.IsNull() ? null : query.String;
		}
		public static implicit operator QueryLink(string query)
		{
			return query.IsEmpty() ? null : new QueryLink() { String = query };
		}
		#endregion
	}
}