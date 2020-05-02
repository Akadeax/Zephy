using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Server.utilities
{
    static class Util
    {
        public static string ToString<T>(this List<T> list)
        {
            return String.Join(",", list.ToArray());
        }
    }
}
