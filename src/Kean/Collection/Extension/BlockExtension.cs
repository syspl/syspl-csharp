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

namespace Kean.Collection.Extension
{
	public static class BlockExtension
	{
		public static IBlock<T> Reverse<T>(this IBlock<T> me)
		{
			int half = me.Count / 2;
			for (int i = 0; i < half; i++)
			{
				T t = me[i];
				me[i] = me[me.Count - i - 1];
				me[me.Count - i - 1] = t;
			}
			return me;
		}
		public static IBlock<T> Apply<T>(this IBlock<T> me, Action<T> function)
		{
			foreach (T element in me)
				function(element);
			return me;
		}
		public static IBlock<T> Modify<T>(this IBlock<T> me, Func<T, T> function)
		{
			for (int i = 0; i < me.Count; i++)
				me[i] = function(me[i]);
			return me;
		}
		public static void Map<T, S>(this IBlock<T> me, IBlock<S> output, Func<T, S> function)
		{
			int minimumLength = (me.Count > output.Count) ? output.Count : me.Count;
			for (int i = 0; i < minimumLength; i++)
				output[i] = function(me[i]);
		}
		public static IBlock<S> Map<T, S>(this IBlock<T> me, Func<T, S> function)
		{
			IBlock<S> result = new Block<S>(me.Count);
			for (int i = 0; i < me.Count; i++)
				result[i] = function(me[i]);
			return result;
		}
		public static int Index<T>(this IBlock<T> me, Func<T, bool> function)
		{
			int result = -1;
			for (int i = 0; me.NotNull() && i < me.Count; i++)
				if (function(me[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		public static T Find<T>(this IBlock<T> me, Func<T, bool> function)
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
		public static S Find<T, S>(this IBlock<T> me, Func<T, S> function)
			where S : class
		{
			S result = default(S);
			foreach (T element in me)
				if ((result = function(element)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this IBlock<T> me, Func<T, bool> function)
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
		public static bool All<T>(this IBlock<T> me, Func<T, bool> function)
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
		public static S Fold<T, S>(this IBlock<T> me, Func<T, S, S> function, S initial)
		{
			foreach (T element in me)
				initial = function(element, initial);
			return initial;
		}
		public static bool CopyTo<T>(this IBlock<T> me, T[] target, int targetOffset = 0, int count = -1, int sourceOffset = 0)
		{
			bool result;
			if (me is IArrayCopyable<T>)
				result = ((IArrayCopyable<T>) me).CopyTo(target, targetOffset, count, sourceOffset);
			else
			{
				count = System.Math.Min(count == -1 ? target.Length : count, me.Count - sourceOffset);
				for (int i = 0; i < count; i++)
					target[targetOffset + i] = me[sourceOffset + i];
				result = count > 0;
			}
			return result;
		}
		/// <summary>
		/// Extracts an array from <paramref>me</paramref>. Changing the resulting array may break <paramref>me</paramref>.
		/// </summary>
		/// <param name="me">Block to extract array from.</param>
		/// <returns>The content of <paramref>me</paramref> as an array. It may or may not be a copy.</returns>
		public static T[] AsArray<T>(this IBlock<T> me)
		{
			return (me as IAsArray<T>)?.AsArray() ?? me.ToArray();
		}
		/// <summary>
		/// Copies data of <paramref>me</paramref> into a new array.
		/// </summary>
		/// <param name="me">Block to copy data from.</param>
		/// <returns>A new array containing the data from <paramref>me</paramref>.</returns>
		public static T[] ToArray<T>(this IBlock<T> me)
		{
			var result = new T[me.Count];
			return me.CopyTo(result) ? result : null;
		}
		public static Slice<T> Slice<T>(this IBlock<T> me, int offset)
		{
			return new Slice<T>(me, offset);
		}
		public static Slice<T> Slice<T>(this IBlock<T> me, int offset, int count)
		{
			return new Slice<T>(me, offset, count);
		}
		public static IBlock<T> Merge<T>(this IBlock<T> me, IBlock<T> other)
		{
			return me.IsNull() ? other :
				other.IsNull() ? me :
				new Merge<T>(me, other);
		}
		public static IBlock<T> Merge<T>(this IBlock<T> me, T[] other)
		{
			return me.IsNull() ? (Block<T>)other :
				other.IsNull() ? me :
				new Merge<T>(me, (Block<T>)other);
		}
		public static IBlock<T> Resize<T>(this IBlock<T> me, int count)
		{
			return me.Count > count ? me.Slice(0, count) : me.Merge(new Block<T>(count - me.Count));
		}
	}
}
