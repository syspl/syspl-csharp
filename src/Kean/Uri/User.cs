// Copyright (C) 2010-2011  Simon Mika <simon@mika.se>
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

namespace Kean.Uri
{
	public class User :
		IEquatable<User>
	{
		public string Name { get; set; }
		public string Password { get; set; }
		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder(this.Name.PercentEncode());
				if (this.Password.NotNull())
				{
					result.Append(":");
					result.Append(this.Password.PercentEncode());
				}
				return result.ToString();
			}
			set
			{
				if (value.IsEmpty())
				{
					this.Name = null;
					this.Password = null;
				}
				else
				{
					string[] splitted = value.Split(new char[] { ':' }, 2);
					this.Name = splitted[0].PercentDecode();
					this.Password = splitted.Length > 1 ? splitted[1].PercentDecode() : null;
				}
			}
		}
		#endregion
		public User()
		{
		}
		public User(string name, string password) :
			this()
		{
			this.Name = name;
			this.Password = password;
		}
		public User Copy()
		{
			return new User(this.Name, this.Password);
		}
		#region IEquatable<User> Members
		public bool Equals(User other)
		{
			return other.NotNull() && this.Name == other.Name && this.Password == other.Password;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is User && this.Equals(other as User);
		}
		public override int GetHashCode()
		{
			return this.Name.Hash() ^ this.Password.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		#region Operators
		public static bool operator ==(User left, User right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(User left, User right)
		{
			return !(left == right);
		}
		public static implicit operator string(User user)
		{
			return user.IsNull() ? null : user.String;
		}
		public static implicit operator User(string user)
		{
			return user.IsEmpty() ? null : new User() { String = user };
		}
		#endregion
	}
}
