using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector
{
    internal class RoutingApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            CreateUIApp.CreateUI<ConectorTab>(application);
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
