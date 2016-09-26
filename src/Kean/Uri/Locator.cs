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
using Generic = System.Collections.Generic;

namespace Kean.Uri
{
	public class Locator :
		IEquatable<Locator>
	{
		public Scheme Scheme { get; set; }
		public Authority Authority { get; set; }
		Path path = new Path();
		public Path Path
		{
			get { return this.path; }
			set { this.path = value ?? new Path(); }
		}
		Query query = new Query();
		public Query Query
		{
			get { return this.query; }
			set { this.query = value ?? new Query(); }
		}
		public string Fragment { get; set; }
		public string PlatformPath
		{
			get { return this.Scheme == "file" ? (this.Authority.IsNull() ? "" : "\\\\" + ((string)this.Authority).ToUpper() + "\\") + this.Path.PlatformPath : ""; }
		}
		#region IString Members
		public string String
		{
			get
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder();
				if (this.Scheme.NotNull())
					result.AppendFormat("{0}://", this.Scheme);
				if (this.Authority.NotNull())
					result.Append(this.Authority);
				string path = this.Path.String;
				if (path.StartsWith(".") && result.Length > 0)
					result.Append("/");
				result.Append(path);
				if (!this.Query.Empty)
					result.AppendFormat("?{0}", this.Query);
				if (this.Fragment.NotNull())
					result.AppendFormat("#{0}", this.Fragment.PercentEncode(' ', '#'));
				return result.ToString();
			}
			set
			{
				if (value.IsEmpty())
				{
					this.Scheme = null;
					this.Authority = null;
					this.Path = null;
					this.Query = null;
					this.Fragment = null;
				}
				else
				{
					string[] splitted = value.Split(new string[] { "://" }, 2, StringSplitOptions.None);
					if (splitted.Length > 1) // has scheme
						this.Scheme = splitted[0];
					value = splitted.Last();
					if (!value.StartsWith("/") && !value.StartsWith("./") && !value.StartsWith("../"))
					{
						splitted = value.Split(new char[] { '/', '#', '?' }, 2);
						this.Authority = splitted[0];
						if (splitted.Length > 1)
							value = value.Substring(splitted[0].Length);
						else
							value = null;
					}
					if (value.NotEmpty())
					{
						if (value.NotEmpty())
						{
							if (value.StartsWith("#"))
							{
								this.Fragment = value.TrimStart('#');
								value = null;
							}
							else
							{
								splitted = value.Split(new char[] { '#' }, 2, StringSplitOptions.RemoveEmptyEntries);
								if (splitted.Length > 1)
								{
									this.Fragment = splitted[1].PercentDecode();
									value = splitted[0];
								}
							}
							if (value.NotEmpty())
							{
								if (value.StartsWith("?"))
									this.Query = value.TrimStart('?');
								else
								{
									splitted = splitted[0].Split(new char[] { '?' }, 2, StringSplitOptions.RemoveEmptyEntries);
									if (splitted.Length > 1)
										this.Query = splitted[1];
									this.Path = splitted[0];
								}
							}
						}
					}
				}
			}
		}
		#endregion
		public Locator()
		{
		}
		public Locator(Scheme scheme, Path path) : this(scheme, null, path)
		{
		}
		public Locator(Scheme scheme, Authority authority, Path path) : this(scheme, authority, path, null, null)
		{
		}
		public Locator(Scheme scheme, Authority authority, Path path, Query query) : this(scheme, authority, path, query, null)
		{
		}
		public Locator(Scheme scheme, Authority authority, Path path, Query query, string fragment) :
			this()
		{
			this.Scheme = scheme;
			this.Authority = authority;
			this.Path = path;
			this.Query = query;
			this.Fragment = fragment;
		}
		public Locator Copy()
		{
			return new Locator(this.Scheme.IsNull() ? null : this.Scheme.Copy(), this.Authority.IsNull() ? null : this.Authority.Copy(), this.Path.IsNull() ? null : this.Path.Copy(), this.Query.IsNull() ? null : this.Query.Copy(), this.Fragment);
		}
		public Locator Resolve(Locator absolute)
		{
			Locator result;
			if (this.Scheme.NotNull())
				result = this;
			else if (this.Authority.NotNull())
			{
				if (this.Authority == ".")
					result = new Locator(absolute.Scheme, absolute.Authority, absolute.Path.Folder + this.Path, this.Query, this.Fragment);
				else
					result = new Locator(absolute.Scheme, this.Authority, this.Path, this.Query, this.Fragment);
			}
			else if (!this.Path.Empty)
			{
				result = new Locator(absolute.Scheme, absolute.Authority, this.Path.Resolve(absolute.Path), this.Query, this.Fragment);
			}
			else if (this.Query.NotNull() && !this.Query.Empty)
				result = new Locator(absolute.Scheme, absolute.Authority, absolute.Path, this.Query, this.Fragment);
			else if (this.Fragment.NotNull())
				result = new Locator(absolute.Scheme, absolute.Authority, absolute.Path, absolute.Query, this.Fragment);
			else
				result = absolute;
			return result;
		}
		public Locator Relative(Locator locator)
		{
			Locator result = this.Copy();
			if (locator.NotNull() && result.Scheme == locator.Scheme)
			{
				result.Scheme = null;
				if (result.Authority == locator.Authority)
				{
					result.Authority = null;
					if (result.Path == locator.Path)
					{
						result.Path = null;
						if (result.Query == locator.Query)
						{
							result.Query = null;
							if (result.Fragment == locator.Fragment)
								result.Fragment = null;
						}
					}
				}
			}
			return result;
		}
		#region IEquatable<Locator> Members
		public bool Equals(Locator other)
		{
			return other.NotNull() &&
			this.Scheme == other.Scheme &&
			this.Authority == other.Authority &&
			this.Path == other.Path &&
			this.Query == other.Query &&
			this.Fragment == other.Fragment;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Locator && this.Equals(other as Locator);
		}
		public override int GetHashCode()
		{
			return this.Scheme.Hash() ^ this.Authority.Hash() ^ this.Path.Hash() ^ this.Query.Hash() ^ this.Fragment.Hash();
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		public static Locator FromPlatformPath(string path)
		{
			string authority = null;
			if (path.NotEmpty() && path.StartsWith("\\\\"))
			{
				path = path.Substring(2);
				string[] splitted = path.Split(new char [] {
					System.IO.Path.DirectorySeparatorChar,
					System.IO.Path.AltDirectorySeparatorChar
				}, 2);
				authority = splitted[0].ToLower();
				path = splitted.Length > 1 ? splitted[1] : "";
			}
			return path.NotEmpty() ? new Locator("file", authority, Path.FromPlatformPath(path)) : null;
		}
		public static Locator FromPlatformPath(string path, params string[] folders)
		{
			Locator result = Locator.FromPlatformPath(path);
			if (result.NotNull())
				foreach (string folder in folders)
					result.Path.Add(folder);
			return result;
		}
		public static Locator FromRelativePlatformPath(string path, params string[] folders)
		{
			return path.NotEmpty() ? Locator.FromPlatformPath(System.IO.Path.GetFullPath(path), folders) : null;
		}
		public static implicit operator Locator(System.IO.DirectoryInfo directory)
		{
			return directory.NotNull() ? new Locator("file", directory) : null;
		}
		#region Operators
		public static bool operator ==(Locator left, Locator right)
		{
			return left.Same(right) || (left.NotNull() && left.Equals(right));
		}
		public static bool operator !=(Locator left, Locator right)
		{
			return !(left == right);
		}
		public static implicit operator string(Locator locator)
		{
			return locator.IsNull() ? null : locator.String;
		}
		public static implicit operator Locator(string locator)
		{
			return locator.NotEmpty() ? new Locator() { String = locator } : null;
		}
		#region Add Operators
		public static Locator operator +(Locator left, Path right)
		{
			if (left.NotNull())
			{
				left = left.Copy();
				left.Path += right;
			}
			return left;
		}
		public static Locator operator +(Locator left, Generic.IEnumerable<string> right)
		{
			left = left.Copy();
			foreach (var item in right)
				left.Path += item;
			return left;
		}
		public static Locator operator +(Locator left, KeyValue<string, string> right)
		{
			left = left.Copy();
			left.Query[right.Key] = right.Value;
			return left;
		}
		#endregion
		#endregion
	}
}
