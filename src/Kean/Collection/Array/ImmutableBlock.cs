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

namespace Kean.Collection.Array
{
	public class ImmutableBlock<T> :
		Abstract.ImmutableBlock<T>,
		IImmutableBlock<T>
	{
		T[] backend;
		#region Constructor
		public ImmutableBlock(params T[] backend)
		{
			this.backend = backend;
		}
		#endregion
		#region IImmutableBlock<T>
		public override int Count { get { return this.backend.Length; } }
		public override T this[int index] { get { return this.backend[index]; } }
		#endregion
		#region Operators
		public static implicit operator ImmutableBlock<T>(T[] data)
		{
			return new ImmutableBlock<T>(data);
		}
		public static explicit operator T[](ImmutableBlock<T> data)
		{
			return data.backend;
		}
		#endregion
	}
}

