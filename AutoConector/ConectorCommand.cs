using AutoConector.TaskConecter;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector
{
    [Transaction(TransactionMode.Manual)]
    public class ConectorCommand : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ElementId levelId;
            ElementId DuctTypeId;
            Connector OriginConnectorIN = null;


            FamilyInstance element = (FamilyInstance)(doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, new MySelectionFilter())));


            List<MEPModel> anemos = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                                                       .OfCategory(BuiltInCategory.OST_DuctTerminal)
                                                       .WhereElementIsNotElementType()
                                                       .Cast<FamilyInstance>()
                                                       .ToList()
                                                       .Select(x => x.MEPModel)
                                                       .ToList();
            DuctType Dtype = new FilteredElementCollector(doc).OfClass(typeof(DuctType))
                                                       .OfCategory(BuiltInCategory.OST_DuctCurves)
                                                       .Cast<DuctType>()
                                                       .ToList()
                                                       .Where(d => d.Name == "Taps / Short Radius")
                                                       .FirstOrDefault();

            levelId = element.LevelId;
            DuctTypeId = Dtype.Id;


            List<Connector> ConnectersFASU = new List<Connector>();
            XYZ point = ((LocationPoint)(element.Location)).Point;
            MEPModel FASU = element.MEPModel;
            foreach (Connector c in FASU.ConnectorManager.Connectors)
            {
                OriginConnectorIN = c.AssignedFlowDirection == FlowDirectionType.In ? c : OriginConnectorIN;
                if (c.AssignedFlowDirection == FlowDirectionType.Out) ConnectersFASU.Add(c);
            }

            using (TransactionGroup tranGr = new TransactionGroup(doc, "Run"))
            {
                tranGr.Start();

                List<Connector> AirAbove;
                List<Connector> AirBelow;
                List<Connector> RedundantConAbove;
                List<Connector> RedundantConBelow;

                bool basic = true;
                if (OriginConnectorIN.SetDirection(doc, point).IsAlmostEqualTo(XYZ.BasisY.Negate()) || OriginConnectorIN.SetDirection(doc, point).IsAlmostEqualTo(XYZ.BasisY)) basic = false;

                Dictionary<Connector, Connector> Above = ListDirDuctAndAir.DictionaryAirToConnectorF(doc, anemos, ConnectersFASU, OriginConnectorIN, point, basic, out AirAbove, out RedundantConAbove);

                Above.Route(doc, ConnectersFASU, DuctTypeId, levelId, OriginConnectorIN, point, basic);

                Dictionary<Connector, Connector> Below = ListDirDuctAndAir.DictionaryAirToConnectorF(doc, anemos, ConnectersFASU, OriginConnectorIN, point, !basic, out AirBelow, out RedundantConBelow);
                Below.Route(doc, ConnectersFASU, DuctTypeId, levelId, OriginConnectorIN, point, basic);
                Dictionary<Connector, Connector> Excess = new Dictionary<Connector, Connector>();

                if (AirAbove.Count != 0)
                {
                    int count = Below.Count;
                    // AirAbove.Reverse();
                    // RedundantConBelow.Reverse();

                    if (Below.Count == 0) RedundantConAbove.Reverse();
                    for (int i = 0; i < AirAbove.Count; i++)
                    {

                        Excess.Add(AirAbove[i], RedundantConBelow[count]);


                        count++;
                    }
                }

                if (AirBelow.Count != 0)
                {
                    int count = Above.Count;
                    // AirBelow.Reverse();
                    if (Above.Count == 0) RedundantConAbove.Reverse();
                    //RedundantConAbove.Reverse();

                    for (int i = 0; i < AirBelow.Count; i++)
                    {
                        Excess.Add(AirBelow[i], RedundantConAbove[count]);
                        count++;
                    }
                }

                Excess.ExcessRoute(doc, DuctTypeId, levelId, point, basic);

                tranGr.Assimilate();

            }

            return Result.Succeeded;
        }

        public static string GetPath()
        {
            return typeof(ConectorCommand).Namespace + "." + nameof(ConectorCommand);
        }
    }
}
