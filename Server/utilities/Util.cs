using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Server.Util
{
    static class Util
    {
        private static readonly Random rand = new Random();

        /// <summary>
        /// Joins Strings in list with ',' in between
        /// </summary>
        public static string ToString<T>(this List<T> list)
        {
            return string.Join(",", list.ToArray());
        }


        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string PrettyJson(this string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);
            return JsonSerializer.Serialize(jsonElement, options);
        }

        public static int RandTimestamp()
        {
            int utcStart = new DateTime(2021, 1, 1).ToUnixTimestamp();
            int utcEnd = DateTime.UtcNow.ToUnixTimestamp();

            return rand.Next(utcStart, utcEnd);
        }

        public static int ToUnixTimestamp(this DateTime date)
        {
            return (int)(date - DateTime.UnixEpoch).TotalSeconds;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static bool HasDuplicates(List<string> list)
        {
            var hashset = new HashSet<string>();
            foreach (var name in list)
            {
                if (!hashset.Add(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
