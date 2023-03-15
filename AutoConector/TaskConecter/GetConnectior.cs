using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.TaskConecter
{
    public static class GetConnectiorModel
    {

        public static Connector GetconnectorDuct(this Duct d, int index)
        {
            var point = (((LocationCurve)d.Location).Curve).GetEndPoint(index);
            foreach (Connector c in d.ConnectorManager.Connectors) if (c.Origin.IsAlmostEqualTo(point)) return c;
            return null;
        }
        public static Connector GetConnectior(this MEPModel model)
        {
            foreach (Connector c in model.ConnectorManager.Connectors) return c;
            return null;
        }

        public static XYZ SetPointZ(this Connector m, XYZ point) => new XYZ(m.Origin.X, m.Origin.Y, point.Z);

        public static XYZ SetPointT(this Connector c1, Connector c2, bool b = true)
        {
            if (b) return new XYZ(c1.Origin.X, c2.Origin.Y, c1.Origin.Z);
            return new XYZ(c2.Origin.X, c1.Origin.Y, c1.Origin.Z);
        }

        public static bool CheckDirectionY(this Connector c, double Y)
        {
            if (c.Origin.Y > Y) { return true; }
            return false;
        }

        public static bool CheckDirectionX(this Connector c, double X)
        {
            if (c.Origin.X > X) { return true; }
            return false;
        }

        public static double origin(this Connector c, bool b)
        {
            if (b) return c.Origin.X;
            return c.Origin.Y;
        }

        public static XYZ SetDirection(this Connector origin, Document doc, XYZ point)
        {
            XYZ a;
            using (Transaction tr = new Transaction(doc, "Line"))
            {
                tr.Start();
                Line li = Line.CreateBound(new XYZ(point.X, point.Y, origin.Origin.Z), origin.Origin);
                a = li.Direction;
                tr.Commit();
            }
            return a;
        }

        public static double CornerAir(this Connector connector1, Connector connectorOrigin, XYZ BasisVector)
        {
            XYZ newPoint = new XYZ(connector1.Origin.X, connector1.Origin.Y, connectorOrigin.Origin.Z);
            XYZ get = connectorOrigin.Origin.Subtract(newPoint);
            return get.AngleTo(BasisVector);
        }
        public static double CornerConnecF(this Connector connector1, Connector connectorOrigin, XYZ BasisVector)
        {
            XYZ get = connectorOrigin.Origin.Subtract(connector1.Origin);
            return get.AngleTo(BasisVector);
        }

        public static List<Connector> Sort(this List<Connector> connectors, Document doc, Connector OriginConnectorIN, XYZ point) => connectors.OrderByDescending(c => c.CornerAir(OriginConnectorIN, OriginConnectorIN.SetDirection(doc, point))).ToList();

        public static List<Connector> Sort2(this List<Connector> connectors, Document doc, Connector OriginConnectorIN, XYZ point, bool basis)
        {

            for (int i = 0; i < connectors.Count; i++)
            {
                if (i < 1)
                {

                    if ((i + 1 < connectors.Count) && ConditionToDraw.CaseSort(connectors[i], connectors[i + 1], OriginConnectorIN, point, basis))
                    {
                        Connector c1;
                        c1 = connectors[i];
                        connectors[i] = connectors[i + 1];
                        connectors[i + 1] = c1;

                    }
                }

            }
            return connectors;
        }

        public static List<Connector> SortFasu(this List<Connector> connectors, Document doc, Connector OriginConnectorIN, XYZ point) => connectors.OrderByDescending(c => c.CornerConnecF(OriginConnectorIN, OriginConnectorIN.SetDirection(doc, point))).ToList();
        public static void Route(this Dictionary<Connector, Connector> a, Document doc, List<Connector> conFCU, ElementId DuctTypeId, ElementId levelId, Connector OriginIN, XYZ point, bool basis)
        {

            for (int i = 0; i < a.Count; i++)
            {
                int j = i + 1;


                if (ConditionToDraw.CaseAirAlignedToF(a.ElementAt(i).Key, a.ElementAt(i).Value, basis))
                {
                    a.ElementAt(i).Key.Route2(doc, a.ElementAt(i).Value, DuctTypeId, levelId, basis);
                }
                //trường hợp conector cuối cùng nằm song song với F IN.
                else if (3 == i)
                {
                    //trường hợp airterminal nằm thẳng hàng với conector cuối cùng đi Route2
                    if (ConditionToDraw.CaseAirAlignedToF(a.ElementAt(i).Key, a.ElementAt(i).Value, !basis))
                    {
                        a.ElementAt(i).Key.Route2(doc, a.ElementAt(i).Value, DuctTypeId, levelId, !basis);
                    }
                    else if (ConditionToDraw.CaseLengthALessthanF(a.ElementAt(i).Key, a.ElementAt(i).Value, !basis))
                    {
                        a.ElementAt(i).Key.Route5(doc, a.ElementAt(i).Value, DuctTypeId, levelId, point, !basis, i);

                    }
                    //trường hợp bình thường đi Route1
                    else
                    {
                        a.ElementAt(i).Key.Route1(doc, a.ElementAt(i).Value, DuctTypeId, levelId, !basis);
                    }
                }

                else if (i != 0 && ConditionToDraw.CaseBelow(conFCU[1], a.ElementAt(i).Key, a.ElementAt(i - 1).Key, basis) && ConditionToDraw.CaseAirAlignedToF(a.ElementAt(i).Key, a.ElementAt(i - 1).Key, !basis))
                {
                    a.ElementAt(i).Key.Route3(doc, a.ElementAt(i).Value, DuctTypeId, levelId, basis, j);
                }

                else if ((i + 1) < a.Count && ConditionToDraw.CaseAbove(conFCU[1], a.ElementAt(i).Key, a.ElementAt(i + 1).Key, basis) && ConditionToDraw.CaseAirAlignedToF(a.ElementAt(i).Key, a.ElementAt(i + 1).Key, !basis))
                {
                    a.ElementAt(i).Key.Route3(doc, a.ElementAt(i).Value, DuctTypeId, levelId, basis, j);

                }
                else if (i == 0 && ConditionToDraw.CaseLengthALessthanF(a.ElementAt(i).Key, a.ElementAt(i).Value, basis))
                {

                    a.ElementAt(i).Key.Route6(doc, a.ElementAt(i).Value, DuctTypeId, levelId, OriginIN, point, basis, i);
                }
                else if (ConditionToDraw.CaseLengthALessthanF(a.ElementAt(i).Key, a.ElementAt(i).Value, basis))
                {

                    a.ElementAt(i).Key.Route5(doc, a.ElementAt(i).Value, DuctTypeId, levelId, point, basis, i);
                }

                //trường hợp bình thường đi Route1
                else
                {

                    a.ElementAt(i).Key.Route1(doc, a.ElementAt(i).Value, DuctTypeId, levelId, basis);

                }

                j++;


            }
        }
        public static void ExcessRoute(this Dictionary<Connector, Connector> a, Document doc, ElementId DuctTypeId, ElementId levelId, XYZ point, bool basis)
        {
            for (int i = 0; i < a.Count; i++)
            {
                int j = i + 2;
                if (Math.Round(point.DistanceTo(a.ElementAt(i).Value.Origin), 5).ToString().Equals("1.48999")) a.ElementAt(i).Key.Route1(doc, a.ElementAt(i).Value, DuctTypeId, levelId, !basis);
                else a.ElementAt(i).Key.Route4(doc, a.ElementAt(i).Value, DuctTypeId, levelId, point, basis, j);
                j++;
            }
        }
    }

}
