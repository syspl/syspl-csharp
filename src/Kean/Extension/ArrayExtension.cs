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

namespace Kean.Extension
{
	public static class ArrayExtension
	{
		public static T[] Reverse<T>(this T[] me)
		{
			int half = me.Length / 2;
			for (int i = 0; i < half; i++)
			{
				T t = me[i];
				me[i] = me[me.Length - i - 1];
				me[me.Length - i - 1] = t;
			}
			return me;
		}
		public static T[] Apply<T>(this T[] me, Action<T> function)
		{
			foreach (T element in me)
				function(element);
			return me;
		}
		public static T[] Modify<T>(this T[] me, Func<T, T> function)
		{
			for (int i = 0; i < me.Length; i++)
				me[i] = function(me[i]);
			return me;
		}
		public static void Map<T, S>(this T[] me, S[] result, Func<T, S> function)
		{
			int minimumLength = (me.Length > result.Length) ? result.Length : me.Length;
			for (int i = 0; i < minimumLength; i++)
				result[i] = function(me[i]);
		}
		public static S[] Map<T, S>(this T[] input, Func<T, S> function)
		{
			S[] result = null;
			if (input.NotNull())
			{
				result = new S[input.Length];
				for (int i = 0; i < input.Length; i++)
					result[i] = function(input[i]);
			}
			return result;
		}
		public static int Index<T>(this T[] me, Func<T, bool> function)
		{
			return me.Index(0, function);
		}
		public static int Index<T>(this T[] me, int start, Func<T, bool> function)
		{
			return me.Index(start, me.Length - 1, function);
		}
		public static int Index<T>(this T[] me, int start, int end, Func<T, bool> function)
		{
			int count = me.Length - 1;
			if (start < 0)
				start = count + start;
			if (end < 0)
				end = count + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (function(me[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index<T>(this T[] me, T needle)
		{
			return me.Index(0, needle);
		}
		public static int Index<T>(this T[] me, int start, T needle)
		{
			return me.Index(start, me.Length - 1, needle);
		}
		public static int Index<T>(this T[] me, int start, int end, T needle)
		{
			int count = me.Length - 1;
			if (start < 0)
				start = count + start;
			if (end < 0)
				end = count + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me[i].SameOrEquals(needle))
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index<T>(this T[] me, int start, int end, params T[] needles)
			where T : IEquatable<T>
		{
			int count = me.Length - 1;
			if (start < 0)
				start = count + start;
			if (end < 0)
				end = count + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (needles.Contains(me[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		public static bool Contains<T>(this T[] me, T needle)
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
		public static bool Contains<T>(this T[] me, params T[] needles)
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
		public static T Find<T>(this T[] me, Func<T, bool> function)
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
		public static S Find<T, S>(this T[] me, Func<T, S> function)
		{
			S result = default(S);
			foreach (T element in me)
				if ((result = function(element)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this T[] me, Func<T, bool> function)
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
		public static bool Exists<T>(this T[] me, Func<T, bool> function, int length)
		{
			bool result = false;
			for (int i = 0; i < length; i++)
				if (function(me[i]))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool Exists<T>(this T[] me, Func<T, bool> function, int start, int length)
		{
			bool result = false;
			length += start;
			for (int i = start; i < length; i++)
				if (function(me[i]))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool All<T>(this T[] me, Func<T, bool> function)
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
		public static S Fold<T, S>(this T[] me, Func<T, S, S> function, S initial)
		{
			foreach (T element in me)
				initial = function(element, initial);
			return initial;
		}
		public static T[] Copy<T>(this T[] me)
		{
			T[] result = new T[me.Length];
			System.Buffer.BlockCopy(me, 0, result, 0, me.Length);
			return result;
		}
		public static T[] Sort<T>(this T[] me, Comparison<T> comparison)
		{
			Array.Sort(me, comparison);
			return me;
		}
		public static T[] Sort<T>(this T[] me)
		{
			Array.Sort(me);
			return me;
		}
		public static bool Empty<T>(this T[] me)
		{
			return me.IsNull() || me.Length == 0;
		}
		public static bool NotEmpty<T>(this T[] me)
		{
			return !me.Empty();
		}
		public static T First<T>(this T[] me)
		{
			return me.NotEmpty() ? me[0] : default(T);
		}
		public static T Last<T>(this T[] me)
		{
			return me.NotEmpty() ? me[me.Length - 1] : default(T);
		}
		public static T[] Initialize<T>(this T[] me, T value)
		{
			for (int i = 0; i < me.Length; i++)
				me[i] = value;
			return me;
		}
		public static T[] Initialize<T>(this T[] me, Func<T> create)
		{
			for (int i = 0; i < me.Length; i++)
				me[i] = create();
			return me;
		}
		public static T[] Initialize<T>(this T[] me, Func<int, T> create)
		{
			for (int i = 0; i < me.Length; i++)
				me[i] = create(i);
			return me;
		}
		public static T[] Prepend<T>(this T[] me, T value)
		{
			int length = me.Length;
			T[] result = new T[length + 1];
			Array.Copy(me, 0, result, 1, length);
			result[0] = value;
			return result;
		}
		public static T[] Append<T>(this T[] me, T value)
		{
			int length = me.Length;
			T[] result = new T[length + 1];
			Array.Copy(me, result, length);
			result[me.Length] = value;
			return result;
		}
	}
}
