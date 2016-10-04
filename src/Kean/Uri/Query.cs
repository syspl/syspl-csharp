// Copyright (C) 2012  Simon Mika <simon@mika.se>
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

namespace Kean.Uri
{
	public class Query :
		Collection.Linked.Dictionary<QueryLink, string, string>,
		IEquatable<Query>
	{
		public bool Empty { get { return this.Head.IsNull(); } }
		public Query() { }
		public Query Copy()
		{
			return new Query() { Head = this.Head.NotNull() ? this.Head.Copy() : null };
		}
		public void Remove(params string[] keys)
		{
			if (this.Head.NotNull())
				this.Head = this.Head.Remove(keys);
		}
		public void Keep(params string[] keys)
		{
			if (this.Head.NotNull())
				this.Head = this.Head.Keep(keys);
		}
		public int Get(string key, int @default)
		{
			int result;
			if (!int.TryParse(this[key] ?? "", out result))
				result = @default;
			return result;
		}
		public float Get(string key, float @default)
		{
			return this[key].Parse(@default);
		}
		public double Get(string key, double @default)
		{
			return this[key].Parse(@default);
		}
		public bool Get(string key, bool @default)
		{
			return this[key].Parse(@default);
		}
		public bool NotFalse(string key)
		{
			bool result;
			if (!bool.TryParse(this[key] ?? "", out result))
				result = true;
			return result;
		}
		public T GetEnumeration<T>(string key, T @default) where T : struct
		{
			T result;
			if (!Enum.TryParse<T>(this[key], true, out result))
				result = @default;
			return result;
		}

		#region IString Members
		public string String
		{
			get { return this.Head.NotNull() ? this.Head.String.Replace(' ', '+') : ""; }
			set { this.Head = value.NotEmpty() ? new QueryLink() { String = value.Replace('+', ' ') } : null; }
		}
		#endregion
		#region IEquatable<Query> Members
		public bool Equals(Query other)
		{
			return this.Head == other.Head;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Query && this.Equals(other as Query);
		}
		public override int GetHashCode()
		{
			return this.Head.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		#region Operators
		public static bool operator ==(Query left, Query right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Query left, Query right)
		{
			return !(left == right);
		}
		public static implicit operator string(Query query)
		{
			return query.IsNull() ? null : query.String;
		}
		public static implicit operator Query(string query)
		{
			return new Query() { String = query.IsEmpty() ? null : query };
		}
		#endregion
	}
}