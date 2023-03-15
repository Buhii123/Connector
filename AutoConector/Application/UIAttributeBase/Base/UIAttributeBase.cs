using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.Application.UIAttributeBase.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIAttributeBase : Attribute
    {
        public UIAttributeBase(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}
