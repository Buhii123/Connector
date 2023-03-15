using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.Asset
{
    public static class ResourceAssembly
    {
        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
        public static string GetNamespace()
        {
            return typeof(ResourceAssembly).Namespace + ".";
        }
    }
}
