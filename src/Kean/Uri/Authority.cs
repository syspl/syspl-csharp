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
	public class Authority :
		IEquatable<Authority>
	{
		public User User { get; set; }
		public Endpoint Endpoint { get; set; }
		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder();
				if (this.User.NotNull())
				{
					result.Append(this.User);
					result.Append("@");
				}
				if (this.Endpoint.NotNull())
					result.Append(this.Endpoint);
				return result.ToString();
			}
			set
			{
				if (value.IsEmpty())
				{
					this.User = null;
					this.Endpoint = null;
				}
				else
				{
					string[] splitted = value.Split(new char[] { '@' }, 2);
					if (splitted.Length > 1)
					{
						this.User = splitted[0];
						this.Endpoint = splitted[1];
					}
					else
					{
						this.User = null;
						this.Endpoint = splitted[0];
					}
				}
			}
		}
		#endregion
		#region IEquatable<Endpoint> Members
		public bool Equals(Authority other)
		{
			return !object.ReferenceEquals(other, null) && this.User == other.User && this.Endpoint == other.Endpoint;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Authority && this.Equals(other as Authority);
		}
		public override int GetHashCode()
		{
			return this.User.Hash() ^ this.Endpoint.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		public Authority()
		{
		}
		public Authority(User user, Endpoint endpoint) :
			this()
		{
			this.User = user;
			this.Endpoint = endpoint;
		}
		public Authority Copy()
		{
			return new Authority(this.User.IsNull() ? null : this.User.Copy(), this.Endpoint.IsNull() ? null : this.Endpoint.Copy());
		}
		#region Operators
		public static bool operator ==(Authority left, Authority right)
		{
			return object.ReferenceEquals(left, right) || (!object.ReferenceEquals(left, null) && left.Equals(right));
		}
		public static bool operator !=(Authority left, Authority right)
		{
			return !(left == right);
		}
		public static implicit operator string(Authority authority)
		{
			return authority.IsNull() ? null : authority.String;
		}
		public static implicit operator Authority(string authority)
		{
			return authority.IsEmpty() ? null : new Authority() { String = authority };
		}
		#endregion
	}
}
