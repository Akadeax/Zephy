using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Server.utilities
{
    static class Util
    {
        /// <summary>
        /// Joins Strings in list with ',' in between
        /// </summary>
        public static string ToString<T>(this List<T> list)
        {
            return String.Join(",", list.ToArray());
        }

        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
