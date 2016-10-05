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

namespace Kean.Collection.Abstract
{
	public abstract class ImmutableBlock<T> :
		CommonBlock<T>,
		IImmutableBlock<T>
	{
		public abstract T this[int index] { get; }
		protected ImmutableBlock()
		{ }
		protected sealed override T Get (int index)
		{
			return this[index];
		}
	}
}