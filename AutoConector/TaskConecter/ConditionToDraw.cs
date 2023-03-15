using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConector.TaskConecter
{
    static class ConditionToDraw
    {
        public static bool CaseAirAlignedToF(Connector conA, Connector conF, bool basis)
        {
            if (Math.Round(conA.origin(basis), 5).ToString().Equals(Math.Round(conF.origin(basis), 5).ToString())) return true;
            return false;
        }
        public static bool CaseBelow(Connector origin2, Connector conA1, Connector conA2, bool basis)
        {
            if (basis == true) if ((origin2.origin(basis) < conA1.origin(basis)) && (origin2.origin(basis) < conA2.origin(basis))) return true;

            if (basis == false) if ((origin2.origin(basis) > conA1.origin(basis)) && (origin2.origin(basis) > conA2.origin(basis))) return true;
            return false;
        }
        public static bool CaseAbove(Connector origin2, Connector conA1, Connector conA2, bool basis)
        {
            if (basis == true) if ((origin2.origin(basis) > conA1.origin(basis)) && (origin2.origin(basis) > conA2.origin(basis))) return true;

            if (basis == false) if ((origin2.origin(basis) < conA1.origin(basis)) && (origin2.origin(basis) < conA2.origin(basis))) return true;

            return false;
        }
        public static bool CaseLengthALessthanF(Connector conA, Connector conF, bool basis)
        {
            if (basis == true) if (new XYZ(conA.Origin.X, conA.Origin.Y, conF.Origin.Z).DistanceTo(new XYZ(conF.Origin.X, conA.Origin.Y, conF.Origin.Z)) < 1) return true;

            if (basis == false) if (new XYZ(conA.Origin.X, conA.Origin.Y, conF.Origin.Z).DistanceTo(new XYZ(conA.Origin.X, conF.Origin.Y, conF.Origin.Z)) < 1) return true;

            return false;
        }
        public static bool CaseSort(Connector conAir1, Connector conAir2, Connector Origin, XYZ point, bool basis)
        {

            if (((conAir1.Origin.X > point.X) && (conAir2.Origin.X > point.X)) || ((conAir1.Origin.X < point.X) && (conAir2.Origin.X < point.X)))
            {
                if ((conAir1.Origin.Y > point.Y) && (conAir2.Origin.Y > point.Y)) if (conAir1.Origin.Y > conAir2.Origin.Y) return true;

                if ((conAir1.Origin.Y < point.Y) && (conAir2.Origin.Y < point.Y)) if ((conAir1.Origin.Y < conAir2.Origin.Y)) return true;

            }

            return false;
        }


    }

}
