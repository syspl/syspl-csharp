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
	public class Domain :
	Collection.ILink<Domain, string>,
			IEquatable<Domain>
	{
		#region ILink<Domain, string> Members
		public string Head { get; set; }
		public Domain Tail { get; set; }
		#endregion
		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder(this.Head);
				if (this.Tail != null)
				{
					result.Append(".");
					result.Append(this.Tail.String);
				}
				return result.ToString();
			}
			set
			{
				if (value == null || value == "")
				{
					this.Head = null;
					this.Tail = null;
				}
				else
				{
					string[] splitted = value.Split(new char[] { '.' }, 2);
					this.Head = splitted[0];
					if (splitted.Length > 1)
						this.Tail = new Domain() { String = splitted[1] };
					else
						this.Tail = null;
				}
			}
		}
		#endregion
		public Domain()
		{ }
		public Domain(string head, Domain tail) :
			this()
		{
			this.Head = head;
			this.Tail = tail;
		}
		public Domain Copy()
		{
			return new Domain(this.Head, this.Tail.IsNull() ? null : this.Tail.Copy());
		}
		#region IEquatable<Domain> Members
		public bool Equals(Domain other)
		{
			return other.NotNull() && this.Head == other.Head && this.Tail == other.Tail;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Domain && this.Equals(other as Domain);
		}
		public override int GetHashCode()
		{
			return this.Head.Hash() ^ this.Tail.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		#region Operators
		public static bool operator ==(Domain left, Domain right)
		{
			return object.ReferenceEquals(left, right) || (!object.ReferenceEquals(left, null) && left.Equals(right));
		}
		public static bool operator !=(Domain left, Domain right)
		{
			return !(left == right);
		}
		public static implicit operator string(Domain domain)
		{
			return domain.IsNull() ? "" : domain.String;
		}
		public static implicit operator Domain(string domain)
		{
			return domain.IsEmpty() ? null : new Domain() { String = domain };
		}
		#endregion
	}
}
