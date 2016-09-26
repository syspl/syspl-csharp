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

namespace Kean.Collection
{
	public class Vector<T> :
		Abstract.Vector<T>
	{
		T[] data;
		public override int Count { get { return this.data.Length; } }
		public override T this[int index]
		{
			get { return this.data[index]; }
			set { this.data[index] = value; }
		}
		public Vector(int count) :
			this(new T[count])
		{ }
		public Vector(params T[] data)
		{
			this.data = data;
		}
		public static implicit operator Vector<T>(T[] array)
		{
			return new Vector<T>(array);
		}
		public static explicit operator T[](Vector<T> vector)
		{
			return vector.data;
		}
	}
}
