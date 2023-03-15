using AutoConector.Application.Lib;
using AutoConector.Application.UIAttributeBase;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector
{
    public class CreateUIApp
    {
        public static void CreateUI<T>(UIControlledApplication application) where T : class
        {
            var buhiiTab = typeof(T).GetCustomAttributes<TabAttribute>(false).FirstOrDefault();
            if (buhiiTab == null) return;

            application.CreateRibbonTab(buhiiTab.Name);

            var buhiiPanels = typeof(T).GetClassTypes();

            foreach (var buhiiPanel in buhiiPanels)
            {
                var atrbuhiiPanel = buhiiPanel.GetCustomAttributes<PanelAttribute>(false).FirstOrDefault();

                atrbuhiiPanel.SetData(buhiiTab.Name);
                var ribbonPanel = atrbuhiiPanel.CreateRibbonPanel(application);

                List<Type> items = buhiiPanel.GetClassTypes();
                foreach (var item in items)
                {
                    object atrItem = item.GetCustomAttributes(false).FirstOrDefault();
                    switch (atrItem)
                    {
                        case StackedButtonAttribute stacked:
                            var classButtonTypes = item.GetClassTypes();
                            var pushButtonDatas = new List<PushButtonData>();
                            foreach (var classButtonType in classButtonTypes)
                            {
                                var buttonAttribute = classButtonType.GetCustomAttributes<ButtonAttribute>(false)
                                    .FirstOrDefault();
                                pushButtonDatas.Add(buttonAttribute.CreatePushButtonData());
                            }

                            ribbonPanel.AddStackedItems(pushButtonDatas);
                            break;
                        case PulldownButtonDataAttribute pulldownButtonDataAttribute:
                            var classButtonTypes1 = item.GetClassTypes();
                            var pulldownButtonData = pulldownButtonDataAttribute.CreatePulldownButtonData();
                            var pulldownButton = (PulldownButton)ribbonPanel.AddItem(pulldownButtonData);
                            foreach (var classButtonType in classButtonTypes1)
                            {
                                var buttonAttribute = classButtonType.GetCustomAttributes<ButtonAttribute>(false)
                                    .FirstOrDefault();

                                pulldownButton.AddPushButton(buttonAttribute.CreatePushButtonData());
                            }

                            break;
                        case ButtonAttribute buttonAttribute:
                            var pushButtonData = buttonAttribute.CreatePushButtonData();
                            ribbonPanel.AddItem(pushButtonData);
                            break;
                    }
                }
            }
        }
    }
}
