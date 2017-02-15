// Copyright (C) 2014, 2017  Simon Mika <simon@mika.se>
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

using Error = Kean.Error;

namespace SysPL.Exception
{
	public class Exception :
		Error.Exception
	{
		protected internal Exception(Error.Level level, string title, string message, params string[] arguments) :
			this(null, level, title, message, arguments)
		{
		}
		protected internal Exception(System.Exception innerException, Error.Level level, string title, string message, params string[] arguments) :
			base(innerException, level, title, message, arguments)
		{
		}
	}
}
