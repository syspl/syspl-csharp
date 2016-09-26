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
	public class Endpoint :
			IEquatable<Endpoint>
	{
		public Domain Host { get; set; }
		public uint? Port { get; set; }
		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder(this.Host);
				if (this.Port.HasValue)
				{
					result.Append(":");
					result.Append(this.Port.Value.ToString());
				}
				return result.ToString();
			}
			set
			{
				if (value == null || value == "")
				{
					this.Host = null;
					this.Port = null;
				}
				else
				{
					string[] splitted = value.Split(new char[] { ':' }, 2);
					this.Host = splitted[0];
					uint port;
					if (splitted.Length > 1 && uint.TryParse(splitted[1], out port))
						this.Port = port;
					else
						this.Port = null;
				}
			}
		}
		#endregion
		public Endpoint()
		{ }
		public Endpoint(Domain host, uint? port) :
			this()
		{
			this.Host = host;
			this.Port = port;
		}
		public Endpoint Copy()
		{
			return new Endpoint(this.Host.IsNull() ? null : this.Host.Copy(), this.Port);
		}

		#region IEquatable<Endpoint> Members
		public bool Equals(Endpoint other)
		{
			return other.NotNull() && this.Host == other.Host && this.Port == other.Port;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Endpoint && this.Equals(other as Endpoint);
		}
		public override int GetHashCode()
		{
			return this.Host.Hash() ^ this.Port.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		#region Operators
		public static bool operator ==(Endpoint left, Endpoint right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Endpoint left, Endpoint right)
		{
			return !(left == right);
		}
		public static implicit operator string(Endpoint endpoint)
		{
			return endpoint.IsNull() ? null : endpoint.String;
		}
		public static implicit operator Endpoint(string endpoint)
		{
			return endpoint.IsEmpty() ? null : new Endpoint() { String = endpoint };
		}
		#endregion
	}
}
