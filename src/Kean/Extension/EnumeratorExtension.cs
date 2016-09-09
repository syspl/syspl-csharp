using Generic = System.Collections.Generic;
namespace Kean.Extension
{
	public static class EnumeratorExtension
	{
		static T[] ToArray<T>(this Generic.IEnumerator<T> me, int count)
		{
			T[] result;
			if (me.MoveNext())
			{
				var head = me.Current;
				result = me.ToArray(count + 1);
				result[count] = head;
			}
			else
				result = new T[count + 1];
			return result;
		}
		public static T[] ToArray<T>(this Generic.IEnumerator<T> me)
		{
			return me.ToArray(0);
		}
	}
}
