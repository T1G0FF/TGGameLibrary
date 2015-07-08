using System;

namespace TGameLibrary
{
	/// <remarks>
	/// Various extension methods
	/// </remarks>
    public static partial class MyExtensions
	{	
		/// <summary>
		/// Fills the elements of an array with a given value.
		/// </summary>
		/// <typeparam name="T">Type of array to fill.</typeparam>
		/// <param name="arr">The array to fill.</param>
		/// <param name="value">The value to fill the array with.</param>
		/// <returns>An array filled with a given value.</returns>
		public static T[] Populate<T>(this T[] arr, T value)
		{	for ( int i = 0; i < arr.Length; ++i )
			{	arr[i] = value;
			}
			return arr;
		}
	}
}
