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

using System;
using Kean.Extension;

namespace Kean.Collection.Linked.Extension
{
	public static class LinkExtension
	{
		public static L Add<L, T>(this L me, T item)
			where L : class, ILink<L, T>, new()
		{
			return new L()
			{
				Head = item,
				Tail = me,
			};
		}
		public static L Append<L, T>(this L me, T item)
			where L : class, ILink<L, T>, new()
		{
			return (me.Tail.IsNull() ? new L() { Head = item } : me.Tail.Append(item)).Add(me.Head);
		}
		public static T Get<L, T>(this ILink<L, T> me, int index)
			where L : class, ILink<L, T>, new()
		{
			if (me.IsNull())
				throw new IndexOutOfRangeException();
			return index == 0 ? me.Head : me.Tail.Get<L, T>(index - 1);
		}
		public static void Set<L, T>(this ILink<L, T> me, int index, T item)
			where L : class, ILink<L, T>, new()
		{
			if (me.IsNull())
				throw new IndexOutOfRangeException();
			else if (index == 0)
				me.Head = item;
			else
				me.Tail.Set(index - 1, item);
		}
		public static int Count<L, T>(this ILink<L, T> me)
			where L : class, ILink<L, T>, new()
		{
			return me.IsNull() ? 0 : (1 + me.Tail.Count<L, T>());
		}
		public static L Insert<L, T>(this ILink<L, T> me, int index, T item)
			where L : class, ILink<L, T>, new()
		{
			if (me.IsNull() && index > 0)
				throw new IndexOutOfRangeException();
			return (index == 0) ?
				(me as L).Add(item) :
				me.Tail.Insert<L, T>(index - 1, item).Add(me.Head);
		}
		public static L Remove<L, T>(this ILink<L, T> me, int index)
			where L : class, ILink<L, T>, new()
		{
			if (me.IsNull())
				throw new IndexOutOfRangeException();
			return (index == 0) ?
				me.Tail :
				me.Tail.Remove<L, T>(index - 1).Add(me.Head);
		}
		public static L Remove<L, T>(this ILink<L, T> me, int index, out T element)
			where L : class, ILink<L, T>, new()
		{
			L result;
			if (me.IsNull())
				throw new IndexOutOfRangeException();
			else if (index == 0)
			{
				element = me.Head;
				result = me.Tail;
			}
			else
				result = me.Tail.Remove<L, T>(index - 1, out element).Add(me.Head);
			return result;
		}
		public static L Remove<L, T>(this ILink<L, T> me, Func<T, bool> function)
			where L : class, ILink<L, T>, new()
		{
			return me.IsNull() ? null : function(me.Head) ? (me.Tail.NotNull() ? me.Tail.Remove(function) : null) : (me.Tail.NotNull() ? me.Tail.Remove(function).Add(me.Head) : me as L);
		}
		public static bool Equals<L, T>(this ILink<L, T> me, L other)
			where L : class, ILink<L, T>, new()
		{
			return me.Same(other) || me.NotNull() && other.NotNull() && me.Head.Equals(other.Head) && me.Tail.Equals(other.Tail);
		}
		public static R Fold<L, T, R>(this ILink<L, T> me, Func<T, R, R> function)
			where L : class, ILink<L, T>, new()
		{
			return me.Fold(function, default(R));
		}
		public static R Fold<L, T, R>(this ILink<L, T> me, Func<T, R, R> function, R initial)
			where L : class, ILink<L, T>, new()
		{
			return me.IsNull() ? initial : function(me.Head, me.Tail.Fold(function, initial));
		}
		public static R FoldReverse<L, T, R>(this ILink<L, T> me, Func<T, R, R> function)
			where L : class, ILink<L, T>, new()
		{
			return me.FoldReverse(function, default(R));
		}
		public static R FoldReverse<L, T, R>(this ILink<L, T> me, Func<T, R, R> function, R initial)
			where L : class, ILink<L, T>, new()
		{
			return me.IsNull() ? initial : me.Tail.Fold(function, function(me.Head, initial));
		}
		public static void Apply<L, T>(this ILink<L, T> me, Action<T> function)
			where L : class, ILink<L, T>, new()
		{
			if (!me.IsNull())
			{
				function(me.Head);
				me.Tail.Apply(function);
			}
		}
		public static R Map<L, T, R, S>(this ILink<L, T> me, Func<T, S> function)
			where L : class, ILink<L, T>, new()
			where R : class, ILink<R, S>, new()
		{
			return me.IsNull() ? null : new R()
			{
				Head = function(me.Head),
				Tail = me.Tail.Map<L, T, R, S>(function),
			};
		}

		public static int Index<L, T>(this ILink<L, T> me, Func<T, bool> function)
			where L : class, ILink<L, T>, new()
		{
			return me.IsNull() ? -1 : function(me.Head) ? 0 : 1 + me.Tail.Index(function);
		}
		public static T Find<L, T>(this ILink<L, T> me, Func<T, bool> function)
			where L : class, ILink<L, T>, new()
		{
			return me.IsNull() ? default(T) : function(me.Head) ? me.Head : me.Tail.Find(function);
		}
		public static S Find<L, T, S>(this ILink<L, T> me, Func<T, S> function)
			where L : class, ILink<L, T>, new()
			where S : class
		{
			S result = function(me.Head);
			return result ?? (me.Tail.IsNull() ? default(S) : me.Tail.Find(function));
		}
		public static bool Exists<L, T>(this ILink<L, T> me, Func<T, bool> function)
			where L : class, ILink<L, T>, new()
		{
			return me.NotNull() && (function(me.Head) || me.Tail.Exists(function));
		}
		public static bool All<L, T>(this ILink<L, T> me, Func<T, bool> function)
			where L : class, ILink<L, T>, new()
		{
			return me.IsNull() || (function(me.Head) && me.Tail.Exists(function));
		}
		public static System.Collections.Generic.IEnumerator<T> GetEnumerator<L, T>(this ILink<L, T> me)
			where L : class, ILink<L, T>, new()
		{
			while (me.NotNull())
			{
				yield return me.Head;
				me = me.Tail;
			}
		}
	}
}
