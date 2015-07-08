using System;
using System.Collections.Generic;

namespace TGameLibrary
{
    /// <remarks>
    /// Various extension methods
    /// </remarks>
    public static partial class MyExtensions
    {
        /// <summary>
        /// Compares each element in an array to see if they are all equal.
        /// </summary>
        /// <typeparam name="T">Type of array to test.</typeparam>
        /// <param name="arr">The array to test.</param>
        /// <returns><c>true</c> if all elements are equal, <c>false</c> otherwise.</returns>
        public static bool IsAllEqual<T>(this T[] arr)
            where T : IComparable<T>, IEnumerable<T>
        {
            foreach (T cell in arr)
            {
                if ( !cell.Equals(arr[0]) )
                {
                    return false;
                }
            }
            return true;
        }
    }
}
