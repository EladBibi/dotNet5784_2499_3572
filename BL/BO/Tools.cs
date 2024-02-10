using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    internal static class Tools
    {
        public static string ToStringProperty<T>(this T obj)
        {


            StringBuilder str = new StringBuilder();
            foreach (PropertyInfo item in obj!.GetType().GetProperties())
            {
                str.AppendLine($"{item.Name}: {item.GetValue(obj, null)}");
            }

            return str.ToString();
        }

    }
}


