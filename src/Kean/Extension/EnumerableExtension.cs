using Generic = System.Collections.Generic;
namespace Kean.Extension
{
	public static class EnumerableExtension
	{
		public static T[] ToArray<T>(this Generic.IEnumerable<T> me)
		{
			return me.GetEnumerator().ToArray();
		}
	}
}
