using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.Application.Lib
{
    public static class RibbonPanelExtension
    {
        public static IList<RibbonItem> AddStackedItems(this RibbonPanel ribbonPanel, List<PushButtonData> pushButtonDatas)
        {
            const int amount2PushButtonData = 2;
            const int amount3PushButtonData = 3;
            IList<RibbonItem> result = null;
            switch (pushButtonDatas.Count)
            {
                case amount2PushButtonData:
                    result = ribbonPanel.AddStackedItems(pushButtonDatas[0], pushButtonDatas[1]);
                    break;
                case amount3PushButtonData:
                    result = ribbonPanel.AddStackedItems(pushButtonDatas[0], pushButtonDatas[1], pushButtonDatas[2]);
                    break;
            }

            return result;
        }
    }
}
