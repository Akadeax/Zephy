using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Server.utilities
{
    class Util
    {
        public static string ConvertListToString<T>(List<T> list)
        {
            return String.Join(",", list.ToArray());
        }
    }
}
