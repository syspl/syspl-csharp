// Copyright (C) 2016  Simon Mika <simon@mika.se>
//
// This file is part of SysPL.
//
// SysPL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SysPL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SysPL.  If not, see <http://www.gnu.org/licenses/>.
//

using Generic = System.Collections.Generic;
namespace Kean.Extension
{
	public static class EnumeratorExtension
	{
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
