// Copyright (C) 2012, 2016  Simon Mika <simon@mika.se>
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

namespace Kean
{
	/// <summary>
	/// Enables collections to expose there inner data as an array without needing to copy it if possible.
	/// </summary>
	public interface IAsArray<T>
	{
		/// <summary>
		/// Extracts an array from <paramref>me</paramref>. Changing the resulting array may break <paramref>me</paramref>.
		/// </summary>
		/// <param name="me">Collection to extract array from.</param>
		/// <returns>The content of <paramref>me</paramref> as an array. It may or may not be a copy.</returns>
		T[] AsArray();
	}
}
