using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.TaskConecter
{
    public static class RouteDuct
    {
        public static void Route1(this Connector conAir, Document doc, Connector conF, ElementId ductTypeId, ElementId levelId, bool b = true)
        {

            using (Transaction tran1 = new Transaction(doc, "Create Duct Route 1"))
            {
                tran1.Start();
                Duct d1 = Duct.Create(doc, ductTypeId, levelId, conAir, conAir.SetPointZ(conF.Origin));
                Duct d2 = Duct.Create(doc, ductTypeId, levelId, conF, conF.SetPointT(conAir, b));
                Duct d3 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), d2.GetconnectorDuct(1));
                ConnectorRoute1(doc, d1, d2, d3);
                tran1.Commit();
            }
        }
        public static void Route2(this Connector conAir, Document doc, Connector conF, ElementId ductTypeId, ElementId levelId, bool b = true)
        {
            using (Transaction tran1 = new Transaction(doc, "Create Duct Route 2"))
            {
                tran1.Start();
                Duct d1 = Duct.Create(doc, ductTypeId, levelId, conAir, conAir.SetPointZ(conF.Origin));
                Duct d2 = Duct.Create(doc, ductTypeId, levelId, conF, conF.SetPointT(conAir, b));
                ConnectorRoute2(doc, d1, d2);
                tran1.Commit();
            }
        }
        public static void Route3(this Connector conAir, Document doc, Connector conF, ElementId ductTypeId, ElementId levelId, bool b, int i)
        {

            using (Transaction tran1 = new Transaction(doc, "Create Duct Route 3"))
            {
                tran1.Start();
                if (b)
                {
                    XYZ startPoint = new XYZ(conAir.Origin.X, conAir.Origin.Y, conF.Origin.Z);
                    XYZ endPoint = new XYZ(conAir.Origin.X, conF.Origin.Y, conF.Origin.Z);
                    XYZ newPoint = new XYZ(((i - 1) * startPoint.X + endPoint.X) / (i), ((i - 1) * startPoint.Y + endPoint.Y) / (i), ((i - 1) * startPoint.Z + endPoint.Z) / (i));
                    Duct d1 = Duct.Create(doc, ductTypeId, levelId, conAir, conAir.SetPointZ(conF.Origin));
                    Duct d2 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), newPoint);
                    Duct d4 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X, d2.GetconnectorDuct(1).Origin.Y, conF.Origin.Z));
                    Duct d3 = Duct.Create(doc, ductTypeId, levelId, d2.GetconnectorDuct(1), new XYZ(d4.GetconnectorDuct(1).Origin.X, d4.GetconnectorDuct(1).Origin.Y, d4.GetconnectorDuct(1).Origin.Z));
                    ConnectorRoute3(doc, d1, d2, d3, d4);

                }
                if (!b)
                {
                    XYZ startPoint = new XYZ(conAir.Origin.X, conAir.Origin.Y, conF.Origin.Z);
                    XYZ endPoint = new XYZ(conF.Origin.X, conAir.Origin.Y, conF.Origin.Z);
                    XYZ newPoint = new XYZ(((i - 1) * startPoint.X + endPoint.X) / (i), ((i - 1) * startPoint.Y + endPoint.Y) / (i), ((i - 1) * startPoint.Z + endPoint.Z) / (i));
                    Duct d1 = Duct.Create(doc, ductTypeId, levelId, conAir, conAir.SetPointZ(conF.Origin));
                    Duct d2 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), newPoint);
                    Duct d4 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(d2.GetconnectorDuct(1).Origin.X, conF.Origin.Y, conF.Origin.Z));
                    Duct d3 = Duct.Create(doc, ductTypeId, levelId, d2.GetconnectorDuct(1), new XYZ(d4.GetconnectorDuct(1).Origin.X, d4.GetconnectorDuct(1).Origin.Y, d4.GetconnectorDuct(1).Origin.Z));
                    ConnectorRoute3(doc, d1, d2, d3, d4);
                }
                tran1.Commit();
            }
        }
        public static void Route4(this Connector conAir, Document doc, Connector conF, ElementId ductTypeId, ElementId levelId, XYZ point, bool b, int i)
        {

            using (Transaction tran1 = new Transaction(doc, "Create Duct Route 4"))
            {
                tran1.Start();
                if (b)
                {
                    Duct d1 = Duct.Create(doc, ductTypeId, levelId, conAir, conAir.SetPointZ(conF.Origin));
                    Duct d2 = null;
                    if (conF.Origin.Y < point.Y) d2 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X, conF.Origin.Y - i, conF.Origin.Z));
                    else d2 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X, conF.Origin.Y + i, conF.Origin.Z));
                    Duct d3 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), new XYZ(d1.GetconnectorDuct(1).Origin.X, d2.GetconnectorDuct(1).Origin.Y, conF.Origin.Z));
                    Duct d4 = Duct.Create(doc, ductTypeId, levelId, d3.GetconnectorDuct(1), d2.GetconnectorDuct(1));
                    ConnectorRoute4(doc, d1, d2, d3, d4);
                }
                if (!b)
                {
                    Duct d1 = Duct.Create(doc, ductTypeId, levelId, conAir, conAir.SetPointZ(conF.Origin));
                    Duct d2 = null;
                    if (conF.Origin.X < point.X) d2 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X - i, conF.Origin.Y, conF.Origin.Z));
                    else d2 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X + i, conF.Origin.Y, conF.Origin.Z));
                    Duct d3 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), new XYZ(d2.GetconnectorDuct(1).Origin.X, d1.GetconnectorDuct(1).Origin.Y, conF.Origin.Z));
                    Duct d4 = Duct.Create(doc, ductTypeId, levelId, d3.GetconnectorDuct(1), d2.GetconnectorDuct(1));
                    ConnectorRoute4(doc, d1, d2, d3, d4);
                }

                tran1.Commit();

            }

        }
        public static void Route5(this Connector conAir, Document doc, Connector conF, ElementId ductTypeId, ElementId levelId, XYZ point, bool b, int i)
        {
            if (b)
            {
                using (Transaction tr = new Transaction(doc, "Create Duct Route 5"))
                {
                    Duct d1, d2, d3, d4, d5, d6;
                    tr.Start();
                    if (point.Y > conAir.Origin.Y) d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X, conF.Origin.Y - 0.8, conF.Origin.Z));

                    else d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X, conF.Origin.Y + 0.8, conF.Origin.Z));

                    d2 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), new XYZ(d1.GetconnectorDuct(1).Origin.X, d1.GetconnectorDuct(1).Origin.Y, d1.GetconnectorDuct(1).Origin.Z - 1));
                    d3 = Duct.Create(doc, ductTypeId, levelId, d2.GetconnectorDuct(1), new XYZ(d2.GetconnectorDuct(1).Origin.X + 2.5, d2.GetconnectorDuct(1).Origin.Y, d2.GetconnectorDuct(1).Origin.Z));
                    d4 = Duct.Create(doc, ductTypeId, levelId, d3.GetconnectorDuct(1), new XYZ(d3.GetconnectorDuct(1).Origin.X, conAir.Origin.Y, d3.GetconnectorDuct(1).Origin.Z));
                    d5 = Duct.Create(doc, ductTypeId, levelId, d4.GetconnectorDuct(1), new XYZ(conAir.Origin.X, conAir.Origin.Y, d4.GetconnectorDuct(1).Origin.Z));
                    d6 = Duct.Create(doc, ductTypeId, levelId, conAir, d5.GetconnectorDuct(1));
                    ConnectorRoute5(doc, d1, d2, d3, d4, d5, d6);
                    tr.Commit();
                }
            }
            if (!b)
            {
                using (Transaction tr = new Transaction(doc, "Create Duct Route 5"))
                {
                    Duct d1, d2, d3, d4, d5, d6;
                    tr.Start();
                    if (point.X > conAir.Origin.X) d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X - 0.8, conF.Origin.Y, conF.Origin.Z));

                    else d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X + 0.8, conF.Origin.Y, conF.Origin.Z));

                    d2 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), new XYZ(d1.GetconnectorDuct(1).Origin.X, d1.GetconnectorDuct(1).Origin.Y, d1.GetconnectorDuct(1).Origin.Z - 1));
                    d3 = Duct.Create(doc, ductTypeId, levelId, d2.GetconnectorDuct(1), new XYZ(d2.GetconnectorDuct(1).Origin.X, d2.GetconnectorDuct(1).Origin.Y + 2.5, d2.GetconnectorDuct(1).Origin.Z));
                    d4 = Duct.Create(doc, ductTypeId, levelId, d3.GetconnectorDuct(1), new XYZ(conAir.Origin.X, d3.GetconnectorDuct(1).Origin.Y, d3.GetconnectorDuct(1).Origin.Z));
                    d5 = Duct.Create(doc, ductTypeId, levelId, d4.GetconnectorDuct(1), new XYZ(conAir.Origin.X, conAir.Origin.Y, d4.GetconnectorDuct(1).Origin.Z));
                    d6 = Duct.Create(doc, ductTypeId, levelId, conAir, d5.GetconnectorDuct(1));
                    ConnectorRoute5(doc, d1, d2, d3, d4, d5, d6);
                    tr.Commit();
                }
            }

        }
        public static void Route6(this Connector conAir, Document doc, Connector conF, ElementId ductTypeId, ElementId levelId, Connector OriginIN, XYZ point, bool b, int i)
        {
            if (b == true)
            {
                using (Transaction tr = new Transaction(doc, "Create Duct Route 6"))
                {
                    Duct d1, d2, d3, d4, d5;
                    tr.Start();
                    if (point.Y > conAir.Origin.Y) d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X, conF.Origin.Y - 0.8, conF.Origin.Z));
                    else d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X, conF.Origin.Y + 0.8, conF.Origin.Z));
                    d2 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), new XYZ((d1.GetconnectorDuct(1).Origin.X - 2), d1.GetconnectorDuct(1).Origin.Y, d1.GetconnectorDuct(1).Origin.Z));
                    d3 = Duct.Create(doc, ductTypeId, levelId, d2.GetconnectorDuct(1), new XYZ(d2.GetconnectorDuct(1).Origin.X, conAir.Origin.Y, conF.Origin.Z));
                    d4 = Duct.Create(doc, ductTypeId, levelId, d3.GetconnectorDuct(1), new XYZ(conAir.Origin.X, conAir.Origin.Y, conF.Origin.Z));
                    d5 = Duct.Create(doc, ductTypeId, levelId, d4.GetconnectorDuct(1), conAir);
                    ConnectorRoute6(doc, d1, d2, d3, d4, d5);

                    tr.Commit();
                }
            }
            if (b == false)
            {
                using (Transaction tr = new Transaction(doc, "Create Duct Route 6"))
                {
                    Duct d1, d2, d3, d4, d5;
                    tr.Start();
                    if (point.X > conAir.Origin.X) d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X - 1, conF.Origin.Y, conF.Origin.Z));
                    else d1 = Duct.Create(doc, ductTypeId, levelId, conF, new XYZ(conF.Origin.X + 1, conF.Origin.Y, conF.Origin.Z));
                    d2 = Duct.Create(doc, ductTypeId, levelId, d1.GetconnectorDuct(1), new XYZ((d1.GetconnectorDuct(1).Origin.X), d1.GetconnectorDuct(1).Origin.Y + 2, d1.GetconnectorDuct(1).Origin.Z));
                    d3 = Duct.Create(doc, ductTypeId, levelId, d2.GetconnectorDuct(1), new XYZ(conAir.Origin.X, d2.GetconnectorDuct(1).Origin.Y, conF.Origin.Z));
                    d4 = Duct.Create(doc, ductTypeId, levelId, d3.GetconnectorDuct(1), new XYZ(conAir.Origin.X, conAir.Origin.Y, conF.Origin.Z));
                    d5 = Duct.Create(doc, ductTypeId, levelId, d4.GetconnectorDuct(1), conAir);
                    ConnectorRoute6(doc, d1, d2, d3, d4, d5);

                    tr.Commit();
                }
            }

        }

        private static void ConnectorRoute1(Document doc, Duct d1, Duct d2, Duct d3)
        {
            doc.Create.NewElbowFitting(d2.GetconnectorDuct(1), d3.GetconnectorDuct(1));
            doc.Create.NewElbowFitting(d1.GetconnectorDuct(1), d3.GetconnectorDuct(0));
        }
        private static void ConnectorRoute2(Document doc, Duct d1, Duct d2)
        {
            doc.Create.NewElbowFitting(d1.GetconnectorDuct(1), d2.GetconnectorDuct(1));
        }
        private static void ConnectorRoute3(Document doc, Duct d1, Duct d2, Duct d3, Duct d4)
        {
            doc.Create.NewElbowFitting(d1.GetconnectorDuct(1), d2.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d2.GetconnectorDuct(1), d3.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d4.GetconnectorDuct(1), d3.GetconnectorDuct(1));
        }
        private static void ConnectorRoute4(Document doc, Duct d1, Duct d2, Duct d3, Duct d4)
        {
            doc.Create.NewElbowFitting(d1.GetconnectorDuct(1), d3.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d3.GetconnectorDuct(1), d4.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d4.GetconnectorDuct(1), d2.GetconnectorDuct(1));
        }
        private static void ConnectorRoute5(Document doc, Duct d1, Duct d2, Duct d3, Duct d4, Duct d5, Duct d6)
        {
            doc.Create.NewElbowFitting(d1.GetconnectorDuct(1), d2.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d2.GetconnectorDuct(1), d3.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d3.GetconnectorDuct(1), d4.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d4.GetconnectorDuct(1), d5.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d6.GetconnectorDuct(1), d5.GetconnectorDuct(1));
        }
        private static void ConnectorRoute6(Document doc, Duct d1, Duct d2, Duct d3, Duct d4, Duct d5)
        {
            doc.Create.NewElbowFitting(d1.GetconnectorDuct(1), d2.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d2.GetconnectorDuct(1), d3.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d3.GetconnectorDuct(1), d4.GetconnectorDuct(0));
            doc.Create.NewElbowFitting(d4.GetconnectorDuct(1), d5.GetconnectorDuct(0));
        }

    }

}
