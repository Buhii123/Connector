using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.Application.Lib
{
    public static class StringExtension
    {
        public static string RemoveWhitespace(this string str)
        {
            return string.Concat(str.Where(c => !Char.IsWhiteSpace(c)));
        }
    }
}
