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

using Globalization = System.Globalization;
using Generic = System.Collections.Generic;
using RegularExpressions = System.Text.RegularExpressions;

namespace Kean.Extension
{
	public static class StringExtension
	{
		public static bool NotEmpty(this string me)
		{
			return me.NotNull() && me != "";
		}
		public static bool IsEmpty(this string me)
		{
			return me.IsNull() || me == "";
		}
		public static bool Like(this string me, string pattern)
		{
			return new RegularExpressions.Regex("^" + RegularExpressions.Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegularExpressions.RegexOptions.IgnoreCase | RegularExpressions.RegexOptions.Singleline).IsMatch(me);
		}
		public static string Format(this string me, object argument)
		{
			return string.Format(me, argument);
		}
		public static string Format(this string me, object argument0, object argument1)
		{
			return string.Format(me, argument0, argument1);
		}
		public static string Format(this string me, object argument0, object argument1, object argument2)
		{
			return string.Format(me, argument0, argument1, argument2);
		}
		public static string Format(this string me, params object[] arguments)
		{
			return string.Format(me, arguments);
		}
		public static int Index(this string me, string needle)
		{
			return me.Index(0, needle);
		}
		public static int Index(this string me, int start, string needle)
		{
			return me.Index(start, me.Length - 1, needle);
		}
		public static int Index(this string me, int start, int end, string needle)
		{
			int length = me.Length - 1;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me[i] == needle[0] && me.Substring(i, needle.Length) == needle)
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index(this string me, char needle)
		{
			return me.Index(0, needle);
		}
		public static int Index(this string me, int start, char needle)
		{
			return me.Index(start, me.Length - 1, needle);
		}
		public static int Index(this string me, int start, int end, char needle)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me[i] == needle)
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index(this string me, params char[] needles)
		{
			return me.Index(0, needles);
		}
		public static int Index(this string me, int start, params char[] needles)
		{
			return me.Index(start, me.Length - 1, needles);
		}
		public static int Index(this string me, int start, int end, params char[] needles)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (needles.Contains(me[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		public static string Get(this string me, int index)
		{
			return index >= 0 ?
				me.Get(index, me.Length - index) :
				me.Get(me.Length + index, -index);
		}
		public static string Get(this string me, int index, int length)
		{
			string result = "";
			if (length != 0)
			{
				int count = me.Length;
				int sliceIndex = (index >= 0) ? index : count + index;
				int sliceLength = (length > 0) ? length : count + length - sliceIndex;
				result = me.Substring(sliceIndex, sliceLength);
			}
			return result;
		}
		public static string PercentEncode(this string me, params char[] characters)
		{
			if (characters.Empty())
				characters = new char[] {
					' ',
					'"',
					'!',
					'#',
					'$',
					'%',
					'&',
					'\'',
					'(',
					')',
					'*',
					'+',
					',',
					'/',
					':',
					';',
					'=',
					'?',
					'@',
					'[',
					']'
				};
			Text.Builder result = null;
			if (me.NotNull())
				foreach (char c in me)
					if (characters.Contains(c))
						result += "%" + System.Text.Encoding.ASCII.GetBytes(new char[] { c }).First().ToString("X2");
					else
						result += c;
			return result;
		}
		public static string PercentDecode(this string me)
		{
			Text.Builder result = null;
			if (me.NotNull())
			{
				Generic.IEnumerator<char> enumerator = me.GetEnumerator();
				while (enumerator.MoveNext())
					if (enumerator.Current == '%')
					{
						char[] characters = new char[] { enumerator.Next(), enumerator.Next() };
						byte value;
						if (byte.TryParse(new string(characters), System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.InvariantInfo, out value))
							result += System.Text.Encoding.ASCII.GetChars(new byte[] { value });
						else
							result += "%" + characters;
					}
					else
						result += enumerator.Current;
			}
			return result;
		}
		public static Generic.IEnumerator<char> GetEnumerator(this string me)
		{
			for (int i = 0; i < me.Length; i++)
				yield return me[i];
		}
		public static int Parse(this string me, int @default) {
			int result;
			return int.TryParse(me, Globalization.NumberStyles.Any, Globalization.NumberFormatInfo.InvariantInfo, out result) ? result : @default;
		}
		public static float Parse(this string me, float @default) {
			float result;
			return float.TryParse(me, Globalization.NumberStyles.Any, Globalization.NumberFormatInfo.InvariantInfo, out result) ? result : @default;
		}
		public static double Parse(this string me, double @default) {
			double result;
			return double.TryParse(me, Globalization.NumberStyles.Any, Globalization.NumberFormatInfo.InvariantInfo, out result) ? result : @default;
		}
		public static bool Parse(this string me, bool @default)
		{
			bool result;
			return bool.TryParse(me, out result) ? result : @default;
		}
	}
}
