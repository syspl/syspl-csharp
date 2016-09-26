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
using Generic = System.Collections.Generic;

namespace Kean.Extension
{
	public static class EnumeratorExtension
	{
				public static T Next<T>(this Generic.IEnumerator<T> me)
		{
			return me.NotNull() && me.MoveNext() ? me.Current : default(T);
		}
		public static T First<T>(this Generic.IEnumerator<T> me)
		{
			T result;
			if (me.NotNull())
			{
				result = me.MoveNext() ? me.Current : default(T);
				me.Dispose();
			}
			else
				result = default(T);
			return result;
		}
		public static Generic.IEnumerator<T> Restart<T>(this Generic.IEnumerator<T> me)
		{
			me.Reset();
			return me;
		}
		public static void Apply<T>(this Generic.IEnumerator<T> me, Action<T> function)
		{
			while (me.MoveNext())
				function(me.Current);
		}
		public static Generic.IEnumerator<S> Map<T, S>(this Generic.IEnumerator<T> me, Func<T, S> function)
		{
			while (me.MoveNext())
				yield return function(me.Current);
		}
		public static int Index<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			int result = -1;
			int i = 0;
			while (me.MoveNext())
				if (function(me.Current))
				{
					result = i;
					break;
				}
				else
					i++;
			return result;
		}
		public static int Index<T>(this Generic.IEnumerator<T> me, T needle)
		{
			return me.Index(element => element.SameOrEquals(needle));
		}
		public static int Index<T>(this Generic.IEnumerator<T> me, params T[] needles)
			where T : IEquatable<T>
		{
			return me.Index(element => needles.Contains(element));
		}
		public static bool Contains<T>(this Generic.IEnumerator<T> me, T needle)
			where T : IEquatable<T>
		{
			bool result = false;
			while (me.MoveNext())
				if (needle.SameOrEquals(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool Contains<T>(this Generic.IEnumerator<T> me, params T[] needles)
			where T : IEquatable<T>
		{
			bool result = false;
			while (me.MoveNext())
				if (needles.Contains(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static T Find<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			T result = default(T);
			while (me.MoveNext())
				if (function(me.Current))
				{
					result = me.Current;
					break;
				}
			return result;
		}
		public static S Find<T, S>(this Generic.IEnumerator<T> me, Func<T, S> function)
		{
			S result = default(S);
			while (me.MoveNext())
				if ((result = function(me.Current)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			bool result = false;
			while (me.MoveNext())
				if (function(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool All<T>(this Generic.IEnumerator<T> me, Func<T, bool> function)
		{
			bool result = true;
			while (me.MoveNext())
				if (!function(me.Current))
				{
					result = false;
					break;
				}
			return result;
		}
		public static S Fold<T, S>(this Generic.IEnumerator<T> me, Func<T, S, S> function, S initial)
		{
			while (me.MoveNext())
				initial = function(me.Current, initial);
			return initial;
		}
		public static Generic.IEnumerator<S> Cast<T, S>(this Generic.IEnumerator<T> me, Func<T, S> cast)
		{
			while (me.MoveNext())
				yield return cast(me.Current);
		}
		#region Skip
		/// <summary>
		/// Skip the next <paramref name="count"/> elements in <paramref name="me"/>.
		/// </summary>
		/// <param name="me">Enumerator to skip in.</param>
		/// <param name="count">Number of elements to skip.</param>
		/// <typeparam name="T">Any type.</typeparam>
		public static Generic.IEnumerator<T> Skip<T>(this Generic.IEnumerator<T> me, int count)
		{
			while (count > 0 && me.MoveNext())
				count--;
			return me;
		}
		/// <summary>
		/// Skip past the first occurance of separator.
		/// </summary>
		/// <param name="me">Enumerator to skip on.</param>
		/// <param name="separator">Separator to skip past. Shall not contain null.</param>
		/// <typeparam name="T">Any type implementing <c>IEquatable</c>.</typeparam>
		public static Generic.IEnumerator<T> Skip<T>(this Generic.IEnumerator<T> me, params T[] separator)
			where T : IEquatable<T>
		{
			int position = 0;
			while (me.MoveNext())
			{
				if (!me.Current.Equals(separator[position++]))
					position = 0;
				else if (separator.Length == position)
					break;
			}
			return me;
		}
		#endregion
		#region Read
		/// <summary>
		/// Return new enumerator containing the next <paramref name="count"/> elements in <paramref name="me"/>.
		/// </summary>
		/// <param name="me">Enumerator to read from.</param>
		/// <param name="count">Number of elements read.</param>
		/// <typeparam name="T">Any type.</typeparam>
		public static Generic.IEnumerator<T> Read<T>(this Generic.IEnumerator<T> me, int count)
		{
			if (count > 0)
				do
					yield return me.Current;
				while (--count > 0 && me.MoveNext());
		}
		/// <summary>
		/// Create an enumerator containing all elements in <paramref name="me"/> until <paramref name="separator"/>.
		/// </summary>
		/// <param name="me">Enumerator to read from.</param>
		/// <param name="separator">Separator to read from and move past.</param>
		/// <typeparam name="T">Any type implementing <c>IEquatable</c>.</typeparam>
		public static Generic.IEnumerator<T> Read<T>(this Generic.IEnumerator<T> me, params T[] separator)
			where T : IEquatable<T>
		{
			int position = 0;
			while (me.MoveNext())
			{
				if (!me.Current.Equals(separator[position++]))
				{
					for (int i = 0; i < position - 1; i++)
						yield return separator[i];
					yield return me.Current;
					position = 0;
				}
				else if (separator.Length == position)
					yield break;
			}
		}
		#endregion
		public static string Join(this Generic.IEnumerator<string> me, string separator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			if (me.MoveNext())
			{
				result.Append(me.Current);
				while (me.MoveNext())
					result.Append(separator).Append(me.Current);
			}
			return result.ToString();
		}

		static T[] ToArray<T>(this Generic.IEnumerator<T> me, int count)
		{
			T[] result;
			if (me.MoveNext())
			{
				var head = me.Current;
				result = me.ToArray(count + 1);
				result[count] = head;
			}
			else
				result = new T[count + 1];
			return result;
		}
		public static T[] ToArray<T>(this Generic.IEnumerator<T> me)
		{
			return me.ToArray(0);
		}
	}
}
