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

using System;
using Generic = System.Collections.Generic;

namespace Kean.Extension
{
	public static class EnumerableExtension
	{
		public static T First<T>(this Generic.IEnumerable<T> me)
		{
			T result;
			if (me.NotNull())
			{
				using (Generic.IEnumerator<T> enumerator = me.GetEnumerator())
					result = enumerator.MoveNext() ? enumerator.Current : default(T);
			}
			else
				result = default(T);
			return result;
		}
		public static T? FirstOrNull<T>(this Generic.IEnumerable<T> me)
			where T : struct
		{
			T? result;
			if (me.NotNull())
			{
				using (Generic.IEnumerator<T> enumerator = me.GetEnumerator())
					result = enumerator.MoveNext() ? enumerator.Current : default(T);
			}
			else
				result = null;
			return result;
		}
		public static void Apply<T>(this Generic.IEnumerable<T> me, Action<T> function)
		{
			foreach (T element in me)
				function(element);
		}
		public static Generic.IEnumerable<S> Map<T, S>(this Generic.IEnumerable<T> me, Func<T, S> function)
		{
			foreach (T element in me)
				yield return function(element);
		}
		public static int Index<T>(this Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			int result = -1;
			int i = 0;
			foreach (T element in me)
				if (function(element))
				{
					result = i;
					break;
				}
				else
					i++;
			return result;
		}
		public static int Index<T>(this Generic.IEnumerable<T> me, T needle)
		{
			return me.Index(element => element.SameOrEquals(needle));
		}
		public static int Index<T>(this Generic.IEnumerable<T> me, params T[] needles)
			where T : IEquatable<T>
		{
			return me.Index(element => needles.Contains(element));
		}
		public static bool Contains<T>(this Generic.IEnumerable<T> me, T needle)
			where T : IEquatable<T>
		{
			bool result = false;
			foreach (T element in me)
				if (needle.SameOrEquals(element))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool Contains<T>(this Generic.IEnumerable<T> me, params T[] needles)
			where T : IEquatable<T>
		{
			bool result = false;
			foreach (T element in me)
				if (needles.Contains(element))
				{
					result = true;
					break;
				}
			return result;
		}
		public static T Find<T>(this Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			T result = default(T);
			foreach (T element in me)
				if (function(element))
				{
					result = element;
					break;
				}
			return result;
		}
		public static S Find<T, S>(this Generic.IEnumerable<T> me, Func<T, S> function)
		{
			S result = default(S);
			foreach (T element in me)
				if ((result = function(element)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			bool result = false;
			foreach (T element in me)
				if (function(element))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool All<T>(this Generic.IEnumerable<T> me, Func<T, bool> function)
		{
			bool result = true;
			foreach (T element in me)
				if (!function(element))
				{
					result = false;
					break;
				}
			return result;
		}
		public static bool All<T>(this Generic.IEnumerable<T> me, Func<T, bool, bool> function)
		{
			bool result = true;
			Generic.IEnumerator<T> enumerator = me.GetEnumerator();
			bool notLast = enumerator.MoveNext();
			while (notLast)
			{
				T current = enumerator.Current;
				if (!function(current, !(notLast = enumerator.MoveNext())))
				{
					result = false;
					break;
				}
			}
			return result;
		}
		public static S Fold<T, S>(this Generic.IEnumerable<T> me, Func<T, S, S> function, S initial)
		{
			foreach (T element in me)
				initial = function(element, initial);
			return initial;
		}
		#region Prepend, Append
		public static Generic.IEnumerable<T> Prepend<T>(this Generic.IEnumerable<T> me, params T[] other)
		{
			// Analysis disable RedundantCast
			return me.Prepend((Generic.IEnumerable<T>)other);
			// Analysis restore RedundantCast
		}
		public static Generic.IEnumerable<T> Prepend<T>(this Generic.IEnumerable<T> me, Generic.IEnumerable<T> other)
		{
			if (other.NotNull())
				foreach (T item in other)
					yield return item;
			if (me.NotNull())
				foreach (T item in me)
					yield return item;
		}
		public static Generic.IEnumerable<T> Append<T>(this Generic.IEnumerable<T> me, params T[] other)
		{
			return me.Append(other);
		}
		public static Generic.IEnumerable<T> Append<T>(this Generic.IEnumerable<T> me, Generic.IEnumerable<T> other)
		{
			if (me.NotNull())
				foreach (T item in me)
					yield return item;
			if (other.NotNull())
				foreach (T item in other)
					yield return item;
		}
		#endregion
		public static Generic.IEnumerable<T> Where<T>(this Generic.IEnumerable<T> me, Func<T, bool> predicate)
		{
			foreach (T element in me)
				if (predicate(element))
					yield return element;
		}
		public static bool CopyTo<T>(this Generic.IEnumerable<T> me, T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			bool result;
			if (me is IArrayCopyable<T>)
				result = ((IArrayCopyable<T>)me).CopyTo(target, targetOffset, count, sourceOffset);
			else
			{
				foreach (var element in me)
				{
					if (targetOffset >= (count == -1 ? target.Length : count))
						break;
					if (sourceOffset > 0)
						sourceOffset--;
					else
						target[targetOffset++] = element;
				}
				result = targetOffset > 0;
			}
			return result;
		}
		/// <summary>
		/// Extracts an array from <paramref>me</paramref>. Changing the resulting array may break <paramref>me</paramref>.
		/// </summary>
		/// <param name="me">Block to extract array from.</param>
		/// <returns>The content of <paramref>me</paramref> as an array. It may or may not be a copy.</returns>
		public static T[] AsArray<T>(this Generic.IEnumerable<T> me)
		{
			return (me as IAsArray<T>)?.AsArray() ?? me.ToArray();
		}
		static T[] ToArrayHelper<T>(this Generic.IEnumerator<T> me, int count)
		{
			T[] result;
			if (me.MoveNext())
			{
				T element = me.Current;
				result = me.ToArrayHelper(count + 1);
				result[count] = element;
			}
			else
				result = new T[count];
			return result;
		}
		/// <summary>
		/// Copies data of <paramref>me</paramref> into a new array.
		/// </summary>
		/// <param name="me">Block to copy data from.</param>
		/// <returns>A new array containing the data from <paramref>me</paramref>.</returns>
		public static T[] ToArray<T>(this Generic.IEnumerable<T> me)
		{
			return me.GetEnumerator().ToArrayHelper(0);
		}
		public static Generic.IEnumerable<S> Cast<T, S>(this Generic.IEnumerable<T> me, Func<T, S> cast)
		{
			foreach (T element in me)
				yield return cast(element);
		}
		public static T Get<T>(this Generic.IEnumerable<T> me, int index)
		{
			return me.GetEnumerator().Skip(index).Next();
		}
		#region Last
		public static T Last<T>(this Generic.IEnumerable<T> me)
		{
			T last = default(T);
			foreach (T item in me)
				last = item;
			return last;
		}
		#endregion
		#region Skip
		public static Generic.IEnumerable<T> Skip<T>(this Generic.IEnumerable<T> me, int count)
		{
			var enumerator = me.GetEnumerator();
			while (count > 0 && enumerator.MoveNext())
				count--;
			while (enumerator.MoveNext())
				yield return enumerator.Current;
		}
		public static Generic.IEnumerable<T> Skip<T>(this Generic.IEnumerable<T> me, params T[] separator)
			where T : IEquatable<T>
		{
			int position = 0;
			var enumerator = me.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!separator[position++].Equals(enumerator.Current))
					position = 0;
				else if (separator.Length == position)
					break;
			}
			while (enumerator.MoveNext())
				yield return enumerator.Current;
		}
		#endregion
		#region Read
		public static Generic.IEnumerable<T> Read<T>(this Generic.IEnumerable<T> me, int count)
		{
			if (count > 0)
			{
				var enumerator = me.GetEnumerator();
				do
					yield return enumerator.Current;
				while (--count > 0 && enumerator.MoveNext());
			}
		}
		public static Generic.IEnumerable<T> Read<T>(this Generic.IEnumerable<T> me, params T[] separator)
			where T : IEquatable<T>
		{
			int position = 0;
			var enumerator = me.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!separator[position++].Equals(enumerator.Current))
				{
					for (int i = 0; i < position - 1; i++)
						yield return separator[i];
					yield return enumerator.Current;
					position = 0;
				}
				else if (separator.Length == position)
					break;
			}
		}
		#endregion
		public static string Join(this Generic.IEnumerable<string> me)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			Generic.IEnumerator<string> enumerator = me.GetEnumerator();
			while (enumerator.MoveNext())
				result.Append(enumerator.Current);
			return result.ToString();
		}
		public static string Join(this Generic.IEnumerable<string> me, string separator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			Generic.IEnumerator<string> enumerator = me.GetEnumerator();
			if (enumerator.MoveNext())
			{
				result.Append(enumerator.Current);
				while (enumerator.MoveNext())
					result.Append(separator).Append(enumerator.Current);
			}
			return result.ToString();
		}
		public static Generic.IEnumerable<char> Decode(this Generic.IEnumerable<byte> me)
		{
			return me.Decode(System.Text.Encoding.UTF8);
		}
		public static Generic.IEnumerable<char> Decode(this Generic.IEnumerable<byte> me, System.Text.Encoding encoding)
		{
			//if (encoding == System.Text.Encoding.UTF8)
			{
				byte[] buffer = new byte[6];
				int length = 0;
				Generic.IEnumerator<byte> enumerator = me.GetEnumerator();
				while (enumerator.MoveNext())
				{
					buffer[0] = enumerator.Current;
					length =
						buffer[0] < 0x80 ? 1 :
						buffer[0] < 0xc0 ? 0 :
						buffer[0] < 0xe0 ? 2 :
						buffer[0] < 0xf0 ? 3 :
						buffer[0] < 0xf8 ? 4 :
						buffer[0] < 0xfc ? 5 :
						6;
					if (length > 0)
					{
						int i = 1;
						for (; i < length && enumerator.MoveNext(); i++)
							buffer[i] = enumerator.Current;
						if (length == 3 && buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
						{
							length = 0; // Skip "zero width no break space" (0xefbbbf)
													//yield return ;
						}
						if (i == length)
							foreach (char c in encoding.GetChars(buffer, 0, length))
								yield return c;
					}
				}
			}
		}
		public static string Join(this Generic.IEnumerable<char> me)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			foreach (char c in me)
				result.Append(c);
			return result.ToString();
		}
		public static bool SameOrEquals<T>(this Generic.IEnumerable<T> me, Generic.IEnumerable<T> other) where T : IEquatable<T>
		{
			bool result = me.NotNull() && other.NotNull() || me.IsNull() && me.NotNull();
			if (result)
			{
				var meEnumerator = me.GetEnumerator();
				var otherEnumerator = other.GetEnumerator();
				result = meEnumerator.MoveNext() == otherEnumerator.MoveNext();
				while (result)
				{
					var meMoved = meEnumerator.MoveNext();
					var otherMoved = otherEnumerator.MoveNext();
					if (meMoved && otherMoved)
						result = meEnumerator.Current.IsNull() && otherEnumerator.Current.IsNull() || meEnumerator.Current.Equals(otherEnumerator.Current);
					else
					{
						result = meMoved == otherMoved;
						break;
					}
				}
			}
			return result;
		}
	}
}
