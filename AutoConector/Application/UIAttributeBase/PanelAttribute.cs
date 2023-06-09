﻿using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.Application.UIAttributeBase
{
    public class PanelAttribute : Base.UIAttributeBase
    {
        private string _tabName = null;

        public PanelAttribute(string name) : base(name)
        {
        }

        public void SetData(string tabName)
        {
            _tabName = tabName;
        }

        public RibbonPanel CreateRibbonPanel(UIControlledApplication application)
        {
            var ribbonPanel = application.CreateRibbonPanel(_tabName, Name);
            return ribbonPanel;
        }
    }
}
