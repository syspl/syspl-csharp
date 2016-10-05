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

namespace Kean.Collection.Extension
{
	public static class QueueExtension
	{
		public static IQueue<T> Enqueue<T>(this IQueue<T> me, System.Collections.Generic.IEnumerable<T> items)
		{
			foreach (T item in items)
				me.Enqueue(item);
			return me;
		}
		public static IQueue<T> Enqueue<T>(this IQueue<T> me, IQueue<T> source)
		{
			while (!source.Empty)
				me.Enqueue(source.Dequeue());
			return me;
		}
		public static IQueue<T> Clear<T>(this IQueue<T> me)
		{
			while (!me.Empty)
				me.Dequeue();
			return me;
		}
	}
}
