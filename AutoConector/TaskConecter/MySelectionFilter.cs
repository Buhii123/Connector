using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.TaskConecter
{
    public class MySelectionFilter : ISelectionFilter
    {

        public bool AllowElement(Element elem)
        {
            return (elem.Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_DuctAccessory));
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }

}
