namespace Kean.Extension
{
	public static class ArrayExtension
	{
		public static T Last<T>(this T[] me)
		{
			return me[me.Length - 1];
		}
	}
}
