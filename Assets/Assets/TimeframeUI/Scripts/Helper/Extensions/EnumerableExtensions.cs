namespace Termway.Helper
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtension
    {
        /// <summary>
        /// Parse all enumerable element into a string. 
        /// </summary>
        public static string ToStr<T>(this IEnumerable<T> enumerable, string separator = ";")
        {
            return ToFlattenString(enumerable, separator);
        }

        /// <summary>
        /// Parse all enumerable element into a string. <see cref="ToStr"/> Same Same but Different (Name) but still Same.
        /// </summary>
        public static string ToFlattenString<T>(this IEnumerable<T> enumerable, string separator = ";")
        {
            string[] elements = enumerable.Select(e => e.ToString()).ToArray();
            return string.Join(separator, elements);
        }
    }
}
