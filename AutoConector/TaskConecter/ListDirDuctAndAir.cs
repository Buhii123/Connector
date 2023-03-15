using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.TaskConecter
{
    public static class ListDirDuctAndAir
    {
        public static Dictionary<Connector, Connector> DictionaryAirToConnectorF(Document doc, List<MEPModel> airterminal, List<Connector> ConnectersFasu, Connector OriginConnectorIN, XYZ point, bool basis, out List<Connector> ExcessAir, out List<Connector> ConDu)
        {
            List<Connector> newConnectorAir = new List<Connector>();
            List<Connector> newConnecterFasu = new List<Connector>();
            List<Connector> Excess = new List<Connector>();


            Dictionary<Connector, Connector> ver = new Dictionary<Connector, Connector>();


            if (OriginConnectorIN.SetDirection(doc, point).IsAlmostEqualTo(XYZ.BasisX.Negate()) || OriginConnectorIN.SetDirection(doc, point).IsAlmostEqualTo(XYZ.BasisX))
            {
                if (basis)
                {
                    foreach (MEPModel model in airterminal) if (model.GetConnectior().CheckDirectionY(OriginConnectorIN.Origin.Y)) newConnectorAir.Add(model.GetConnectior());
                    foreach (Connector c in ConnectersFasu) if (c.CheckDirectionY(OriginConnectorIN.Origin.Y)) newConnecterFasu.Add(c);
                }
                if (!basis)
                {
                    foreach (MEPModel model in airterminal) if (!model.GetConnectior().CheckDirectionY(OriginConnectorIN.Origin.Y)) newConnectorAir.Add(model.GetConnectior());
                    foreach (Connector c in ConnectersFasu) if (!c.CheckDirectionY(OriginConnectorIN.Origin.Y)) newConnecterFasu.Add(c);
                }
                List<Connector> newConnectorAir2 = newConnectorAir.Sort(doc, OriginConnectorIN, point).Sort2(doc, OriginConnectorIN, point, basis);
                List<Connector> newConnecterFasu2 = newConnecterFasu.SortFasu(doc, OriginConnectorIN, point);
                ConDu = newConnecterFasu2;


                for (int i = 0; i < newConnectorAir2.Count; i++)
                {
                    if (i > 3) Excess.Add(newConnectorAir2[i]);

                    else ver.Add(newConnectorAir2[i], newConnecterFasu2[i]);
                }
                ExcessAir = Excess;
                return ver;
            }
            if (OriginConnectorIN.SetDirection(doc, point).IsAlmostEqualTo(XYZ.BasisY.Negate()) || OriginConnectorIN.SetDirection(doc, point).IsAlmostEqualTo(XYZ.BasisY))
            {
                if (basis)
                {
                    foreach (MEPModel model in airterminal) if (model.GetConnectior().CheckDirectionX(OriginConnectorIN.Origin.X)) newConnectorAir.Add(model.GetConnectior());
                    foreach (Connector c in ConnectersFasu) if (c.CheckDirectionX(OriginConnectorIN.Origin.X)) newConnecterFasu.Add(c);
                }
                if (!basis)
                {
                    foreach (MEPModel model in airterminal) if (!model.GetConnectior().CheckDirectionX(OriginConnectorIN.Origin.X)) newConnectorAir.Add(model.GetConnectior());
                    foreach (Connector c in ConnectersFasu) if (!c.CheckDirectionX(OriginConnectorIN.Origin.X)) newConnecterFasu.Add(c);
                }
                List<Connector> newConnectorAir2 = newConnectorAir.Sort(doc, OriginConnectorIN, point).Sort2(doc, OriginConnectorIN, point, basis);
                List<Connector> newConnecterFasu2 = newConnecterFasu.SortFasu(doc, OriginConnectorIN, point);
                ConDu = newConnecterFasu2;
                for (int i = 0; i < newConnectorAir2.Count; i++)
                {
                    if (i > 3) Excess.Add(newConnectorAir2[i]);

                    else ver.Add(newConnectorAir2[i], newConnecterFasu2[i]);

                }
                ExcessAir = Excess;
                return ver;
            }

            ExcessAir = null;
            ConDu = null;
            return null;

        }






    }

}
