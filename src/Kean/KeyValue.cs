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

using System;
using Kean.Extension;

namespace Kean
{
	public static class KeyValue
	{
		public static KeyValue<K, V> Create<K, V>(K key, V value)
		{
			return new KeyValue<K, V>(key, value);
		}
	}
	public struct KeyValue<K, V> :
		IEquatable<KeyValue<K, V>>
	{
		public K Key;
		public V Value;
		public KeyValue(K key, V value) :
			this()
		{
			this.Key = key;
			this.Value = value;
		}
		#region Object overrides
		public override string ToString()
		{
			return string.Format("({0} = {1})", this.Key, this.Value);
		}
		public override int GetHashCode()
		{
			return this.Key.Hash() ^ this.Value.Hash();
		}
		public override bool Equals(object other)
		{
			return other is KeyValue<K, V> && this.Equals((KeyValue<K, V>)other);
		}
		#endregion
		#region IEquatable<KeyValue<K, V>> Members
		public bool Equals(KeyValue<K, V> other)
		{
			return other.Key.SameOrEquals(this.Key) &&
				other.Value.SameOrEquals(this.Value);
		}
		#endregion
		#region Casts
		public static implicit operator KeyValue<K, V>(Tuple<K, V> tuple)
		{
			return new KeyValue<K, V>(tuple.Item1, tuple.Item2);
		}
		public static implicit operator Tuple<K, V>(KeyValue<K, V> keyValue)
		{
			return Tuple.Create(keyValue.Key, keyValue.Value);
		}
		#endregion
		#region Equality Operators
		public static bool operator ==(KeyValue<K, V> left, KeyValue<K, V> right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(KeyValue<K, V> left, KeyValue<K, V> right)
		{
			return !(left == right);
		}
		#endregion
	}
}
