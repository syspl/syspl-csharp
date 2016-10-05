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
	public class List<T> :
		Block<T>,
		IList<T>
	{
		IList<T> backend;
		#region Constructors
		public List() :	this(0) { }
		public List(int count) : this(0, new Block<T>(count)) { }
		public List(params T[] backend) : this(backend.Length, new Block<T>(backend)) { }
		public List(int count, params T[] backend) : this(count, new Block<T>(backend)) { }
		public List(IBlock<T> backend) :	this(new Wrap.List<T>(backend))	{ }
		public List(int count, IBlock<T> backend) :	this(new Wrap.List<T>(count, backend)) { }
		public List(IList<T> backend)
		{
			this.backend = backend;
		}
		#endregion
		#region IList<T>
		public IList<T> Add(T item)
		{
			return this.backend.Add(item);
		}
		public T Remove()
		{
			return this.backend.Remove();
		}
		public T Remove(int index)
		{
			return this.backend.Remove(index);
		}
		public IList<T> Insert(int index, T item)
		{
			return this.backend.Insert(index, item);
		}
		#endregion
	}
}
