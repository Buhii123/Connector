using AutoConector.Application.UIAttributeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector
{
    [Tab("ConnectorTab")]
    public class ConectorTab
    {
        [Panel("Pannel Test")]
        public class ConnectorTab 
        {
            
            [Button("Connector", "AutoConector.ConectorCommand", LinkImage = "RerouteAll.png", LongDescription = "This is Test")]
            public class ConnectorButton
            {
            }
        }
    }
}
