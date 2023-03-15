using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.Application.UIAttributeBase.Base
{
    public interface IRibbonItem
    {
        string Unique { get; }
        string LinkImage { get; set; }
        string ToolTipImage { get; set; }
        string LongDescription { get; set; }
        string ToolTip { get; set; }
        string Assembly { get; }
        
    }
}
