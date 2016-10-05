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

namespace Kean.Collection.Abstract
{
	public abstract class List<T> :
		Block<T>,
		IList<T>
	{
		protected List()
		{ }
		protected int CapacityCeiling(int capacity)
		{
			// It would be nice to put some science behind these numbers, bachelor thesis work?
			return
				capacity <= 16 ? 16 :
				capacity <= 32 ? 32 :
				capacity <= 64 ? 64 :
				capacity <= 128 ? 128 :
				(capacity / 256 + 1) * 256;
		}
		public abstract IList<T> Add(T item);
		public abstract T Remove();
		public abstract IList<T> Insert(int index, T item);
		public abstract T Remove(int index);
	}
}
