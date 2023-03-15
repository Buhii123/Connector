using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector
{
    public class Information
    {
        public static string ExecutingAssembly =>
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

        public static string ImagesFolder => Path.Combine(ExecutingAssembly, "Assets", "Images");
    }
}
