using AutoConector.Application.Lib;
using AutoConector.Application.UIAttributeBase.Base;
using AutoConector.Asset;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AutoConector.Application.UIAttributeBase
{
    public class ButtonAttribute : Base.UIAttributeBase, IRibbonItem
    {
        public ButtonAttribute(string name, string className) : base(name)
        {
            ClassName = className;
        }

        private string ClassName { get; }

        public string Unique => Name.RemoveWhitespace();
        public string LinkImage { get; set; } = null;
        public string ToolTipImage { get; set; } = null;
        public string LongDescription { get; set; } = null;
        public string ToolTip { get; set; } = null;

        public string Assembly => CoreAssembly.GetAssemblyLocation();
        public PushButtonData CreatePushButtonData()
        {
            var buttonData = new PushButtonData(Unique, Name, Assembly, ClassName);

            if (!string.IsNullOrEmpty(LinkImage))
            {
                
                var bitmapImage = ResourceImage.GetIcon(LinkImage);
                buttonData.LargeImage = bitmapImage;
                buttonData.Image = bitmapImage;
            }

            if (!string.IsNullOrEmpty(ToolTipImage))
            {
              
                var bitmapImage = ResourceImage.GetIcon(LinkImage);
                buttonData.ToolTipImage = bitmapImage;
            }

            if (!string.IsNullOrEmpty(LongDescription))
            {
                buttonData.LongDescription = LongDescription;
            }

            if (!string.IsNullOrEmpty(ToolTip))
            {
                buttonData.ToolTip = ToolTip;
            }

            return buttonData;
        }

        //public void SetData(string assemblyName, string className)
        //{
        //    ClassName = className;
        //}
    }
}
