// Copyright (C) 2010  Simon Mika <simon@mika.se>
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

namespace Kean.Extension
{
	public static class ObjectExtension
	{
		public static bool NotNull(this object me)
		{
			return !object.ReferenceEquals(me, null);
		}
		public static bool IsNull(this object me)
		{
			return object.ReferenceEquals(me, null);
		}
		public static bool Same(this object me, object other)
		{
			return object.ReferenceEquals(me, other);
		}
		public static bool SameOrEquals(this object me, object other)
		{
			return object.ReferenceEquals(me, other) ||
				!object.ReferenceEquals(me, null) && me.Equals(other);
		}
		public static int Hash(this object me)
		{
			return object.ReferenceEquals(me, null) ? 0 : me.GetHashCode();
		}
		public static T ConvertType<T>(this object me)
		{
			return (T)Convert.ChangeType(me, typeof(T));
		}
		public static T As<T>(this object me, T @default)
		{
			return me is T ? (T)me : @default;
		}
		public static T As<T>(this object me)
		{
			return me.As(default(T));
		}
		public static string AsString(this object me)
		{
			string result;
			if (me.IsNull())
				result = null;
			else if (me is string)
				result = me as string;
			else if (me is char)
				result = me.ToString();
			else if (me is bool)
				result = (bool)me ? "true" : "false";
			else if (me is byte)
				result = ((byte)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is sbyte)
				result = ((sbyte)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is short)
				result = ((short)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is ushort)
				result = ((ushort)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is int)
				result = ((int)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is uint)
				result = ((uint)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is long)
				result = ((long)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is ulong)
				result = ((ulong)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is float)
				result = ((float)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is double)
				result = ((double)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is decimal)
				result = ((decimal)me).ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
			else if (me is DateTime)
				result = ((DateTime)me).ToString("o");
			else if (me is DateTimeOffset)
				result = ((DateTimeOffset)me).ToString("o");
			else if (me is TimeSpan)
				result = ((TimeSpan)me).ToString();
			else if (me is Enum)
				result = Enum.GetName(me.GetType(), me).ToLower();
			else
				result = me.ToString();
			return result;
		}
		public static byte[] AsBinary(this object me)
		{
			byte[] result;
			if (me.IsNull())
				result = null;
			else if (me is string)
				result = System.Text.Encoding.UTF8.GetBytes((string)me);
			else if (me is char)
				result = me.AsString().AsBinary();
			else if (me is bool)
				result = BitConverter.GetBytes((bool)me);
			else if (me is byte)
				result = BitConverter.GetBytes((byte)me);
			else if (me is sbyte)
				result = BitConverter.GetBytes((sbyte)me);
			else if (me is short)
				result = BitConverter.GetBytes((short)me);
			else if (me is ushort)
				result = BitConverter.GetBytes((ushort)me);
			else if (me is int)
				result = BitConverter.GetBytes((int)me);
			else if (me is uint)
				result = BitConverter.GetBytes((uint)me);
			else if (me is long)
				result = BitConverter.GetBytes((long)me);
			else if (me is ulong)
				result = BitConverter.GetBytes((ulong)me);
			else if (me is float)
				result = BitConverter.GetBytes((float)me);
			else if (me is double)
				result = BitConverter.GetBytes((double)me);
			else if (me is decimal)
			{
				int[] bits = decimal.GetBits((decimal)me);
				byte[] bytes0 = BitConverter.GetBytes(bits[0]);
				byte[] bytes1 = BitConverter.GetBytes(bits[1]);
				byte[] bytes2 = BitConverter.GetBytes(bits[2]);
				byte[] bytes3 = BitConverter.GetBytes(bits[3]);
				result = new byte[] { bytes0[0], bytes0[1], bytes0[2], bytes0[3], bytes1[0], bytes1[1], bytes1[2], bytes1[3], bytes2[0], bytes2[1], bytes2[2], bytes2[3], bytes3[0], bytes3[1], bytes3[2], bytes3[3] };
			}
			else if (me is DateTime)
				result = ((DateTime)me).Ticks.AsBinary();
			else if (me is DateTimeOffset)
			{
				result = new byte[16];
				Array.Copy(BitConverter.GetBytes(((DateTimeOffset)me).DateTime.Ticks), result, 8);
				Array.Copy(BitConverter.GetBytes(((DateTimeOffset)me).Offset.Ticks), 0, result, 8, 8);
			}
			else if (me is TimeSpan)
				result = ((TimeSpan)me).Ticks.AsBinary();
			else
				result = me.AsString().AsBinary();
			return result;
		}
	}
}
